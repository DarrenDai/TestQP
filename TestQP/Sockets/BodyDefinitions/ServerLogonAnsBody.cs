using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Sockets.BodyDefinitions
{
    public class ServerLogonAnsBody : IMessageBody
    {
        private byte[] _bodyBytes = new byte[1 + 1 + 7];

        #region Public properties

        /// <summary>
        /// 登录状态：0 – 成功，> 0 失败。
        /// </summary>
        public int Status
        {
            get
            {
                return _bodyBytes[0];
            }
            set
            {
                _bodyBytes[0] = (byte)value;
            }
        }

        /// <summary>
        /// TOKEN=0为无效，登录失败时此值为0。
        /// </summary>
        public int Token
        {
            get
            {
                return _bodyBytes[1];
            }
            set
            {
                _bodyBytes[1] = (byte)value;
            }
        }

        /// <summary>
        /// 当前系统时间日期，年月日周时分秒。 BCD[7]
        /// </summary>
        public DateTime Time
        {
            get
            {
                return new DateTime(2000 + ConvertBCDToInt(_bodyBytes[2]),
                          ConvertBCDToInt(_bodyBytes[3]),
                          ConvertBCDToInt(_bodyBytes[4]),
                          ConvertBCDToInt(_bodyBytes[6]),
                          ConvertBCDToInt(_bodyBytes[7]),
                          ConvertBCDToInt(_bodyBytes[8]));
            }
            set
            {
                _bodyBytes[2] = ConvertBCD((byte)(value.Year - 2000));
                _bodyBytes[3] = ConvertBCD((byte)value.Month);
                _bodyBytes[4] = ConvertBCD((byte)value.Day);
                _bodyBytes[6] = ConvertBCD((byte)value.Hour);
                _bodyBytes[7] = ConvertBCD((byte)value.Minute);
                _bodyBytes[8] = ConvertBCD((byte)value.Second);
            }
        }

        #endregion

        public byte[] GetBodyBytes()
        {
            return _bodyBytes;
        }

        public void FromBytes(byte[] bytes)
        {
            if (bytes.Length < _bodyBytes.Length)
                throw new ApplicationException("参数异常，入参长度小于消息体长度！");

            for (int i = 0; i < bytes.Length; i++)
            {
                _bodyBytes[i] = bytes[i];
            }
        }

        public int GetBodyLength()
        {
            return _bodyBytes != null ? _bodyBytes.Length : 0;
        }

        private byte ConvertBCD(byte b)
        {
            byte b1 = (byte)(b / 10);
            byte b2 = (byte)(b % 10);
            return (byte)((b1 << 4) | b2);
        }

        public byte ConvertBCDToInt(byte b)
        {
            byte b1 = (byte)((b >> 4) & 0xF);
            byte b2 = (byte)(b & 0xF);

            return (byte)(b1 * 10 + b2);
        }
    }
}
