using System.Diagnostics;

namespace PushLimit;

/// <summary>
/// 简化版推送限流发送器 — 站在单个线程的视角，用一个函数完成发送。
///
/// 对比原版的 5 个类（TrafficLightService / TagRateLimiter / DeviceRateController /
/// ByTagSender / ByDeviceSender），这里只用一个 SendPush() 函数 + 一个模式枚举。
///
/// 核心思路：
///   不管是 ByTag 还是 ByDevice，线程要做的事就三步：
///     1. 等（根据模式不同，等的策略不同）
///     2. 发（调用方传入的 Action）
///     3. 记（更新全局统计）
///
/// 线程安全说明：
///   - _lastTagSendDurationMs 是线程局部状态（每个线程一个 SimplePushSender 实例），无需加锁。
///   - TrafficLightService / GlobalCounter 本身是线程安全单例。
/// </summary>
public sealed class SimplePushSender
{
    private readonly int _threadId;
    private readonly int _vmCount;

    /// <summary>上次 ByTag 发送的实际耗时（ms），线程局部，用于下次 Sleep 补偿</summary>
    private double _lastTagSendDurationMs;

    public SimplePushSender(int threadId, int vmCount = 1)
    {
        _threadId = threadId;
        _vmCount = vmCount;
    }

    /// <summary>
    /// 统一发送入口 — 一个函数处理 ByTag 和 ByDevice 两种模式的全部限流逻辑。
    ///
    /// 用法：
    ///   sender.SendPush(SendMode.ByTag, deviceCount, () => fcm.SendByTag(tag), ct);
    ///   sender.SendPush(SendMode.ByDevice, tokenCount, () => fcm.SendByDevice(tokens), ct);
    /// </summary>
    /// <param name="mode">发送模式：ByTag 或 ByDevice</param>
    /// <param name="messageCount">
    ///   ByTag = 该 Tag 下的设备总数；
    ///   ByDevice = 本次请求的消息/令牌数
    /// </param>
    /// <param name="sendAction">实际的 FCM 发送逻辑（HTTP 调用等）</param>
    /// <param name="ct">取消令牌</param>
    public void SendPush(SendMode mode, int messageCount, Action sendAction, CancellationToken ct)
    {
        if (mode == SendMode.ByTag)
            SendByTag(messageCount, sendAction, ct);
        else
            SendByDevice(messageCount, sendAction, ct);
    }

    // ──────────────────────────────────────────────────────────
    // ByTag：一个线程发 Tag 消息的完整流程
    // ──────────────────────────────────────────────────────────

    private void SendByTag(int deviceCount, Action sendAction, CancellationToken ct)
    {
        // ── 第一步：等 ──────────────────────────────────────

        // 1a. Waiting Window — 给 ByDevice 留时间降速
        //     BlockedWindow = (deviceCount / 480K) × 60s → 按限速处理这些设备需要的时间
        //     WaitingWindow = min(距上次ByTag结束的间隔, BlockedWindow) + 1ms
        double blockedWindowMs =
            (double)deviceCount / PushLimitConfig.TagDeviceLimitPerMin * 60_000.0;

        long gapMs = GlobalCounter.Instance.LastEndTimeMs > 0
            ? GlobalCounter.Instance.ElapsedMs - GlobalCounter.Instance.LastEndTimeMs
            : 0;

        int waitingMs = (int)Math.Ceiling(Math.Min(gapMs, blockedWindowMs) + 1);
        if (waitingMs > 0)
            ct.WaitHandle.WaitOne(waitingMs);
        ct.ThrowIfCancellationRequested();

        // 1b. 亮红灯 — 通知所有 ByDevice 线程降速
        TrafficLightService.Instance.TurnOn();

        // 1c. 速率级 Sleep — 保证长期平均速率 ≤ 480K/min/VM/线程
        //     目标间隔 = deviceCount / 单线程速率 × 1000
        //     实际 Sleep = 目标间隔 − 上次发送耗时（补偿机制）
        double perThreadPerSec =
            (double)PushLimitConfig.TagDeviceLimitPerMin / _vmCount / PushLimitConfig.ThreadCount / 60.0;
        double targetMs = deviceCount / perThreadPerSec * 1000.0;
        double sleepMs = targetMs - _lastTagSendDurationMs;
        if (sleepMs > 0)
            ct.WaitHandle.WaitOne((int)Math.Ceiling(sleepMs));
        ct.ThrowIfCancellationRequested();

        // ── 第二步：发 ──────────────────────────────────────
        var sw = Stopwatch.StartNew();
        sendAction();
        sw.Stop();

        // ── 第三步：记 ──────────────────────────────────────
        _lastTagSendDurationMs = sw.Elapsed.TotalMilliseconds;
        TrafficLightService.Instance.TurnOff();
        GlobalCounter.Instance.IncrementTagStats(deviceCount);

        if (_threadId < 5)
            Console.WriteLine(
                $"  [ByTag] T{_threadId:D3} | devices={deviceCount,6:N0} " +
                $"wait={waitingMs,4}ms send={sw.ElapsedMilliseconds,4}ms");
    }

    // ──────────────────────────────────────────────────────────
    // ByDevice：一个线程发 Device 消息的完整流程
    // ──────────────────────────────────────────────────────────

    private void SendByDevice(int messageCount, Action sendAction, CancellationToken ct)
    {
        // ── 第一步：等（短轮询，每次最多 200ms，快速响应信号灯变化）──
        //
        // 核心逻辑：
        //   红灯亮 → ByDevice 降速到 120K/min（2000/sec）
        //   灯灭   → ByDevice 全速 600K/min（10000/sec）
        //
        // 每次循环：读灯 → 算当前速率 → 算需要等多久 → 等一小段 → 再检查
        while (true)
        {
            ct.ThrowIfCancellationRequested();

            double perSec = TrafficLightService.Instance.IsOn
                ? PushLimitConfig.DeviceBlockingSpeedPerSec   // 2,000
                : PushLimitConfig.DeviceFullSpeedPerSec;      // 10,000

            double perThreadPerSec = perSec / PushLimitConfig.ThreadCount;
            double neededMs = messageCount / perThreadPerSec * 1000.0;

            if (neededMs <= 200)
            {
                if (neededMs > 0)
                    ct.WaitHandle.WaitOne((int)Math.Ceiling(neededMs));
                break;
            }

            // 还需要等很久 → 只等 200ms，然后重新检查灯的状态
            ct.WaitHandle.WaitOne(200);
        }

        // ── 第二步：发 ──────────────────────────────────────
        var sw = Stopwatch.StartNew();
        sendAction();
        sw.Stop();

        // ── 第三步：记 ──────────────────────────────────────
        GlobalCounter.Instance.IncrementDeviceStats(messageCount);

        if (_threadId < 5)
            Console.WriteLine(
                $"  [ByDev] T{_threadId:D3} | tokens={messageCount,5} " +
                $"send={sw.ElapsedMilliseconds,3}ms");
    }
}

/// <summary>发送模式</summary>
public enum SendMode
{
    /// <summary>按标签发送（一条请求覆盖大量设备）</summary>
    ByTag,

    /// <summary>按设备令牌发送（显式指定目标设备列表）</summary>
    ByDevice
}
