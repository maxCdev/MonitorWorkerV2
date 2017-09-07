using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace MonitorWorkerV2
{
    /// <summary>
    /// represent data access and data storage logic
    /// </summary>
    public class DataContext : Singleton<DataContext>
    {
        private DataModel data;
        public DataModel Data
        {
            get
            {
                if (data == null)
                {
                    data = LoadData();
                }
                return data;
            }
            set
            {
                data = value;
                SaveData();
            }
        }
        private string configPath;
        private string ConfigPath
        {
            get
            {
                if (configPath==null)
                {
                    configPath = AppContext.Instance.WinUserConfigPath;
                }
                return configPath;
            }
        }
        /// <summary>
        /// load data from user config
        /// </summary>
        /// <returns></returns>
        private DataModel LoadData()
        {
            //check if config file exist
            if (!File.Exists(ConfigPath))
            {
                //create default model
                data = new DataModel();

                //save to json config
                SaveData();
            }
            else
            {
                //read json string
                var jsonStr = File.ReadAllText(ConfigPath);

                //set data
                data = JsonConvert.DeserializeObject<DataModel>(jsonStr);
            }
            return data;
        }
        private void SaveData()
        {
            //serialize data, get json string
            var jsonStr = JsonConvert.SerializeObject(data);

            //write json to file
            File.WriteAllText(ConfigPath, jsonStr);
        }
    }
}
