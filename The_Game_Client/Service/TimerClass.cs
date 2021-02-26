using System;
using System.Timers;

namespace The_Game_Client.Utility
{
    public class TimerClass
    {
        private Timer _timer;
        public string inGoing { get; set; }
        
        public TimerClass(int millis)
        {
            _timer = new Timer(millis);
        }
        public void SetTimer()
        {
            _timer.Elapsed += Unblock;
            _timer.Enabled = true;
        }
        public void StopTimer()
        {
            if (_timer.Enabled)
                _timer.Enabled = false;
        }

        public void StartTimer()
        {
            _timer.Start();
        }

        public void StopTimerTime()
        {
            _timer.Stop();
        }

        private void Unblock(Object source, ElapsedEventArgs e)
        {
            inGoing = "Exit";
        }

        
        
    }
}