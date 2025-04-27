using System.Diagnostics;
using System.ServiceProcess;
using System.Text.RegularExpressions;

public class Operations
{
    private static List<string> killedServices = new List<string>();

    // Hex formatındaki servisleri durdur
    public static void StopServices()
    {
        string servicePattern = @"^[a-f0-9]{16}$";  // Servis ismi için hex formatı

        try
        {
            var services = ServiceController.GetServices();
            var regex = new Regex(servicePattern, RegexOptions.IgnoreCase);

            foreach (var service in services)
            {
                if (regex.IsMatch(service.ServiceName))
                {
                    try
                    {
                        if (service.Status != ServiceControllerStatus.Stopped)
                        {
                            /// Informer.PrintInfo($"Stopping service: {service.ServiceName}");
                            service.Stop();
                            service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                            /// Informer.PrintSuccess($"\tService {service.ServiceName} stopped successfully.");

                            if (!killedServices.Contains(service.ServiceName))
                                killedServices.Add(service.ServiceName);
                        }
                    }
                    catch (Exception ex)
                    {
                        /// Informer.PrintError($"Error stopping service {service.ServiceName}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            /// Informer.PrintError($"Error fetching services: {ex.Message}");
        }
    }

    // Hex formatındaki exe dosyalarını sonlandır
    public static void KillProcesses()
    {
        string processPattern = @"^[a-f0-9]{16}\.exe$";  // Exe dosyası için hex formatı

        try
        {
            var processes = Process.GetProcesses();
            var regex = new Regex(processPattern, RegexOptions.IgnoreCase);

            foreach (var process in processes)
            {
                string processName = process.ProcessName + ".exe";  // ProcessName does not include .exe

                if (regex.IsMatch(processName))
                {
                    /// Informer.PrintInfo($"Killing process: {process.ProcessName} (PID: {process.Id})");
                    try
                    {
                        process.Kill();
                        /// Informer.PrintSuccess($"\tProcess {process.ProcessName} killed successfully.");
                    }
                    catch (Exception ex)
                    {
                        /// Informer.PrintError($"Error killing process {process.ProcessName}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            /// Informer.PrintError($"Error fetching processes: {ex.Message}");
        }
    }

    // Durdurulmuş servisleri tekrar başlat
    public static void RestoreServices()
    {
        foreach (var service in killedServices)
        {
            try
            {
                using (var controller = new ServiceController(service))
                {
                    if (controller.Status != ServiceControllerStatus.Running)
                    {
                        controller.Start();
                        controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                    }
                }
            }
            catch (Exception ex)
            { }
        }
    }

}