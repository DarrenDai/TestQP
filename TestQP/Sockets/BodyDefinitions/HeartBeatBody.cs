using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Sockets.BodyDefinitions
{
    public class HeartBeatBody : IMessageBody
    {
        #region Private fields

        private byte[] _bodyBytes = new byte[0];

        #endregion

        #region  Interface implements


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
