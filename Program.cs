Service.StartService();
Informer.PrintInfo("Service is running. 'Ctrl-C' to stop the service & exit..\n\n");

bool shouldExit = false;
while (!shouldExit) { Console.ReadKey(intercept: true); }

Console.CancelKeyPress += (sender, e) =>
{
    Informer.PrintInfo("Ctrl+C algılandı. Servis durduruluyor...");

    shouldExit = true;
    e.Cancel = false; // Kapanma işlemini biz yönetiyoruz
};