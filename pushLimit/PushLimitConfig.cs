namespace PushLimit;

/// <summary>
/// 全局配置常量 — 所有硬约束在此定义，不可在其它文件中硬编码。
/// </summary>
public static class PushLimitConfig
{
    // ── 全局约束 ──────────────────────────────────────────
    /// <summary>系统总发送速率上限（条/分钟）</summary>
    public const int GlobalRateLimitPerMin = 600_000;

    /// <summary>并发线程数</summary>
    public const int ThreadCount = 100;

    // ── ByTag 配额 ────────────────────────────────────────
    /// <summary>ByTag 占总配额的比例</summary>
    public const double TagQuotaRatio = 0.80;

    /// <summary>ByTag 每分钟最大设备数 = 600K × 80% = 480,000</summary>
    public const int TagDeviceLimitPerMin = (int)(GlobalRateLimitPerMin * TagQuotaRatio); // 480_000

    // ── ByDevice 双速率 ───────────────────────────────────
    /// <summary>全速模式速率（条/分钟）= 600K × 100%</summary>
    public const int DeviceFullSpeedPerMin = GlobalRateLimitPerMin; // 600_000

    /// <summary>阻塞模式速率比例</summary>
    public const double DeviceBlockingRatio = 0.20;

    /// <summary>阻塞模式速率（条/分钟）= 600K × 20% = 120,000</summary>
    public const int DeviceBlockingSpeedPerMin = (int)(GlobalRateLimitPerMin * DeviceBlockingRatio); // 120_000

    // ── 时间参数 ──────────────────────────────────────────
    /// <summary>信号灯采样间隔（毫秒），每秒读取一次</summary>
    public const int SamplingIntervalMs = 1_000;

    /// <summary>部署虚拟机数量（单机 = 1）</summary>
    public const int VmCount = 1;

    // ── 派生常量 ──────────────────────────────────────────
    /// <summary>
    /// 单个 ByTag 线程每秒允许的设备数。
    /// = TagDeviceLimitPerMin / VmCount / ThreadCount / 60
    /// = 480000 / 1 / 100 / 60 = 80 设备/秒
    /// </summary>
    public static double TagDevicesPerThreadPerSecond =>
        (double)TagDeviceLimitPerMin / VmCount / ThreadCount / 60.0;

    /// <summary>全速模式下 ByDevice 每秒允许的消息数 = 10,000</summary>
    public static double DeviceFullSpeedPerSec =>
        (double)DeviceFullSpeedPerMin / 60.0;

    /// <summary>阻塞模式下 ByDevice 每秒允许的消息数 = 2,000</summary>
    public static double DeviceBlockingSpeedPerSec =>
        (double)DeviceBlockingSpeedPerMin / 60.0;
}
