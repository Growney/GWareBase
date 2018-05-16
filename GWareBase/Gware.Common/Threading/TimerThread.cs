using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
        static TimerThread()
        {
            _timeStopwatch = new Stopwatch();
            _timeStopwatch.Start();
        }
        private delegate void TimerAction();
        private ThreadTimerEvents _actionEvents;
        private TimerAction _timerActions;

        private static Stopwatch _timeStopwatch;
        private long _lastMinutePass;
        private long _lastSecondPass;
        private long _lastHourPass;

        public long LastHourPass
        {
            get { return _lastHourPass; }
            set { _lastHourPass = value; }
        }

        public long LastSecondPass
        {
            get { return _lastSecondPass; }
            set { _lastSecondPass = value; }
        }

        public long LastMinutePass
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

        protected override void ExecuteEntireThreadCycle()
        {
            while (!m_stop)
            {
                Thread.Sleep(500);
                if (!m_stop)
                {
                    _timerActions();
                }
            }
        }

        private void InitiliasePasses()
        {
            _lastMinutePass = _timeStopwatch.ElapsedMilliseconds;
            _lastSecondPass = _timeStopwatch.ElapsedMilliseconds;
            _lastHourPass = _timeStopwatch.ElapsedMilliseconds;
        }
        private void MinutePing()
        {
            TimeSpan timeSinceLast = TimeSpan.FromMilliseconds(_timeStopwatch.ElapsedMilliseconds - _lastMinutePass);
            if (timeSinceLast.TotalMinutes > 1)
            {
                OneMinutePing();
                _lastMinutePass = _timeStopwatch.ElapsedMilliseconds;
            }

        }
        private void SecondPing()
        {
            TimeSpan timeSinceLast = TimeSpan.FromMilliseconds(_timeStopwatch.ElapsedMilliseconds - _lastSecondPass);
            if (timeSinceLast.TotalSeconds > 1)
            {
                OneSecondPing();
                _lastSecondPass = _timeStopwatch.ElapsedMilliseconds;
            }
        }
        private void HourPing()
        {
            TimeSpan timeSinceLast = TimeSpan.FromMilliseconds(_timeStopwatch.ElapsedMilliseconds - _lastHourPass);
            if (timeSinceLast.TotalHours > 1)
            {
                OneHourPing();
                _lastHourPass = _timeStopwatch.ElapsedMilliseconds;
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
