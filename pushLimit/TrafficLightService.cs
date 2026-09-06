using System.Diagnostics;

namespace PushLimit;

/// <summary>
/// 信号灯服务（线程安全单例）。
///
/// 仅在 ByTag 模式发送时启用：
///   - TurnOn()  → 记录 StartTime，标记 IsActive = true
///   - TurnOff() → 记录 EndTime，  标记 IsActive = false
///
/// ByDevice 通过 IsOn 属性每秒轮询信号灯状态，决定是否降速。
/// </summary>
public sealed class TrafficLightService
{
    private static readonly Lazy<TrafficLightService> _instance =
        new(() => new TrafficLightService());

    public static TrafficLightService Instance => _instance.Value;

    private readonly object _lock = new();
    private volatile bool _isActive;
    private Stopwatch _stopwatch = new();

    /// <summary>信号灯开启时的时间戳（来自 GlobalCounter 的 Stopwatch）</summary>
    public long StartTimeMs { get; private set; }

    /// <summary>信号灯关闭时的时间戳</summary>
    public long EndTimeMs { get; private set; }

    /// <summary>当前信号灯是否开启（即 ByTag 正在发送中）</summary>
    public bool IsOn
    {
        get { lock (_lock) return _isActive; }
    }

    private TrafficLightService() { }

    /// <summary>
    /// 开启信号灯，记录 StartTime。
    /// 由 ByTag 线程在发送前调用。
    /// </summary>
    public void TurnOn()
    {
        lock (_lock)
        {
            _isActive = true;
            StartTimeMs = GlobalCounter.Instance.ElapsedMs;
        }
    }

    /// <summary>
    /// 关闭信号灯，记录 EndTime。
    /// 由 ByTag 线程在发送完成后调用。
    /// </summary>
    public void TurnOff()
    {
        lock (_lock)
        {
            _isActive = false;
            EndTimeMs = GlobalCounter.Instance.ElapsedMs;
            GlobalCounter.Instance.RecordEndTime(EndTimeMs);
        }
    }

    /// <summary>获取当前状态快照（用于日志/监控）</summary>
    public (bool IsActive, long StartTimeMs, long EndTimeMs) GetStatus()
    {
        lock (_lock)
        {
            return (_isActive, StartTimeMs, EndTimeMs);
        }
    }
}
