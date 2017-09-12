using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace MonitorWorkerV2
{
    public static class ProcessExtension
    {
        /// <summary>
        /// Console printing output format about process
        /// </summary>
        const string ProcessInfoFormat =
            "Id: {0}\r\nProcessName:{1}\r\nMainWindowTitle:{2}\r\nMachineName:{3}\r\nMainWindowHandle:{4}\r\nSessionId:{5}\r\nFileName:{6}\r\nArguments:{7}\r\nDomain:{8}\r\nUserName:{9}\r\nWorkingDirectory:{10}";

        /// <summary>
        /// Helper to get list of all child processes
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IList<Process> GetChilds(this Process self)
        {
            var children = new List<Process>();
            var search = new ManagementObjectSearcher(
                string.Format("Select * From Win32_Process Where ParentProcessID={0}", self.Id));

            foreach (var item in search.Get())
            {
                var managementObject = (ManagementObject)item;
                var processId = Convert.ToInt32(managementObject["ProcessID"]);
                try
                {
                    children.Add(Process.GetProcessById(processId));
                }
                catch
                {
                    // ignore
                }
            }
            return children;
        }

        /// <summary>
        /// Helper to get parent process
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Process GetParent(this Process self)
        {
            var query = string.Format("SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {0}", self.Id);
            var search = new ManagementObjectSearcher("root\\CIMV2", query);
            foreach (var item in search.Get())
            {
                var managementObject = (ManagementObject)item;
                var processId = Convert.ToInt32(managementObject["ParentProcessId"]);
                try
                {
                    return Process.GetProcessById(processId);
                }
                catch
                {
                    // ignore
                }
            }
            return null;
        }

        public static string GetProcessInfoString(this Process process)
        {
            // get process file location
            var startInfo = process.StartInfo;
            var fileName = startInfo.FileName;
            if (String.IsNullOrEmpty(fileName))
            {
                try
                {
                    fileName = process.MainModule.FileName;
                }
                catch { }
            }

            // get process working folder
            var workingDirectory = startInfo.WorkingDirectory;
            if (String.IsNullOrEmpty(workingDirectory) && !string.IsNullOrEmpty(fileName))
            {
                workingDirectory = Path.GetDirectoryName(fileName);
            }

            // return formated string with process information
            return string.Format(ProcessInfoFormat,
                process.Id,
                process.ProcessName,
                process.MainWindowTitle,
                process.MachineName,
                process.MainWindowHandle,
                process.SessionId,
                fileName,
                startInfo.Arguments,
                startInfo.Domain,
                startInfo.UserName,
                workingDirectory
                );
        }
    }
}
