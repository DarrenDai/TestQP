using System;
using System.Collections.Generic;
using System.Configuration;
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
        //private UInt16 _messageId = 0x0602; //WORD
        //private UInt16 _messageProperty = 0x0025;//WORD
        //private byte _protocolVersion = 0x01;
        //private byte _token = 0x00;
        //private UInt32 _stationId = 0x60000001;//BCD 4
        //private UInt16 _sequenceNO = 0x0004;//WORD

        public const uint HeaderLength = (2 + 2 + 1 + 1 + 4 + 2); //12 bytes
        private byte[] _header = new byte[HeaderLength];

        #endregion

        #region Constructor

        public MessageHeader()
        {
            MessageId = FunctionEnum.CLIENT_LOGON;
            MessageProperty = 0x0025;
            ProtocolVersion = 0x01;
            Token = 0x00;
            //strin to BCD
            StationNo = ConfigurationManager.AppSettings["StationId"];
            SequenceNO = 0x0004;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// 消息 ID WORD
        /// </summary>
        public FunctionEnum MessageId
        {
            get { return (FunctionEnum)_header.GetUInt16PropertyWithOffset(0); }
            set
            {
                //_messageId = (UInt16)value;
                _header.SetUInt16PropertyWithOffset(0, (UInt16)value);
            }
        }

        /// <summary>
        /// 消息体属性 WORD 
        /// 不包括协议包头和协议包尾的数据总长度(字节数)，高字节在前，低字节在后；
        /// </summary>
        public int MessageProperty
        {
            get { return _header.GetUInt16PropertyWithOffset(2); }
            set
            {
                //_messageProperty = (UInt16)value;
                _header.SetUInt16PropertyWithOffset(2, (UInt16)value);
            }
        }

        /// <summary>
        /// 版本 Byte
        /// </summary>
        public int ProtocolVersion
        {
            get { return _header[4]; }
            private set
            {
                // _protocolVersion = (byte)value;
                _header[4] = (byte)value;
            }
        }

        /// <summary>
        /// Token Byte
        /// </summary>
        public int Token
        {
            get { return _header[5]; }
            set
            {
                //_token = (byte)value;
                _header[5] = (byte)value;
            }
        }

        /// <summary>
        /// 电子站牌标识	 BCD[4]
        /// </summary>
        public string StationNo
        {
            get { return _header.GetBCDStringWithOffset(6, 4); }
            set //private
            {
                // _stationId = (UInt32)value;
                _header.SetBCDFromStringWithOffset(value, 6);
            }
        }

        /// <summary>
        /// 流水号 WORD
        /// </summary>
        public int SequenceNO
        {
            get { return _header.GetUInt16PropertyWithOffset(10); }
            set
            {
                //_sequenceNO = (UInt16)value;
                _header.SetUInt16PropertyWithOffset(10, (UInt16)value);
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
