using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorWorkerV2
{
    public class DataModel : IOneWayData, IToWayData, INotifyPropertyChanged
    {
       public DataModel()
        {
            //set default data
            UserText = "JTL-WAWI Username:";
            PassText = "Password";
        }
#region one way data
        public string AppParams
        {
            get; set;
        }

        public string AppPath
        {
            get; set;
        }

        public string Background
        {
            get; set; 
        }

        public string DisconnectApp
        {
            get; set;
        }

        public string DisconnectIco
        {
            get; set;
        }

        public string DisconnectText
        {
            get; set;
        }

        public string KillText
        {
            get; set;
        }

        public string LogoApp
        {
            get; set;
            
        }

        public string MandantText
        {
            get;

            set;
        }

        public string MonitorText
        {
            get; set;
        }

        public string PassText
        {
            get; set;           
        }

        public string ProcessName
        {
            get; set;
        }

        public string ShutdownIco
        {
            get; set;
        }

        public string ShutdownText
        {
            get; set;
        }

        public string StartText
        {
            get; set;
        }

        public string UserText
        {
            get;

            set;
        }

        public string WawiMandant
        {
            get;

            set;
        }
#endregion

#region two way data
        private string _wawiPw;
        public string WawiPw
        {
            get
            {
                return _wawiPw;
            }

            set
            {
                _wawiPw = value;
                OnPropertyChanged("WawiPw");
            }
        }
        private string _wawiUserName;
        public string WawiUserName
        {
            get
            {
               return _wawiUserName;
            }

            set
            {
                _wawiUserName = value;
                OnPropertyChanged("WawiUserName");
            }
        }
        private string _keepCredentials;
        public string KeepCredentials
        {
            get
            {
                return _keepCredentials;
            }

            set
            {
                _keepCredentials = value;
                OnPropertyChanged("KeepCredentials");
            }
        }
        private bool _restart24Hours;
        public bool Restart24Hours
        {
            get
            {
                return _restart24Hours;
            }

            set
            {
                _restart24Hours = value;
                OnPropertyChanged("Restart24Hours");
            }
        }
        private bool _notifyEmailErrors;
        public bool NotifyEmailErrors
        {
            get
            {
                return _notifyEmailErrors;
            }

            set
            {
                _notifyEmailErrors = value;
                OnPropertyChanged("NotifyEmailErrors");
            }
        }
        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// represent one way mode data (data that not update source)
    /// </summary>
    public interface IOneWayData
    {
        string AppPath { set; get; }
        string AppParams { set; get; }
        string ProcessName { set; get; }
        string WawiMandant { set; get; }
        string Background { set; get; }
        string UserText { set; get; }
        string PassText { set; get; }
        string MandantText { set; get; }
        string MonitorText { set; get; }
        string StartText { set; get; }
        string KillText { set; get; }
        string ShutdownText { set; get; }
        string ShutdownIco { set; get; }
        string DisconnectText { set; get; }
        string DisconnectIco { set; get; }
        string LogoApp { set; get; }
        string DisconnectApp { set; get; }

    }
    /// <summary>
    /// represent to way data mode (update UI and Source)
    /// </summary>
    public interface IToWayData
    {
        string WawiUserName { set; get; }
        string WawiPw { set; get; }
        string KeepCredentials { set; get; }
        bool Restart24Hours { set; get; }
        bool NotifyEmailErrors { set; get; }
        string Email { set; get; }
    }
}
