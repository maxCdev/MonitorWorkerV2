using System.Security.Principal;
using System.IO;
using System.Reflection;

namespace MonitorWorkerV2
{
    /// <summary>
    /// Represents app execute location helper properties
    /// </summary>
    public class AppContext : Singleton<AppContext>
    {
        private string executablePath;
        public string ExecutablePath
        {
            get
            {
                if (executablePath==null)
                {
                    executablePath = Assembly.GetExecutingAssembly().Location;
                }
                return executablePath;
            }
        }
        private string executableDirectoryPath;
        public string ExecutableDirectoryPath
        {
            get
            {
                if (executableDirectoryPath == null)
                {
                    var fileNameIndex = ExecutablePath.IndexOf(ExecutableFileName);

                    executableDirectoryPath = ExecutablePath.Remove(fileNameIndex, ExecutableFileName.Length);
                }
                return executableDirectoryPath;
            }
        }
        private string executableFileName;

        public string ExecutableFileName
        {
            get
            {
                if (executableFileName==null)
                {           
                    executableFileName = Path.GetFileName(ExecutablePath);
                }
                return executableFileName;
            }
        }

        private string winUserName;

        public string WinUserName
        {
            get
            {
                if (winUserName == null)
                {
                    winUserName = WindowsIdentity.GetCurrent().Name.Split('\\')[1];
                }
                return winUserName;
            }
           
        }
        private string winUserConfigFileName;

        public string WinUserConfigFileName
        {
            get
            {
                if (winUserConfigFileName == null)
                {
                    winUserConfigFileName = string.Format("{0}.json", WinUserName);
                }
                return winUserConfigFileName;
            }
          
        }

        private string winUserConfigPath;
        public string WinUserConfigPath
        {
            get
            {
                if (winUserConfigPath == null)
                {
                    winUserConfigPath = Path.Combine(ExecutableDirectoryPath, WinUserConfigFileName); 
                }
                return winUserConfigPath;
            }
          
        }


    }
}
