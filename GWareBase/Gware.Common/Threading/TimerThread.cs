using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Threading
{
    [Flags]
    public enum ThreadTimerEvents
    {
        Cycle = 0x01,
        Second = 0x02,
        Minute = 0x04,
        Hour = 0x08,
    }

    public class TimerThread : ThreadBase
    {
        private delegate void TimerAction();
        private ThreadTimerEvents _actionEvents;
        private TimerAction _timerActions;

        private DateTime _lastMinutePass;
        private DateTime _lastSecondPass;
        private DateTime _lastHourPass;

        public DateTime LastHourPass
        {
            get { return _lastHourPass; }
            set { _lastHourPass = value; }
        }

        public DateTime LastSecondPass
        {
            get { return _lastSecondPass; }
            set { _lastSecondPass = value; }
        }

        public DateTime LastMinutePass
        {
            get { return _lastMinutePass; }
            set { _lastMinutePass = value; }
        }

        public ThreadTimerEvents ActionEvents
        {
            get
            {
                return _actionEvents;
            }
            set
            {
                _actionEvents = value;
                _timerActions = null;

                if ((_actionEvents & ThreadTimerEvents.Cycle) == ThreadTimerEvents.Cycle)
                {
                    _timerActions += SleepCyclePing;
                }
                if ((_actionEvents & ThreadTimerEvents.Second) == ThreadTimerEvents.Second)
                {
                    _timerActions += SecondPing;
                }
                if ((_actionEvents & ThreadTimerEvents.Minute) == ThreadTimerEvents.Minute)
                {
                    _timerActions += MinutePing;
                }
                if ((_actionEvents & ThreadTimerEvents.Hour) == ThreadTimerEvents.Hour)
                {
                    _timerActions += HourPing;
                }
            }
        }
        public TimerThread()
            : base()
        {
            ActionEvents = ThreadTimerEvents.Cycle | ThreadTimerEvents.Hour | ThreadTimerEvents.Minute | ThreadTimerEvents.Second;
        }
        public TimerThread(ThreadTimerEvents actionEvents)
        {
            ActionEvents = actionEvents;
        }
        protected override void OnThreadInit()
        {
            InitiliasePasses();
        }
        protected override void OnThreadExit()
        {
            base.OnThreadExit();
        }
        protected override void ExecuteSingleThreadCycle()
        {
            _timerActions();
        }

        private void InitiliasePasses()
        {
            DateTime utcNow = DateTime.UtcNow;
            _lastMinutePass = utcNow;
            _lastSecondPass = utcNow;
            _lastHourPass = utcNow;
        }
        private void MinutePing()
        {
            TimeSpan timeSinceLast = DateTime.UtcNow - _lastMinutePass;
            if (timeSinceLast.TotalMinutes > 1)
            {
                OneMinutePing();
                _lastMinutePass = DateTime.UtcNow;
            }

        }
        private void SecondPing()
        {
            TimeSpan timeSinceLast = DateTime.UtcNow - _lastSecondPass;
            if (timeSinceLast.TotalSeconds > 1)
            {
                OneMinutePing();
                _lastSecondPass = DateTime.UtcNow;
            }
        }
        private void HourPing()
        {
            TimeSpan timeSinceLast = DateTime.UtcNow - _lastHourPass;
            if (timeSinceLast.TotalHours > 1)
            {
                OneMinutePing();
                _lastHourPass = DateTime.UtcNow;
            }
        }
        protected virtual void OneMinutePing()
        {

        }
        protected virtual void OneSecondPing()
        {

        }
        protected virtual void OneHourPing()
        {

        }
        protected virtual void SleepCyclePing()
        {

        }
    }
}
