using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Models
{
    public class BusRouteInfo : NotifyObject
    {
        private int _routeId;
        private string _routeNo;
        private string _startStation;
        private string _endStation;
        private string _startStationTimeRange;
        private string _endStationTimeRange;
        private List<StationPoint> _stations = new List<StationPoint>();

        public int RouteId
        {
            get { return _routeId; }
            set { _routeId = value; OnPropertyChanged(() => RouteId); }
        }

        public string RouteNo
        {
            get { return _routeNo; }
            set { _routeNo = value; OnPropertyChanged(() => RouteNo); }
        }

        public string StartStation
        {
            get { return _startStation; }
            set { _startStation = value; OnPropertyChanged(() => StartStation); }
        }

        public string EndStation
        {
            get { return _endStation; }
            set { _endStation = value; OnPropertyChanged(() => EndStation); }
        }

        public string StartStationTimeRange
        {
            get { return _startStationTimeRange; }
            set { _startStationTimeRange = value; OnPropertyChanged(() => StartStationTimeRange); }
        }

        public string EndStationTimeRange
        {
            get { return _endStationTimeRange; }
            set { _endStationTimeRange = value; OnPropertyChanged(() => EndStationTimeRange); }
        }

        public List<StationPoint> Stations
        {
            get { return _stations; }
            set { _stations = value; }
        }

    }
}
