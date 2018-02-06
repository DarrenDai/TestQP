using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Extensions;

namespace TestQP.Sockets.BodyDefinitions
{
    public class ServerGeneralAnsBody : IMessageBody
    {
        #region Private fields

        private byte[] _bodyBytes = new byte[2 + 2 + 1];

        #endregion

        #region Public properties

        /// <summary>
        /// 对应的电子站牌消息的流水号
        /// WORD
        /// </summary>
        public int SequenceNO
        {
            get { return _bodyBytes.GetUInt16PropertyWithOffset(0); }
            set
            {
                _bodyBytes.SetUInt16PropertyWithOffset(0, (UInt16)value);
            }
        }

        /// <summary>
        /// 对应的电子站牌消息的 ID
        /// WORD
        /// </summary>
        public int AnsMessageId
        {
            get { return _bodyBytes.GetUInt16PropertyWithOffset(2); }
            set
            {
                _bodyBytes.SetUInt16PropertyWithOffset(2, (UInt16)value);
            }
        }

        /// <summary>
        /// 0：成功/确认；1：失败；2：消息有误；
        /// Byte
        /// </summary>
        public int Result
        {
            get { return _bodyBytes[4]; }
            set
            {
                _bodyBytes[4] = (byte)value;
            }
        }

        #endregion

        #region  Interface implements

        public byte[] GetBodyBytes()
        {
            return _bodyBytes;
        }

        public void FromBytes(byte[] bytes)
        {
            if (bytes.Length < _bodyBytes.Length)
                throw new ApplicationException("参数错误，入参长度小于消息体长度。");

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
