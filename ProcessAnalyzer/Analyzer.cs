using Humanizer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessAnalyzer
{
    public class Analyzer
    {
        private Process RequestedProc { get; set; }
        private int ProcessId { get; set; }
        private IntPtr ProcessHandle { get; set; }
        private IntPtr ProcessBase { get; set; }

        public async Task BeginLogic()
        {
            ReadProcess();
            await Task.Delay(-1);
        }

        private void ReadProcess()
        {
            ConsoleWriter.ConsoleWrite(new LogMsg(Statuses.Waiting, $"Please type the name of the process you want to analyze."));
            Process[] procs = null;
            try
            {
                ConsoleWriter.WriteInput();
                var name = Console.ReadLine();
                procs = Process.GetProcessesByName(name);
                if (procs.Length is 0)
                {
                    ConsoleWriter.ConsoleWrite(new LogMsg(Statuses.Failed, $"No proccesses found. Exiting."));
                    Thread.Sleep(3000);
                    Environment.Exit(-1);
                }
                if (procs.Length == 1)
                {
                    RequestedProc = procs[0];
                    ProcessId = RequestedProc.Id;
                    ProcessHandle = RequestedProc.Handle;
                    ProcessBase = RequestedProc.MainModule.BaseAddress;
                    ConsoleWriter.ConsoleWrite(new LogMsg(Statuses.Success, $"Found matching process! \"{RequestedProc.ProcessName}\" || id: {ProcessId} || base address: {ProcessBase} || handle: {ProcessHandle}"));

                }
                else
                {
                    ConsoleWriter.ConsoleWrite(new LogMsg(Statuses.Success, "Found matching processes!"));
                    var dict = new Dictionary<int, Process>();
                    for (int i = 0; i < procs.Length; i++)
                    {
                        dict.Add(i, procs[i]);
                        Console.WriteLine($"\t{i}. {procs[i].ProcessName} id: {procs[i].Id} base address: {procs[i].MainModule.BaseAddress}");
                    }
                    ConsoleWriter.ConsoleWrite(new LogMsg(Statuses.Success, $"Processes listed. Please type the number of the process you want."));
                    ConsoleWriter.WriteInput();
                    name = Console.ReadLine();
                    int.TryParse(name, out var identifier);
                    RequestedProc = dict[identifier];
                    ProcessId = RequestedProc.Id;
                    ProcessHandle = RequestedProc.Handle;
                    ProcessBase = RequestedProc.MainModule.BaseAddress;
                    ConsoleWriter.ConsoleWrite(new LogMsg(Statuses.Success, $"Processes grabbed. \"{RequestedProc.ProcessName}\" || id: {ProcessId} || base address: {ProcessBase} || handle: {ProcessHandle}"));
                }
                var options = new Dictionary<string, string>
                {
                    {"Base priority", RequestedProc.BasePriority.ToString() ?? "null"},
                    {"Handle count", RequestedProc.HandleCount.ToString() ?? "null" },
                    {"Total processor time", RequestedProc.TotalProcessorTime.Humanize() ?? "null" }
                };
                ConsoleWriter.ConsoleWrite(new LogMsg(Statuses.Waiting, "What would you like to know?"));
                Console.WriteLine("\tOptions:");
                for (int i = 0; i < options.Count(); i++)
                {
                    Console.WriteLine($"\t{i}. {options.ElementAt(i).Key}");
                }
                do
                {
                    ConsoleWriter.WriteInput();
                    name = Console.ReadLine();
                    int.TryParse(name, out var result);
                    ConsoleWriter.ConsoleWrite(new LogMsg(Statuses.Success, $"Proccess {options.ElementAt(result).Key} found: {options.ElementAt(result).Value}"));
                } while (true); //wip
            }
            catch (Exception e)
            {
                ConsoleWriter.ConsoleWrite(new LogMsg(Statuses.Exception, e.Message));
                return;
            }
        }

        public void DeepAnalysis()
        {
            //wip
        }
    }
}
