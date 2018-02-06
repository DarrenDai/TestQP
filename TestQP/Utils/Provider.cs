using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Utils
{
    public class Provider
    {
        private static IEventAggregator _eventAggregator;

        public static IEventAggregator EventAggregator
        {
            get
            {
                if (_eventAggregator == null)
                    _eventAggregator = new EventAggregator();// (IEventAggregator)ServiceLocator.Current.GetService(typeof(IEventAggregator));

                return _eventAggregator;
            }
        }
    }
}
