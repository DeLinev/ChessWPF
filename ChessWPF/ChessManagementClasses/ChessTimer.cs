using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ChessManagementClasses
{
    public class ChessTimer
    {
        protected DispatcherTimer timer;
        protected int initialMinutes;
        protected int initialSeconds;
        protected int minutes;
        protected int seconds;

        public int Minutes
        {
            get => minutes;
            protected set
            {
                if (value < 0 || value > 60)
                    throw new ArgumentOutOfRangeException("Minutes must be between 0 and 60");

                minutes = value;
            }
        }
        public int Seconds
        {
            get => seconds;
            protected set
            {
                if (value < 0 || value > 60)
                    throw new ArgumentOutOfRangeException("Seconds must be between 0 and 60");

                seconds = value;
            }
        }
        public string Time { get => string.Format("{0:00}:{1:00}", Minutes, Seconds); }

        public ChessTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        public ChessTimer(int minutes, int seconds)
        {
            timer = new DispatcherTimer();
            Minutes = minutes;
            Seconds = seconds;
            initialMinutes = minutes;
            initialSeconds = seconds;
        }

        public void SetTime(int minutes, int seconds)
        {
            Minutes = minutes;
            Seconds = seconds;
            initialMinutes = minutes;
            initialSeconds = seconds;
        }

        public void HandleTimer()
        {
            if (Seconds == 0)
            {
                if (Minutes == 0)
                {
                    timer.Stop();
                    return;
                }
                Minutes--;
                Seconds = 59;
            }
            else
                Seconds--;
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void SetInterval(TimeSpan interval)
        {
            timer.Interval = interval;
        }

        public void SetTick(EventHandler handler)
        {
            timer.Tick += handler;
        }

        public void Reset()
        {
            SetTime(initialMinutes, initialSeconds);
            timer.Stop();
        }
    }
}
