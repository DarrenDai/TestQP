using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Constants.Enums;
using TestQP.Extensions;
using TestQP.Models;

namespace TestQP.Sockets.BodyDefinitions
{
    public class RealTimeDataBody : IMessageBody
    {
        #region private fields

        private byte[] _bodyBytes;

        #endregion

        #region Public Properties

        /// <summary>
        /// 线路ID
        /// DWORD
        /// </summary>
        public int RouteId
        {
            get { return (int)_bodyBytes.GetUInt32PropertyWithOffset(0); }
            set
            { _bodyBytes.SetUInt32PropertyWithOffset(0, (UInt32)value); }
        }

        /// <summary>
        /// 线路方向 0 – 上行，1 – 下行
        /// Byte
        /// </summary>
        public RouteDirectionEnum RouteDirection
        {
            get { return (RouteDirectionEnum)_bodyBytes[4]; }
            set { _bodyBytes[4] = (byte)value; }
        }

        /// <summary>
        /// 线路上的车辆数 线路单方向上运营中的车辆数
        /// Byte
        /// ***重要***
        ///当一条线路上的运营车辆变成0的时候，服务器会向设备推送一次此协议，且此值为0表示线路上没有运营中的车辆。
        /// </summary>
        public int BusCount
        {
            get { return _bodyBytes[5]; }
            set { _bodyBytes[5] = (byte)value; }
        }

        /// <summary>
        /// 车辆位置[n]
        /// </summary>
        public List<BusRealTimeLocation> BusLocations
        {
            //每一辆车由2个字节表示，其结构如下：
            //第1个字节：
            //位置信息，表示站点序号。（最多255个站）
            //第2个字节：用bit表示以下信息
            //bit: 8，位置信息：站内（1）或站外（0）。
            //bit: 5 – 7，该位置有几辆车。
            //bit: 4，是否过站。
            //bit: 3，是否当前站（灯是否闪烁）。
            //bit: 1- 2，保留。
            //
            //如：
            //0000 0001 0101 0000
            //表示：
            //第1.5站的位置有1辆车，且该位置在当前站之后（未过站）。
            //0000 0101 1010 0100
            //表示：
            //第5站的位置有2辆车，且该位置是当前站。
            get
            {
                var buses = new List<BusRealTimeLocation>();
                for (int i = 6; i < BusCount * 2 + 6; i = i + 2)
                {
                    byte location = _bodyBytes[i + 1];
                    var temp = new BusRealTimeLocation();

                    temp.StationNo = (int)_bodyBytes[i];

                    temp.IsInstation = (location & 0x80) == 0x80;
                    temp.StationBusCount = (int)((location & 0x70) >> 4 & 0xF);
                    temp.IsPassed = (location & 0x08) == 0x08;
                    temp.IsBling = (location & 0x04) == 0x04;

                    buses.Add(temp);
                }
                return buses;
            }
            //get { return _bodyBytes[4]; }
            //set { _bodyBytes[4] = (byte)value; }
        }

        /// <summary>
        /// 定制信息
        /// </summary>
        public string CustomedInfo
        {
            //墨水屏浦东公交定制版：
            //定制信息1字节，最近1辆车距离本站几分钟，FF（-1）表示待发。
            //
            //Android定制：
            //最近一辆车的状态信息。如：5站、待发等。
            get
            {
                byte[] bytes = new byte[_bodyBytes.Length - BusCount * 2 - 6];
                Array.Copy(_bodyBytes, BusCount * 2 + 6, bytes, 0, bytes.Length);
                return Encoding.GetEncoding("gbk").GetString(bytes).Trim('\0');
            }
            //get { return _bodyBytes[4]; }
            //set { _bodyBytes[4] = (byte)value; }
        }
        #endregion

        #region Interface implements

        public byte[] GetBodyBytes()
        {
            return _bodyBytes;
        }

        public void FromBytes(byte[] bytes)
        {
            _bodyBytes = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                _bodyBytes[i] = bytes[i];
            }
        }

        public int GetBodyLength()
        {
            return _bodyBytes != null ? _bodyBytes.Length : 0;
        }

        #endregion
    }
}
