using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Events
{
    public class Events
    {
        public class LogEvent : CompositePresentationEvent<string>
        {
        }
    }
}
