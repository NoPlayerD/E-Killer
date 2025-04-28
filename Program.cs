using System.ServiceProcess;
using System.Timers;

public class Program : ServiceBase
{
    private bool serviceStat = false;
    private bool activationStat = false;
    private System.Timers.Timer _timer;
    private enum operations { killTasks, killServices, restoreServices }

    public Program()
    {
        this.ServiceName = "E-Killer";  // Servis ismi
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
        /// Console.WriteLine("\n\n");
        activationStat = false;

        foreach (var drive in DriveInfo.GetDrives())
        {
            var file = Path.Combine(drive.Name, "killer.ksk");

            /// Informer.PrintInfo($"Searching drive ({drive.Name})");

            if (File.Exists(file))
            {
                /// Informer.PrintSuccess($"Found the file ({file})");
                /// Console.WriteLine("\n");

                activationStat = true;
            }
            else
            {
                /// Informer.PrintError($"Couldn't find the file ({file})");
                /// Console.WriteLine("\n");
            }
        }

        if (activationStat)
        {
            await Task.Run(() => DoWork(operations.killServices));
            await Task.Run(() => DoWork(operations.killTasks));
        }
        else
        {
            await Task.Run(() => DoWork(operations.restoreServices));
        }


    }

    private void DoWork(operations input)
    {
        if (input == operations.killServices)
            Operations.StopServices();

        if (input == operations.killTasks)
            Operations.KillProcesses();

        if (input == operations.restoreServices)
            Operations.RestoreServices();
    }

    public static void Main()
    {
        ServiceBase.Run(new Program());
    }
}
