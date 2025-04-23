using System.Timers;

public static class Service
{
    private static bool serviceStat = false;
    private static System.Timers.Timer _timer;
    private enum operations { tasks, services }


    public static void StartService()
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

    private static async void OnElapsedTime(object sender, ElapsedEventArgs e)
    {
        Console.WriteLine("\n\n");

        if (File.Exists(@"C:\kill.txt"))
        {
            Informer.PrintSuccess("Found the file (C:/kill.txt)");

            await Task.Run(() => DoWork(operations.services));
            await Task.Run(() => DoWork(operations.tasks));
        }
        else
        { Informer.PrintError("Couldn't find the file (C:/kill.txt)"); }
    }

    private static void DoWork(operations input)
    {
        if (input == operations.services)
            Operations.StopServices();

        if (input == operations.tasks)
            Operations.KillProcesses();
    }

}