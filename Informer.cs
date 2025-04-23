public static class Informer
{
    public static void PrintInfo(string msg) => Print(msg, ConsoleColor.Yellow);
    public static void PrintError(string msg) => Print(msg, ConsoleColor.Red);
    public static void PrintSuccess(string msg) => Print(msg, ConsoleColor.Green);

    private static void Print(string msg, ConsoleColor color)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        Console.ForegroundColor = color;
        Console.WriteLine($"{timestamp} | {msg}");
        Console.ResetColor();
    }
}
