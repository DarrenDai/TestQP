﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Models
{
    public class BusLocationInfo : NotifyObject
    {
        private int _stationNo;

        public int StationNo
        {
            get { return _stationNo; }
            set { _stationNo = value; OnPropertyChanged(() => StationNo); }
        }

        private bool _isInstation;

        public bool IsInstation
        {
            get { return _isInstation; }
            set { _isInstation = value; OnPropertyChanged(() => IsInstation); }
        }

        private int _stationBusCount;

        public int StationBusCount
        {
            get { return _stationBusCount; }
            set { _stationBusCount = value; OnPropertyChanged(() => StationBusCount); }
        }

        private bool _isPassed;

        public bool IsPassed
        {
            get { return _isPassed; }
            set { _isPassed = value; OnPropertyChanged(() => IsPassed); }
        }

        private bool _isBling;

        public bool IsBling
        {
            get { return _isBling; }
            set { _isBling = value; OnPropertyChanged(() => IsBling); }
        }

        //private string _customedString;

        //public string CustomedString
        //{
        //    get { return _customedString; }
        //    set { _customedString = value; OnPropertyChanged(() => CustomedString); }
        //}

    }
}
