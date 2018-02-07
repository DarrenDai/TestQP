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
        private bool _isBling;
        private int _order;
        private bool _isCurrentStation;

        //private bool _is

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(() => Name); }
        }

        public bool IsBling
        {
            get { return _isBling; }
            set
            {
                if (_isBling != value)
                {
                    _isBling = value;
                    OnPropertyChanged(() => IsBling);
                }
            }
        }

        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(() => Order); }
        }

        public bool IsCurrentStation
        {
            get { return _isCurrentStation; }
            set { _isCurrentStation = value; }
        }

    }
}
