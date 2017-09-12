using Microsoft.Win32;
using System;
using System.Windows;

namespace MonitorWorkerV2
{
    public class AppStartupManager
    {
        /// <summary>
        /// Set key to windows registry for start app when os start
        /// </summary>
        /// <param name="runOnStart"></param>
        public static void SetStartup(bool runOnStart)
        {
            try
            {
                //open subkey in registry dict.
                var rk = Registry.CurrentUser.OpenSubKey
                  ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                var appContext = AppContext.Instance;

                //get app ex. file name
                var appName = AppContext.Instance.ExecutableFileName;

                //get app ex path
                var path = appContext.ExecutablePath;

                //check if flag true
                if (runOnStart)
                {
                    //subscribe on startup 
                    rk.SetValue(appName, path);
                }
                else
                {
                    //unsubscribe on startup 
                    rk.DeleteValue(appName, false);
                }
            }
            catch (Exception ex)
            {
                //show exeption meg by msg box
                MessageBox.Show(ex.Message);
            }

        }
    }
}
