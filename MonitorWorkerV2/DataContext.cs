using System.IO;
using Newtonsoft.Json;
namespace MonitorWorkerV2
{
    /// <summary>
    /// represent data access and data storage logic
    /// </summary>
    public class DataContext : Singleton<DataContext>
    {
        private DataModel data;

        public void Init()
        {
            //subscribe to any data properties changes
            Data.PropertyChanged += (sender, e) => { SaveData(); };
            
            //get data model properties
            var dataProperties = Data.GetType().GetProperties();

            //call property changed event for all data properties (for update UI)
            foreach (var prop in dataProperties)
            {                
                Data.OnPropertyChanged(prop.Name);
            } 
        }

        public DataModel Data
        {
            get
            {
                //check if data initialized
                if (data == null)
                {
                    //load data from file
                    data = LoadData();
                }
                return data;
            }
            set
            {
                data = value;

                //save data to file
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
        private string DataJson
        {
            get
            {               
                //serialize data, return json string
                return  JsonConvert.SerializeObject(Data);
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
            //write json to file
            File.WriteAllText(ConfigPath, DataJson);
        }
    }
}
