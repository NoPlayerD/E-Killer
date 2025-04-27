using System.Diagnostics;
using System.ServiceProcess;
using System.Text.RegularExpressions;

public class Operations
{
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
                            service.WaitForStatus(ServiceControllerStatus.Stopped);
                            /// Informer.PrintSuccess($"\tService {service.ServiceName} stopped successfully.");
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

}