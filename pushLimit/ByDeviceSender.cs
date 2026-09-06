using System.Diagnostics;

namespace PushLimit;

/// <summary>
/// ByDevice 发送器 — 按设备令牌（Device Token）批量发送推送。
///
/// 一条消息内显式包含多个设备令牌直接发送。
/// 发送速率受 DeviceRateController 控制，根据信号灯状态在
/// 全速（600K/min）和阻塞（120K/min）之间动态切换。
/// </summary>
public sealed class ByDeviceSender
{
    private readonly int _threadId;
    private readonly DeviceRateController _rateController;

    public ByDeviceSender(int threadId, DeviceRateController rateController)
    {
        _threadId = threadId;
        _rateController = rateController;
    }

    /// <summary>
    /// 执行一次完整的 ByDevice 发送流程（含速率控制）。
    /// </summary>
    /// <param name="deviceTokens">设备令牌列表</param>
    public void Send(List<string> deviceTokens, CancellationToken ct)
    {
        int count = deviceTokens.Count;

        // 速率控制：等待直到有发送配额
        bool wasFullSpeed = _rateController.IsFullSpeed;
        _rateController.WaitForSlot(count, ct);

        // 执行实际发送
        var sw = Stopwatch.StartNew();
        SendToFcm(deviceTokens);
        sw.Stop();

        // 更新统计
        GlobalCounter.Instance.IncrementDeviceStats(count);

        if (_threadId < 5) // 只打印前 5 个线程
        {
            string mode = wasFullSpeed ? "FULL" : "SLOW";
            Console.WriteLine(
                $"  [ByDev] T{_threadId:D3} | tokens={count,5} " +
                $"mode={mode} send={sw.ElapsedMilliseconds,3}ms");
        }
    }

    /// <summary>
    /// 模拟 FCM ByDevice API 调用。
    /// 真实场景中此处为 HTTP POST 到 FCM send endpoint，
    /// body 中包含 registration_ids 数组。
    /// </summary>
    private void SendToFcm(List<string> deviceTokens)
    {
        // 模拟网络延迟：2–8ms
        Thread.Sleep(Random.Shared.Next(2, 9));
    }
}
