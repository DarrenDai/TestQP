using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Models
{
    public class BusRealTimeLocation : NotifyObject
    {
        private int _stationNo;
        private bool _isInstation;
        private int _stationBusCount;
        private bool _isPassed;
        private bool _isBling;

        public int StationNo
        {
            get { return _stationNo; }
            set { _stationNo = value; OnPropertyChanged(() => StationNo); }
        }

        public bool IsInstation
        {
            get { return _isInstation; }
            set { _isInstation = value; OnPropertyChanged(() => IsInstation); }
        }

        public int StationBusCount
        {
            get { return _stationBusCount; }
            set { _stationBusCount = value; OnPropertyChanged(() => StationBusCount); }
        }

        public bool IsPassed
        {
            get { return _isPassed; }
            set { _isPassed = value; OnPropertyChanged(() => IsPassed); }
        }

        public bool IsBling
        {
            get { return _isBling; }
            set { _isBling = value; OnPropertyChanged(() => IsBling); }
        }
    }
}
