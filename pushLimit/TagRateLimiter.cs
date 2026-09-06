using System.Diagnostics;

namespace PushLimit;

/// <summary>
/// ByTag 速率限制器 — 每个线程持有独立实例。
///
/// 核心职责：
///   1. 计算 Waiting Window 并执行阻塞等待（让 ByDevice 有时间降速）。
///   2. 管理信号灯（TurnOn / TurnOff）。
///   3. 根据上次实际发送耗时，计算下次发送前的 Sleep 时间，
///      保证长期平均速率 ≤ 480K / VMs / 线程数。
/// </summary>
public sealed class TagRateLimiter
{
    private readonly int _threadId;
    private readonly int _vmCount;
    private readonly object _lock = new();

    /// <summary>上次 ByTag 请求的实际耗时（ms）</summary>
    private double _lastSendDurationMs;

    public TagRateLimiter(int threadId, int vmCount)
    {
        _threadId = threadId;
        _vmCount = vmCount;
    }

    /// <summary>
    /// ByTag 发送的完整控制流程：
    ///
    ///   Step 1 — 计算 Waiting Window 并等待（让 ByDevice 降速）
    ///   Step 2 — 开启信号灯
    ///   Step 3 — 速率级 Sleep（根据上次耗时动态调整）
    ///   Step 4 — 执行实际发送（由调用方在返回值后执行）
    ///
    /// 发送完成后，调用方必须调用 RecordSendComplete() 记录耗时。
    /// </summary>
    /// <param name="deviceCount">本次 Tag 请求包含的设备总数</param>
    /// <returns>Waiting Window 等待毫秒数（用于日志）</returns>
    public int WaitBeforeSend(int deviceCount, CancellationToken ct)
    {
        // ── Step 1: Waiting Window ───────────────────────────
        int waitingWindowMs = CalculateWaitingWindow(deviceCount);
        if (waitingWindowMs > 0)
        {
            // 使用 WaitHandle 替代 Thread.Sleep，支持取消
            ct.WaitHandle.WaitOne(waitingWindowMs);
            ct.ThrowIfCancellationRequested();
        }

        // ── Step 2: 开启信号灯 ───────────────────────────────
        TrafficLightService.Instance.TurnOn();

        // ── Step 3: 速率级 Sleep ─────────────────────────────
        // 单线程目标速率 = 480K / VMs / 100 / 60  设备/秒
        double perThreadPerSec =
            (double)PushLimitConfig.TagDeviceLimitPerMin
            / _vmCount
            / PushLimitConfig.ThreadCount
            / 60.0;

        // 本次请求在目标速率下应占用的时间
        double targetIntervalMs = (deviceCount / perThreadPerSec) * 1000.0;

        // 需要补足的 Sleep = 目标时间 − 上次实际发送耗时
        double sleepMs;
        lock (_lock)
        {
            sleepMs = targetIntervalMs - _lastSendDurationMs;
        }

        if (sleepMs > 0)
        {
            ct.WaitHandle.WaitOne((int)Math.Ceiling(sleepMs));
            ct.ThrowIfCancellationRequested();
        }

        return waitingWindowMs;
    }

    /// <summary>
    /// ByTag 发送完成后调用，记录实际耗时用于下次 Sleep 计算。
    /// 同时关闭信号灯并更新统计。
    /// </summary>
    /// <param name="sendDurationMs">本次发送从开始到结束的耗时（ms）</param>
    /// <param name="deviceCount">本次发送的设备数</param>
    public void RecordSendComplete(double sendDurationMs, int deviceCount)
    {
        lock (_lock)
        {
            _lastSendDurationMs = sendDurationMs;
        }

        TrafficLightService.Instance.TurnOff();
        GlobalCounter.Instance.IncrementTagStats(deviceCount);
    }

    // ── Waiting Window 核心算法 ──────────────────────────────

    /// <summary>
    /// 计算 Waiting Window（毫秒）。
    ///
    ///   Blocked Window = (deviceCount / 480K) × 60 × 1000  ms
    ///   Waiting Window = min(now − lastEndTime, BlockedWindow) + 1  ms
    ///
    /// 目的：预留时间给 ByDevice 降速，防止 ByTag 触发 FCM 429 限流。
    /// </summary>
    private int CalculateWaitingWindow(int deviceCount)
    {
        // Blocked Window：ByTag 按 480K/min 速率处理这些设备所需时间
        double blockedWindowMs =
            ((double)deviceCount / PushLimitConfig.TagDeviceLimitPerMin)
            * 60.0
            * 1000.0;

        // 距上次 ByTag 发送结束的时间间隔
        long lastEnd = GlobalCounter.Instance.LastEndTimeMs;
        long now = GlobalCounter.Instance.ElapsedMs;
        long gapMs = (lastEnd > 0) ? (now - lastEnd) : 0;

        // Waiting Window = min(gap, blockedWindow) + 1
        double waitingMs = Math.Min(gapMs, blockedWindowMs) + 1;

        return (int)Math.Ceiling(waitingMs);
    }
}
