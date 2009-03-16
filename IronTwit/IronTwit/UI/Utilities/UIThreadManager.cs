using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Unite.UI.Utilities
{
    public class UIThreadManager
    {
        private Thread _Thread;

        public UIThreadManager(Thread thread)
        {
            _Thread = thread;
        }

        public void Execute(Action action)
        {
            var dispatcher = Dispatcher.FromThread(_Thread);
            dispatcher.Invoke(DispatcherPriority.Normal, action);
        }
    }
}
