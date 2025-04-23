using System.Diagnostics;
using System.ServiceProcess;
using System.Text.RegularExpressions;

namespace StopServicesAndProcesses
{
    class Program
    {
        static void Main(string[] args)
        {
            // Servis isimleri ve exe dosyaları için regex
            string servicePattern = @"^[a-f0-9]{16}$";  // Servis ismi için hex formatı
            string processPattern = @"^[a-f0-9]{16}\.exe$";  // Exe dosyası için hex formatı

            // Servisleri kontrol et ve durdur
            StopServices(servicePattern);

            // Çalışan prosesleri kontrol et ve sonlandır
            KillProcesses(processPattern);
        }

        // Hex formatındaki servisleri durdur
        static void StopServices(string servicePattern)
        {
            try
            {
                var services = ServiceController.GetServices();
                var regex = new Regex(servicePattern, RegexOptions.IgnoreCase);

                foreach (var service in services)
                {
                    if (regex.IsMatch(service.ServiceName))
                    {
                        Console.WriteLine($"Stopping service: {service.ServiceName}");
                        try
                        {
                            if (service.Status != ServiceControllerStatus.Stopped)
                            {
                                service.Stop();
                                service.WaitForStatus(ServiceControllerStatus.Stopped);
                                Console.WriteLine($"Service {service.ServiceName} stopped successfully.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error stopping service {service.ServiceName}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching services: {ex.Message}");
            }
        }

        // Hex formatındaki exe dosyalarını sonlandır
        static void KillProcesses(string processPattern)
        {
            try
            {
                var processes = Process.GetProcesses();
                var regex = new Regex(processPattern, RegexOptions.IgnoreCase);

                foreach (var process in processes)
                {
                    string processName = process.ProcessName + ".exe";  // ProcessName does not include .exe

                    if (regex.IsMatch(processName))
                    {
                        Console.WriteLine($"Killing process: {process.ProcessName} (PID: {process.Id})");
                        try
                        {
                            process.Kill();
                            Console.WriteLine($"Process {process.ProcessName} killed successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error killing process {process.ProcessName}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching processes: {ex.Message}");
            }
        }
    
    }
}
