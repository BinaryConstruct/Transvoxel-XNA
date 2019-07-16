using System;
using System.Threading;
using TransvoxelXnaStudio.Framework;

namespace TransvoxelXnaStudio
{
    public class Logger
    {
        private static Logger _instance;
        public static Logger GetLogger()
        {
            if (_instance == null)
            {
                var singleton = new Logger();
                if (Interlocked.CompareExchange(ref _instance, singleton, null) != null)
                {
                    //if (singleton is IDisposable) singleton.Dispose();
                }
            }
            return _instance;
        }
        private Logger()
        {
            
        }

        public event EventHandler<EventArgs<string>> Logged;


        protected virtual void OnLogged(object sender, EventArgs<string> e)
        {
            if (Logged != null) Logged(sender, e);
        }
        public void Log(object sender, string message)
        {
            OnLogged(sender, new EventArgs<string>(message));
        }
    }
}