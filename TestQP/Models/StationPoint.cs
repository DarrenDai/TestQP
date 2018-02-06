using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Models
{
    public class StationPoint : NotifyObject
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }


        private bool _isBling;

        public bool IsBling
        {
            get { return _isBling; }
            set { _isBling = value; OnPropertyChanged(); }
        }

        private int _order;

        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(); }
        }


    }
}
