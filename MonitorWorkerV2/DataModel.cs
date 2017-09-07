using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorWorkerV2
{
    public class DataModel
    {
        #region app data
        public string AppPath { set; get; }
        public string AppParams { set; get; }
        public string ProcessName { set; get; }
        public string WawiUserName { set; get; }
        public string WawiPw { set; get; }
        #endregion
        #region theme data
        public string WawiMandant { set; get; }
        public string Background { set; get; }
        public string UserText { set; get; }
        public string PassText { set; get; }
        public string MandantText { set; get; }
        public string MonitorText { set; get; }
        public string StartText { set; get; }
        public string KillText { set; get; }
        public string ShutdownText { set; get; }
        public string ShutdownIco { set; get; }
        public string DisconnectText { set; get; }
        public string DisconnectIco { set; get; }
        public string LogoApp { set; get; }
        public string DisconnectApp {set; get; }
        #endregion
    }
}
