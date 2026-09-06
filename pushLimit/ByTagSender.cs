using System.Diagnostics;

namespace PushLimit;

/// <summary>
/// ByTag 发送器 — 按用户订阅的标签（Tag）发送推送。
///
/// 一条请求对应 FCM 后端拆分的海量设备，因此一次发送即代表大量设备。
/// 每个线程持有独立的 TagRateLimiter 实例进行速率控制。
/// </summary>
public sealed class ByTagSender
{
    private readonly int _threadId;
    private readonly TagRateLimiter _rateLimiter;

    public ByTagSender(int threadId, int vmCount = 1)
    {
        _threadId = threadId;
        _rateLimiter = new TagRateLimiter(threadId, vmCount);
    }

    /// <summary>
    /// 执行一次完整的 ByTag 发送流程（含速率控制）。
    /// </summary>
    /// <param name="tag">目标标签名</param>
    /// <param name="deviceCount">该标签下的设备总数</param>
    public void Send(string tag, int deviceCount, CancellationToken ct)
    {
        // Step 1 & 2: Waiting Window 等待 + 开启信号灯 + 速率级 Sleep
        int waitingMs = _rateLimiter.WaitBeforeSend(deviceCount, ct);

        // Step 3: 执行实际发送，计时
        var sw = Stopwatch.StartNew();
        SendToFcm(tag, deviceCount);
        sw.Stop();

        // Step 4: 记录耗时，关闭信号灯
        _rateLimiter.RecordSendComplete(sw.Elapsed.TotalMilliseconds, deviceCount);

        if (_threadId < 5) // 只打印前 5 个线程，避免日志爆炸
        {
            Console.WriteLine(
                $"  [ByTag] T{_threadId:D3} | tag={tag,-12} " +
                $"devices={deviceCount,6:N0} | wait={waitingMs,4}ms " +
                $"send={sw.ElapsedMilliseconds,4}ms");
        }
    }

    /// <summary>
    /// 模拟 FCM ByTag API 调用。
    /// 真实场景中此处为 HTTP POST 到 FCM send endpoint。
    /// </summary>
    private void SendToFcm(string tag, int deviceCount)
    {
        // 模拟网络延迟：5–15ms
        Thread.Sleep(Random.Shared.Next(5, 16));
    }
}
