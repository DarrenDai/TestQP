using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Constants.Enums;

namespace TestQP.Models
{
    public class StationPoint : NotifyObject
    {
        private string _name;
        private bool _isBling;
        private int _order;
        private bool _isCurrentStation;
        private int _currentStationBusCount;
        private BusRelativeEnum _busRelativeLocation;

        public int CurrentStationBusCount
        {
            get { return _currentStationBusCount; }
            set
            {
                _currentStationBusCount = value;
                OnPropertyChanged(() => CurrentStationBusCount);
            }
        }

        public BusRelativeEnum BusRelativeLocation
        {
            get { return _busRelativeLocation; }
            set
            {
                _busRelativeLocation = value;
                OnPropertyChanged(() => BusRelativeLocation);
            }
        }

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
