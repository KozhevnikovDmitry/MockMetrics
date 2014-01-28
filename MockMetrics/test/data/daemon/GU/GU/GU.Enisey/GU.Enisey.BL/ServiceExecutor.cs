using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace GU.Enisey.BL
{
    public class ServiceExecutor : IDisposable
    {
        private Timer _timer;
        private double _interval;
        private Action _action;
        private volatile object _locker = new object();

        public ServiceExecutor(Action action, double interval)
        {
            _action = action;
            _interval = interval;
            _timer = new Timer(interval);
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Start();
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (_locker)
            {
                _timer.Stop();
                _action.Invoke();
                _timer.Interval = _interval;
                _timer.Start();
            }
        }

        #region IDisposable

        public void Dispose()
        {
            _timer.Dispose();
        }

        #endregion
    }
}
