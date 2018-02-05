using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Constants;
using TestQP.Converters;
using TestQP.Extensions;

namespace TestQP.Sockets
{
    public class MessageHeader
    {
        #region Private fields

        // Messageid should be function id
        private UInt16 _messageId = 0x0602; //WORD
        private UInt16 _messageProperty = 0x0025;//WORD
        private byte _protocolVersion = 0x01;
        private byte _token = 0x00;
        private UInt32 _stationId = 0x60000001;//BCD 4
        private UInt16 _sequenceNO = 0x0004;//WORD

        public const uint HeaderLength = (2 + 2 + 1 + 1 + 4 + 2); //12 bytes
        private byte[] _header = new byte[HeaderLength];
        #endregion

        #region Constructor

        public MessageHeader()
        {
            MessageId = (UInt16)FunctionEnum.CLIENT_LOGON;
            MessageProperty = 0x0025;
            ProtocolVersion = 0x01;
            Token = 0x00;
            StationId = 0x60000001;
            SequenceNO = 0x0004;
        }

        #endregion

        #region Public properties

        public UInt16 MessageId
        {
            get { return _header.GetUInt16PropertyWithOffset(0); }
            set
            {
                _messageId = value;

                _header.SetUInt16PropertyWithOffset(0, value);
            }
        }

        public UInt16 MessageProperty
        {
            get { return _header.GetUInt16PropertyWithOffset(2); }
            set
            {
                _messageProperty = value;
                _header.SetUInt16PropertyWithOffset(2, value);
            }
        }

        public byte ProtocolVersion
        {
            get { return _header[4]; }
            set
            {
                _protocolVersion = value;

                _header[4] = value;
            }
        }

        public byte Token
        {
            get { return _header[5]; }
            set
            {
                _token = value;
                _header[5] = value;
            }
        }

        public UInt32 StationId
        {
            get { return _header.GetUInt32PropertyWithOffset(6); }
            set
            {
                _stationId = value;
                _header.SetUInt32PropertyWithOffset(6, value);
            }
        }

        public UInt16 SequenceNO
        {
            get { return _header.GetUInt16PropertyWithOffset(10); }
            set
            {
                _sequenceNO = value;
                _header.SetUInt16PropertyWithOffset(10, value);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get header bytes
        /// </summary>
        /// <returns></returns>
        public byte[] GetHeader()
        {
            return _header;
        }

        /// <summary>
        /// initialize header with bytes
        /// </summary>
        /// <param name="bytes"></param>
        public void FromBytes(byte[] bytes)
        {
            if (bytes.Length < _header.Length)
                throw new ApplicationException("参数不合法，提供的字节长度小于消息头长度！");

            for (int i = 0; i < _header.Length; i++)
            {
                _header[i] = bytes[i];
            }
        }

        #endregion

        #region private methods

        #endregion
    }
}
