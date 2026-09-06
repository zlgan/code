using PushLimit;

// ─────────────────────────────────────────────────────────────
//  .NET 消息推送系统 — 并发模拟入口
//
//  100 个线程并发执行 ByTag / ByDevice 发送，
//  通过 TrafficLight + DeviceRateController + TagRateLimiter
//  保证总速率不超过 600K 条/分钟。
// ─────────────────────────────────────────────────────────────

const int SimulationDurationSec = 30;  // 模拟运行时长（秒）
const int VmCount = PushLimitConfig.VmCount;
const int ThreadCount = PushLimitConfig.ThreadCount;

// 示例标签池
string[] tags = ["news", "sports", "tech", "finance", "weather", "social", "promo"];

Console.WriteLine("╔══════════════════════════════════════════════════╗");
Console.WriteLine("║   .NET Push Notification System — FCM Simulation ║");
Console.WriteLine("╠══════════════════════════════════════════════════╣");
Console.WriteLine($"║  Global Rate Limit : {PushLimitConfig.GlobalRateLimitPerMin:N0} msg/min             ║");
Console.WriteLine($"║  Thread Count      : {ThreadCount}                                ║");
Console.WriteLine($"║  Tag Quota (80%)   : {PushLimitConfig.TagDeviceLimitPerMin:N0} devices/min          ║");
Console.WriteLine($"║  Device Full (100%): {PushLimitConfig.DeviceFullSpeedPerMin:N0} msg/min             ║");
Console.WriteLine($"║  Device Block (20%): {PushLimitConfig.DeviceBlockingSpeedPerMin:N0} msg/min             ║");
Console.WriteLine($"║  Simulation        : {SimulationDurationSec}s                              ║");
Console.WriteLine($"║  VMs               : {VmCount}                                 ║");
Console.WriteLine("╚══════════════════════════════════════════════════╝");
Console.WriteLine();

// ── 初始化组件 ──────────────────────────────────────────────
GlobalCounter.Instance.Start();

var deviceRateController = new DeviceRateController();
deviceRateController.Start();

// ── 启动 100 个工作线程 ─────────────────────────────────────
var cts = new CancellationTokenSource();
var threads = new Thread[ThreadCount];

Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Starting {ThreadCount} worker threads...\n");

for (int i = 0; i < ThreadCount; i++)
{
    int threadId = i;
    threads[i] = new Thread(() => WorkerLoop(threadId, cts.Token))
    {
        IsBackground = true,
        Name = $"Worker-{threadId:D3}"
    };
    threads[i].Start();
}

// ── 监控线程：每 5 秒打印一次统计 ────────────────────────────
var monitorThread = new Thread(() => MonitorLoop(cts.Token))
{
    IsBackground = true,
    Name = "Monitor"
};
monitorThread.Start();

// ── 等待模拟结束 ────────────────────────────────────────────
Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Running for {SimulationDurationSec} seconds...\n");
Thread.Sleep(SimulationDurationSec * 1000);

// ── 停止所有线程 ────────────────────────────────────────────
Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] Stopping workers...");
cts.Cancel();

foreach (var t in threads)
{
    if (t.IsAlive) t.Join(3000);
}
monitorThread.Join(2000);

deviceRateController.Stop();

// ── 最终统计 ────────────────────────────────────────────────
PrintFinalStats();

// ═══════════════════════════════════════════════════════════
//  Worker Loop — 每个线程随机执行 ByTag 或 ByDevice 发送
// ═══════════════════════════════════════════════════════════
void WorkerLoop(int threadId, CancellationToken ct)
{
    var tagSender = new ByTagSender(threadId, VmCount);
    var deviceSender = new ByDeviceSender(threadId, deviceRateController);

    // 线程级随机数生成器
    var rng = new Random(threadId * 31 + 7);

    while (!ct.IsCancellationRequested)
    {
        try
        {
            // 70% 概率走 ByTag，30% 概率走 ByDevice
            if (rng.NextDouble() < 0.7)
            {
                // ── ByTag 发送 ──
                string tag = tags[rng.Next(tags.Length)];

                // 模拟标签下的设备数：500–5000
                int deviceCount = rng.Next(500, 5001);

                tagSender.Send(tag, deviceCount, ct);
            }
            else
            {
                // ── ByDevice 发送 ──
                int tokenCount = rng.Next(10, 101); // 10–100 个令牌
                var tokens = new List<string>(tokenCount);
                for (int j = 0; j < tokenCount; j++)
                {
                    tokens.Add($"fcm_token_{rng.Next(100000, 999999):X6}");
                }

                deviceSender.Send(tokens, ct);
            }
        }
        catch (OperationCanceledException)
        {
            break; // 正常退出
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"  [ERROR] T{threadId:D3}: {ex.Message}");
        }
    }
}

// ═══════════════════════════════════════════════════════════
//  Monitor Loop — 定期打印运行状态
// ═══════════════════════════════════════════════════════════
void MonitorLoop(CancellationToken ct)
{
    while (!ct.IsCancellationRequested)
    {
        ct.WaitHandle.WaitOne(5000);
        if (ct.IsCancellationRequested) break;

        var (tagSends, tagDevices, devSends, devMsgs, elapsedMs) =
            GlobalCounter.Instance.GetSnapshot();

        var (lightActive, lightStart, lightEnd) =
            TrafficLightService.Instance.GetStatus();

        string lightStr = lightActive ? "🔴 ON " : "🟢 OFF";
        double elapsedSec = elapsedMs / 1000.0;
        double tagRate = elapsedSec > 0 ? tagDevices / elapsedSec * 60 : 0;
        double devRate = elapsedSec > 0 ? devMsgs / elapsedSec * 60 : 0;
        string devMode = deviceRateController.IsFullSpeed ? "FULL" : "SLOW";

        Console.WriteLine(
            $"─── [{DateTime.Now:HH:mm:ss}] elapsed={elapsedSec,5:F1}s " +
            $"light={lightStr} " +
            $"| Tag: {tagSends,4} sends  {tagDevices,10:N0} devs  ({tagRate,10:N0}/min) " +
            $"| Dev: {devSends,4} sends  {devMsgs,8:N0} msgs  ({devRate,10:N0}/min) [{devMode}] ───");
    }
}

// ═══════════════════════════════════════════════════════════
//  打印最终统计报表
// ═══════════════════════════════════════════════════════════
void PrintFinalStats()
{
    var (tagSends, tagDevices, devSends, devMsgs, elapsedMs) =
        GlobalCounter.Instance.GetSnapshot();

    double elapsedMin = elapsedMs / 60_000.0;

    Console.WriteLine();
    Console.WriteLine("╔══════════════════════════════════════════════════╗");
    Console.WriteLine("║              FINAL STATISTICS                    ║");
    Console.WriteLine("╠══════════════════════════════════════════════════╣");
    Console.WriteLine($"║  Duration          : {elapsedMs / 1000.0,8:F1} s ({elapsedMin:F3} min)          ║");
    Console.WriteLine($"║                                                  ║");
    Console.WriteLine($"║  ByTag  Sends      : {tagSends,8:N0}                      ║");
    Console.WriteLine($"║  ByTag  Devices    : {tagDevices,10:N0}                    ║");

    double tagActualRate = elapsedMin > 0 ? tagDevices / elapsedMin : 0;
    Console.WriteLine($"║  ByTag  Rate       : {tagActualRate,10:N0} /min                      ║");
    Console.WriteLine($"║  ByTag  Limit      : {PushLimitConfig.TagDeviceLimitPerMin,10:N0} /min                      ║");

    Console.WriteLine($"║                                                  ║");
    Console.WriteLine($"║  ByDevice Sends    : {devSends,8:N0}                      ║");
    Console.WriteLine($"║  ByDevice Messages : {devMsgs,10:N0}                    ║");

    double devActualRate = elapsedMin > 0 ? devMsgs / elapsedMin : 0;
    Console.WriteLine($"║  ByDevice Rate     : {devActualRate,10:N0} /min                      ║");

    Console.WriteLine($"║                                                  ║");
    double totalMsgs = tagDevices + devMsgs;
    double totalRate = elapsedMin > 0 ? totalMsgs / elapsedMin : 0;
    Console.WriteLine($"║  Total Messages    : {totalMsgs,10:N0}                    ║");
    Console.WriteLine($"║  Total Rate        : {totalRate,10:N0} /min                      ║");
    Console.WriteLine($"║  Global Limit      : {PushLimitConfig.GlobalRateLimitPerMin,10:N0} /min                      ║");
    Console.WriteLine("╚══════════════════════════════════════════════════╝");
}
