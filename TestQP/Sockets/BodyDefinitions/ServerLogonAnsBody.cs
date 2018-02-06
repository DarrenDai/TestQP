using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Constants.Enums;
using TestQP.Converters;

namespace TestQP.Sockets.BodyDefinitions
{
    public class ServerLogonAnsBody : IMessageBody
    {
        #region Private fields

        private byte[] _bodyBytes = new byte[1 + 1 + 7];

        #endregion

        #region Public properties

        /// <summary>
        /// 登录状态：0 – 成功，> 0 失败。
        /// BYTE
        /// </summary>
        public LogonResultEnmu Status
        {
            get
            {
                return (LogonResultEnmu)_bodyBytes[0];
            }
            set
            {
                _bodyBytes[0] = (byte)value;
            }
        }

        /// <summary>
        /// TOKEN=0为无效，登录失败时此值为0。
        /// BYTE
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
        /// 当前系统时间日期，年月日周时分秒。 
        /// BCD[7]
        /// </summary>
        public DateTime Time
        {
            get
            {
                var tempBytes = new byte[7];
                Array.Copy(_bodyBytes, 2, tempBytes, 0, tempBytes.Length);
                return ByteConverter.BCDToDatetime(tempBytes);
            }
            set
            {
                var tempBytes = ByteConverter.DatetimeToBCD(value);
                Array.Copy(tempBytes, 0, _bodyBytes, 2, tempBytes.Length);
            }
        }

        #endregion

        #region Interface implements

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

        #endregion
    }
}
