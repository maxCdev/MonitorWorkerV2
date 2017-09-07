namespace MonitorWorkerV2
{
    public class Singleton<T> where T : new()
    {
        private static T instance;

        private static object syncRoot = new object();

        public static T Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance==null)
                    {
                         instance = new T();              
                    }
                }
                return instance;             
            }
            
        }
    }
}
