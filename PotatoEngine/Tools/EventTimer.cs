using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace PotatoEngine
{
    public class EventTimer
    {
        public Timer Timer { get; private set; }
        public bool Enabled
        {
            get
            {
                return Timer.Enabled;
            }
        }

        public EventTimer(double interval)
        {
            Timer = new Timer(interval);
            Timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Timer.Enabled = false;
        }

        public void Start()
        {
            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
        }

        public void Destroy()
        {
            Timer.Elapsed -= Timer_Elapsed;
        }

        
    }
}
