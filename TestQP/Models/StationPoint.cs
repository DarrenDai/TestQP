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
            set { _name = value; OnPropertyChanged(() => Name); }
        }


        private bool _isBling;

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

        private int _order;

        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(() => Order); }
        }

        private bool _isCurrentStation;

        public bool IsCurrentStation
        {
            get { return _isCurrentStation; }
            set { _isCurrentStation = value; }
        }

    }
}
