using System.ServiceProcess;
using System.Timers;

public class Program : ServiceBase
{
    private static bool serviceStat = false;
    private static System.Timers.Timer _timer;
    private enum operations { tasks, services }

    public Program()
    {
        this.ServiceName = "Kill.KSK";  // Servis ismi
    }

    protected override void OnStart(string[] args)
    {
        if (!serviceStat)
        {
            serviceStat = true;

            _timer = new System.Timers.Timer(5000); // 5 saniyede bir çalışır
            _timer.Elapsed += OnElapsedTime;
            _timer.AutoReset = true;
            _timer.Start();
        }
    }

    protected override void OnStop()
    {
        if (_timer != null)
        {
            _timer.Stop();
            _timer.Dispose();
        }
        serviceStat = false;
    }

    private async void OnElapsedTime(object sender, ElapsedEventArgs e)
    {
        Console.WriteLine("\n\n");

        foreach (var disk in DriveInfo.GetDrives())
        {
            var file = Path.Combine(disk.Name, "killer.ksk");

            /// Informer.PrintInfo($"Searching drive ({disk.Name})");

            if (File.Exists(file))
            {
                /// Informer.PrintSuccess($"Found the file ({file})");

                await Task.Run(() => DoWork(operations.services));
                await Task.Run(() => DoWork(operations.tasks));
                break;
            }
            else
            {
                /// Informer.PrintError($"Couldn't find the file ({file})");
            }

            Console.WriteLine("\n");
        }
    }

    private void DoWork(operations input)
    {
        if (input == operations.services)
            Operations.StopServices();

        if (input == operations.tasks)
            Operations.KillProcesses();
    }

    // Servis başlatıldığında, servis olarak çalıştırmak için Main metodunu yazmalısınız.
    public static void Main()
    {
        ServiceBase.Run(new Program());
    }
}
