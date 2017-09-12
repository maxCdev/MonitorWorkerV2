namespace MonitorWorkerV2
{
    /// <summary>
    /// represents app logic
    /// </summary>
    public class AppController : Singleton<AppController>
    {
        public void Start()
        {
            //Init app data and update UI
            DataContext.Instance.Init();

            //AppStartupManager.SetStartup(true);
        }
        
    }
}
