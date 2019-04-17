using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessAnalyzer
{
    public class Analyzer
    {

        private readonly string _prefix = Process.GetCurrentProcess().ProcessName;
        public async Task BeginLogic()
        {
            ReadProcess();
            await Task.Delay(-1);
        }

        private void ReadProcess()
        {
            ConsoleWriter.ConsoleWrite(_prefix, new LogMsg(Statuses.Waiting, $"Please type the name of the process you want to analyze."));
            Process[] procs = null;
            Process proc = null;
            try
            {
                var name = ConsoleWriter.WriteInput(_prefix);
                procs = Process.GetProcessesByName(name);
                if (procs.Length is 0)
                {
                    ConsoleWriter.ConsoleWrite(_prefix, new LogMsg(Statuses.Failed, $"No proccesses found. Exiting."));
                    Thread.Sleep(3000);
                    Environment.Exit(-1);
                }
                if (procs.Length == 1)
                {
                    proc = procs[0];
                    ConsoleWriter.ConsoleWrite(_prefix, new LogMsg(Statuses.Success, $"Found matching process!"));
                    ConsoleWriter.ConsoleWrite(_prefix, new LogMsg(Statuses.Info, $"\"{proc.ProcessName}\" || id: {proc.Id}"));
                }
                else
                {
                    ConsoleWriter.ConsoleWrite(_prefix, new LogMsg(Statuses.Success, "Found matching processes!"));
                    var dict = new Dictionary<int, Process>();
                    for (int i = 0; i < procs.Length; i++)
                    {
                        dict.Add(i, procs[i]);
                        Console.WriteLine($"\t{i}. {procs[i].ProcessName} id: {procs[i].Id} base address: {procs[i].MainModule.BaseAddress}");
                    }
                    ConsoleWriter.ConsoleWrite(_prefix, new LogMsg(Statuses.Waiting, $"Processes listed. Please type the number of the process you want."));
                    name = ConsoleWriter.WriteInput(_prefix);
                    int.TryParse(name, out var identifier);
                    proc = dict[identifier];
                    ConsoleWriter.ConsoleWrite(_prefix, new LogMsg(Statuses.Success, $"Processes grabbed."));
                    ConsoleWriter.ConsoleWrite(_prefix, new LogMsg(Statuses.Info, $"\"{proc.ProcessName}\" || id: {proc.Id}"));

                }
                if (proc != null) DeepAnalysis(proc);
            }
            catch (Exception e)
            {
                ConsoleWriter.ConsoleWrite(_prefix, new LogMsg(Statuses.Exception, e.Message));
                return;
            }
        }

        public void DeepAnalysis(Process proc)
        {
            ConsoleWriter.ConsoleWrite(_prefix, new LogMsg(Statuses.Info, $"Beginning deep analysis.\n\r"));
            foreach (var prop in proc.GetType().GetProperties())
            {
                try
                {
                    Console.Write($"[{_prefix}] ");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"{prop.Name} ");
                    Console.ResetColor();
                    Console.WriteLine(typeof(Process).GetProperty(prop.Name).GetValue(proc) ?? "null");
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Error: ");
                    Console.ResetColor();
                    Console.WriteLine(e.InnerException.Message);
                }
            }
            Console.Write("\n\r");
            ConsoleWriter.ConsoleWrite(_prefix, new LogMsg(Statuses.Success, $"Analysis finished."));
        }
    }
}
