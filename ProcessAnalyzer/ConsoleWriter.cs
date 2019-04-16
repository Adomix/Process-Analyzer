using System;

namespace ProcessAnalyzer
{
    public class ConsoleWriter
    {
        public static void ConsoleWrite(LogMsg msg)
        {
            switch (msg.Status)
            {
                case Statuses.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case Statuses.Failed:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Statuses.Exception:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case Statuses.Waiting:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            Console.Write($"[{msg.Status,-9}] ");
            Console.ResetColor();
            Console.WriteLine($"{msg.Message}");
        }

        public static void WriteInput()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("> ");
            Console.ResetColor();
        }
    }

    public enum Statuses
    {
        Success = 0,
        Failed = 1,
        Exception = 2,
        Waiting = 3
    }

    public struct LogMsg
    {
        public Statuses Status { get; set; }
        public string Message { get; set; }

        public LogMsg(Statuses stat, string msg)
        {
            Status = stat;
            Message = msg;
        }
    }
}
