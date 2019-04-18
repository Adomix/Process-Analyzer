using System;

namespace ProcessAnalyzer
{
    public class ConsoleWriter
    {
        public static void ConsoleWrite(string prefix, LogMsg msg)
        {
            Console.Write($"[{prefix}] ");
            switch (msg.Status)
            {
                case Statuses.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case Statuses.Failed:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Statuses.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case Statuses.Waiting:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Statuses.Exception:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
            }

            Console.Write($"[{msg.Status,-9}] ");
            Console.ResetColor();
            Console.WriteLine($"{msg.Message}");
        }

        public static string WriteInput(string prefix)
        {
            Console.Write($"[{prefix}] ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(">> ");
            Console.ResetColor();
            return Console.ReadLine();
        }
    }

    public enum Statuses
    {
        Success = 0,
        Failed = 1,
        Info = 2,
        Waiting = 3,
        Exception = 4
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