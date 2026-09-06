namespace PushLimit;

/// <summary>
/// ByDevice 速率控制器。
///
/// 内部以 1 秒间隔轮询 TrafficLightService，动态切换两种速率：
///   - 全速模式（Full-Speed）：600K/分钟  → 10,000 条/秒
///   - 阻塞模式（Blocking） ：120K/分钟  →  2,000 条/秒
///
/// ByDevice 发送线程在每次发送前调用 WaitForSlot()，
/// 该方法以短轮询方式等待，保证在信号灯状态变化时能快速响应降速。
/// </summary>
public sealed class DeviceRateController
{
    private readonly object _lock = new();

    /// <summary>当前是否处于全速模式</summary>
    private volatile bool _isFullSpeed = true;

    /// <summary>后台采样定时器</summary>
    private Timer? _samplingTimer;

    public DeviceRateController()
    {
    }

    /// <summary>
    /// 启动 1 秒间隔的采样定时器。
    /// 每秒读取信号灯状态，更新当前速率模式。
    /// </summary>
    public void Start()
    {
        _samplingTimer = new Timer(
            callback: SampleTrafficLight,
            state: null,
            dueTime: 0,
            period: PushLimitConfig.SamplingIntervalMs);
    }

    /// <summary>停止采样定时器</summary>
    public void Stop()
    {
        _samplingTimer?.Change(Timeout.Infinite, Timeout.Infinite);
        _samplingTimer?.Dispose();
    }

    /// <summary>当前是否处于全速模式</summary>
    public bool IsFullSpeed => _isFullSpeed;

    /// <summary>当前允许的全局速率（条/秒）</summary>
    public double CurrentRatePerSec =>
        _isFullSpeed
            ? PushLimitConfig.DeviceFullSpeedPerSec
            : PushLimitConfig.DeviceBlockingSpeedPerSec;

    /// <summary>
    /// ByDevice 线程调用：等待直到有发送配额。
    ///
    /// 使用短轮询（≤200ms 一次）检查信号灯状态，
    /// 确保在 ByTag 开启信号灯后能快速降速，而不是等到长 Sleep 结束。
    /// </summary>
    /// <param name="messageCount">本次请求包含的消息数</param>
    public void WaitForSlot(int messageCount, CancellationToken ct)
    {
        while (true)
        {
            ct.ThrowIfCancellationRequested();

            // 每次循环都重新检查信号灯，实现快速响应
            bool isFullSpeedNow = !TrafficLightService.Instance.IsOn;
            if (isFullSpeedNow != _isFullSpeed)
            {
                lock (_lock) { _isFullSpeed = isFullSpeedNow; }
            }

            // 计算当前模式下单个线程每秒允许的消息数
            double globalPerSec = isFullSpeedNow
                ? PushLimitConfig.DeviceFullSpeedPerSec
                : PushLimitConfig.DeviceBlockingSpeedPerSec;
            double perThreadPerSec = globalPerSec / PushLimitConfig.ThreadCount;

            // 本次请求需要的等待时间
            double neededMs = (messageCount / perThreadPerSec) * 1000.0;

            // 短等待：最多等 200ms，然后重新检查信号灯
            // 这样在模式切换时不会被困在长 Sleep 中
            if (neededMs <= 200)
            {
                if (neededMs > 0)
                    ct.WaitHandle.WaitOne((int)Math.Ceiling(neededMs));
                return;
            }

            // 需要更长等待 → 只等 200ms 后重新评估
            ct.WaitHandle.WaitOne(200);
        }
    }

    /// <summary>定时器回调：每秒读取信号灯状态</summary>
    private void SampleTrafficLight(object? state)
    {
        bool lightOn = TrafficLightService.Instance.IsOn;
        bool shouldBeFullSpeed = !lightOn;

        if (shouldBeFullSpeed != _isFullSpeed)
        {
            lock (_lock)
            {
                _isFullSpeed = shouldBeFullSpeed;
            }
        }
    }
}
