using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Threading;

namespace MonitorWorkerV2
{
    class Core
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Core));

        /// <summary>
        /// Flags from app.config to configure execution of application 
        /// </summary>
        [Flags]
        public enum AliveFlags
        {
            None = 0,
            Cpu = 0x1,
            WorkingSet = 0x2,
            PrivateMemory = 0x4,
            All = 0xfffffff
        }

        /// <summary>
        /// Synch object for console write
        /// </summary>
        static readonly object SyncRoot = new object();

        /// <summary>
        /// Flag from app.config whether to kill frozen app or not
        /// </summary>
        private static readonly bool KillFrozen;

        /// <summary>
        /// Flags from app.config to configure application run
        /// </summary>
        private static readonly AliveFlags Flags;

        /// <summary>
        /// Interval when will be collected info abot process
        /// </summary>
        private static readonly int SnapshotInterval;

        /// <summary>
        /// Amount of time to monitor processes
        /// </summary>
        private static readonly int SnapshotPeriod;

        static void WriteLine(string format, params object[] arg)
        {           
            log.InfoFormat(format, arg);
        }

        /// <summary>
        /// Constructor called automatically
        /// </summary>
        static Core()
        {
            WriteLine("Setup configuration...");
            // get and init all values from app.config file
            if (!int.TryParse(ConfigurationManager.AppSettings["SnapshotInterval"], out SnapshotInterval) || SnapshotInterval <= 0)
            {
                SnapshotInterval = 10;
            }
            WriteLine("SnapshotInterval: {0} miliseconds", SnapshotInterval);

            if (!int.TryParse(ConfigurationManager.AppSettings["SnapshotPeriod"], out SnapshotPeriod) || SnapshotPeriod <= 0)
            {
                SnapshotPeriod = 20;
            }
            WriteLine("SnapshotPeriod: {0} seconds", SnapshotPeriod);

            bool isAliveCpu;
            if (bool.TryParse(ConfigurationManager.AppSettings["IsAlive.Cpu"], out isAliveCpu) && isAliveCpu)
            {
                Flags |= AliveFlags.Cpu;
            }
            bool isAliveWorkingSet;
            if (bool.TryParse(ConfigurationManager.AppSettings["IsAlive.WorkingSet"], out isAliveWorkingSet) && isAliveWorkingSet)
            {
                Flags |= AliveFlags.WorkingSet;
            }
            bool isAlivePrivateMemory;
            if (bool.TryParse(ConfigurationManager.AppSettings["IsAlive.PrivateMemory"], out isAlivePrivateMemory) && isAlivePrivateMemory)
            {
                Flags |= AliveFlags.PrivateMemory;
            }
            WriteLine("Flags: {0}", Flags);

            bool.TryParse(ConfigurationManager.AppSettings["IsAlive.Cpu"], out KillFrozen);
            WriteLine("KillFrozen: {0}", KillFrozen);
        }

        private static string ProcessName;
        private static string ParentProcessName;
        private static List<int> FrozenParentProcessIds = new List<int>();

        /// <summary>
        /// 1) check if something is forzen
        /// 2) if one or more subprocesses are forzen, check if all of them are from the same mainprocess
        /// 3) kill the oldest subprocess of them
        /// </summary>
        /// <param name="args"></param>
        static void Start(string[] args)
        {
            if (args.Length > 0)
            {
                ProcessName = args[0];
            }

            // check if name of subprocess to monitor is not passed
            if (String.IsNullOrEmpty(ProcessName))
            {
                WriteLine("ERROR: Please pass application name as argument to run this application.\r\n");
                return;
            }
            WriteLine("Process Name: {0}", ProcessName);

            // init parent process name with same of subprocess
            ParentProcessName = ProcessName;

            // optionaly we can pass parent process name as second argument
            if (args.Length > 1)
            {
                ParentProcessName = args[1];
            }
            WriteLine("Parent Process Name: {0}", ParentProcessName);

            // all is done now we can start application
            WriteLine("\r\nStart Application\r\n");

            var tasks = new List<Task>();
            // get all processes curently running in the system
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                // check if process has same name or not continue search
                if (!String.Equals(process.ProcessName, ProcessName, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                // check if parent process was need to check also
                if (!String.IsNullOrEmpty(ParentProcessName))
                {
                    // get parent process
                    var parent = process.GetParent();

                    // check if we have parent process and names are same as expected or continue search
                    if (parent == null || !String.Equals(parent.ProcessName, ParentProcessName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                }

                // create new task to monitor process
                var task = new Task(() => CheckProcess(process));

                // start task
                task.Start();

                // add task to list of all tasks
                tasks.Add(task);
            }

            // check if there was not found any process to monitor
            if (tasks.Count == 0)
            {
                WriteLine("No process found to monitor.\r\n");
            }
            else
            {
                // wait for all process monitoring ends
                Task.WaitAll(tasks.ToArray());
            }

            WriteLine("Finish Application. Exiting...\r\n");
            // exit
        }

        static void CheckProcess(Process process)
        {
            // write to console that we started monitoring
            lock (SyncRoot)
            {
                WriteLine("Starting Monitoring. Process Id: {0}, ProcessName: {1}", process.Id, process.ProcessName);
            }

            try
            {
                // make lists for monitoring process values for each snapshot
                var cpuCounters = new List<double>();
                var workingSetCounters = new List<long>();
                var privateMemoryCounters = new List<long>();

                var startMonitoringTime = DateTime.UtcNow;
                var startProcessorTime = process.TotalProcessorTime;
                var startPrivateMemorySize = process.PrivateMemorySize64;
                var startWorkingSet = process.WorkingSet64;

                while (true)
                {
                    // get latest process information
                    var updatedProcess = Process.GetProcessById(process.Id);

                    // calculate how much processor time was used since previous snapshot
                    var cpuDifference = (updatedProcess.TotalProcessorTime - startProcessorTime).TotalMilliseconds;
                    cpuCounters.Add(cpuDifference);

                    // calculate private memory usage diference since previous snapshot
                    var memDifference = updatedProcess.PrivateMemorySize64 - startPrivateMemorySize;
                    privateMemoryCounters.Add(memDifference);

                    // calculate working set usage diference since previous snapshot
                    var workingSetDifference = updatedProcess.WorkingSet64 - startWorkingSet;
                    workingSetCounters.Add(workingSetDifference);

                    // check if we need to stop monitoring or continue
                    var workingTime = DateTime.UtcNow - startMonitoringTime;
                    if (workingTime.TotalSeconds >= SnapshotPeriod)
                    {
                        break;
                    }

                    // sleep for interval
                    Thread.Sleep(SnapshotInterval);
                }

                lock (SyncRoot)
                {
                    WriteLine("Finished Monitoring. Process Id: {0}, ProcessName: {1}", process.Id, process.ProcessName);
                    WriteLine(process.GetProcessInfoString());
                    WriteLine("Average ProcessorTime ms: {0}", cpuCounters.Average());
                    WriteLine("Average WorkingSet: {0}", workingSetCounters.Average());
                    WriteLine("Average PrivateMemorySize: {0}", privateMemoryCounters.Average());
                    WriteLine("StartTime: {0}", process.StartTime);

                    // alive flag indicating if process is frozen or not
                    var isAlive = false;
                    if (Flags == AliveFlags.None)
                    {
                        isAlive = process.Responding;
                    }
                    else
                    {
                        // check if we need monitor cpu
                        if (Flags.HasFlag(AliveFlags.Cpu))
                        {
                            isAlive = cpuCounters.Any(c => c > 0);
                        }
                        // check if we need monitor private memory
                        if (!isAlive && Flags.HasFlag(AliveFlags.PrivateMemory))
                        {
                            isAlive = privateMemoryCounters.Any(c => c != 0);
                        }
                        // check if we need monitor working set
                        if (!isAlive && Flags.HasFlag(AliveFlags.WorkingSet))
                        {
                            isAlive = workingSetCounters.Any(c => c != 0);
                        }
                    }
                    WriteLine("Is Alive: {0}\r\n", isAlive);

                    // check if something is forzen and need kill it
                    if (!isAlive && KillFrozen)
                    {
                        // get parent process of current
                        var parent = process.GetParent();

                        // check if parent process was allready processed
                        if (!FrozenParentProcessIds.Contains(parent.Id))
                        {
                            // add this process to ignore list, because we already founded frozen app
                            FrozenParentProcessIds.Add(parent.Id);

                            // if one or more subprocesses are forzen
                            // get all of them from the same mainprocess
                            // get the oldest subprocess of them
                            var frozen = parent
                                .GetChilds()
                                .Where(p => String.Equals(p.ProcessName, process.ProcessName, StringComparison.InvariantCultureIgnoreCase))
                                .OrderBy(p => p.StartTime)
                                .First();

                            // check if we need to kill frozen app or not
                            WriteLine("Terminating the oldest subprocess: {0}\r\n", frozen.Id);
                            frozen.Kill();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // write that we ended monitoring with exception
                lock (SyncRoot)
                {
                    WriteLine("Finished Monitoring: Process Id - {0}, ProcessName: {1}\r\nException: {2}\r\n",
                        process.Id, process.ProcessName, ex.Message);
                }
            }
        }

    }
}
