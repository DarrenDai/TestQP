using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Converters;

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
        private UInt32 _stationId = 0x6000;//BCD 4
        private UInt16 _sequenceNO = 0x0001;//WORD

        public const uint HeaderLength = (2 + 2 + 1 + 1 + 4 + 2); //12 bytes
        private byte[] _header = new byte[HeaderLength];
        #endregion

        #region Public properties

        public UInt16 MessageId
        {
            get { return GetPropertyWithOffset(0); }
            set
            {
                _messageId = value;

                SetHeaderValue(0, value);
            }
        }

        public UInt16 MessageProperty
        {
            get { return GetPropertyWithOffset(2); }
            set
            {
                _messageProperty = value;
                SetHeaderValue(2, value);
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
            get { return GetProperty32WithOffset(6); }
            set
            {
                _stationId = value;
                SetHeaderValue(6, value);
            }
        }

        public UInt16 SequenceNO
        {
            get { return GetPropertyWithOffset(10); }
            set
            {
                _sequenceNO = value;
                SetHeaderValue(10, value);
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

        private void SetHeaderValue(int offset, UInt16 val)
        {
            byte[] tempBytes = ByteConverter.Uint16ToBytes(val);
            for (int i = 0; i < 2; i++)
            {
                _header[i + offset] = tempBytes[i];
            }
        }

        private void SetHeaderValue(int offset, UInt32 val)
        {
            byte[] tempBytes = ByteConverter.UInt32ToBytes(val);
            for (int i = 0; i < 4; i++)
            {
                _header[i + offset] = tempBytes[i];
            }
        }

        private UInt16 GetPropertyWithOffset(int offset)
        {
            byte[] tempBytes = new byte[2];
            for (int i = 0; i < 2 && i < _header.Length - offset; i++)
            {
                tempBytes[i] = _header[offset + i];
            }

            return ByteConverter.BytesToUint16(tempBytes);
        }

        private UInt32 GetProperty32WithOffset(int offset)
        {
            byte[] tempBytes = new byte[4];
            for (int i = 0; i < 4 && i < _header.Length - offset; i++)
            {
                tempBytes[i] = _header[offset + i];
            }

            return ByteConverter.BytesToUInt32(tempBytes);
        }

        #endregion
    }
}
