using System.Diagnostics;

namespace PushLimit;

/// <summary>
/// 全局计数器 / 时间戳存储。
///
/// 核心职责：
///   1. 提供高精度单调时钟（基于 Stopwatch），所有组件使用同一时间源。
///   2. 存储上一次 ByTag 发送结束时间戳（LastEndTimeMs），
///      供 TagRateLimiter 计算 Waiting Window 时使用。
///   3. 统计 ByTag / ByDevice 的发送次数与设备总数（用于运行报表）。
/// </summary>
public sealed class GlobalCounter
{
    private static readonly Lazy<GlobalCounter> _instance =
        new(() => new GlobalCounter());

    public static GlobalCounter Instance => _instance.Value;

    private readonly Stopwatch _stopwatch = new();
    private readonly object _lock = new();

    /// <summary>上一次 ByTag 发送结束的时间戳（ms）</summary>
    public long LastEndTimeMs { get; private set; }

    // ── 统计计数 ──────────────────────────────────────────
    public long TotalTagSends { get; private set; }
    public long TotalTagDevices { get; private set; }
    public long TotalDeviceSends { get; private set; }
    public long TotalDeviceMessages { get; private set; }

    private GlobalCounter() { }

    /// <summary>启动全局时钟</summary>
    public void Start()
    {
        _stopwatch.Restart();
    }

    /// <summary>程序启动以来经过的毫秒数</summary>
    public long ElapsedMs => _stopwatch.ElapsedMilliseconds;

    /// <summary>ByTag 发送完成后记录结束时间戳</summary>
    public void RecordEndTime(long endTimeMs)
    {
        lock (_lock)
        {
            LastEndTimeMs = endTimeMs;
        }
    }

    /// <summary>记录一次 ByTag 发送</summary>
    public void IncrementTagStats(int deviceCount)
    {
        lock (_lock)
        {
            TotalTagSends++;
            TotalTagDevices += deviceCount;
        }
    }

    /// <summary>记录一次 ByDevice 发送</summary>
    public void IncrementDeviceStats(int messageCount)
    {
        lock (_lock)
        {
            TotalDeviceSends++;
            TotalDeviceMessages += messageCount;
        }
    }

    /// <summary>获取统计快照</summary>
    public (long TagSends, long TagDevices, long DeviceSends, long DeviceMessages, long ElapsedMs) GetSnapshot()
    {
        lock (_lock)
        {
            return (TotalTagSends, TotalTagDevices,
                    TotalDeviceSends, TotalDeviceMessages,
                    _stopwatch.ElapsedMilliseconds);
        }
    }
}
