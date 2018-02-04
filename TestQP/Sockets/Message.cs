using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Constants;
using TestQP.Converters;

namespace TestQP.Sockets
{
    /// <summary>
    /// 消息由标识位、消息头、消息体和校验码组成
    /// </summary>
    public class Message : IDisposable
    {
        #region Private fields

        //标识位：采用0x8e表示，若校验码、消息头以及消息体中出现0x8e，则要进行转义处理
        //0x8e <————> 0x8d 后紧跟一个0x01； 
        //0x8d <————> 0x8d 后紧跟一个0x02；
        private const byte _flagByte = 0x8e;
        private byte _hashCode;

        #endregion

        #region Constructor

        public Message()
        {

        }

        #endregion

        #region public properties


        //当第10位为1，表示消息体经过RSA算法加密。
        public EncrptMethodEnum EncrptMethod { get; set; }

        /// <summary>
        /// 消息头
        /// </summary>
        public MessageHeader Header { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        public MessageBody MessageBody { get; set; }

        #endregion

        #region Public methods

        public byte[] GetMessageBytes()
        {
            if (Header == null || MessageBody == null)
            {
                return null;
            }

            SetHeaderMessageProperty();

            //定义如下： 
            //0x8e <————> 0x8d 后紧跟一个0x01； 
            //0x8d <————> 0x8d 后紧跟一个0x02； 
            //转义处理过程如下： 
            //发送消息时：消息封装——>计算并填充校验码——>转义； 
            //接收消息时：转义还原——>验证校验码——>解析消息；
            var header = Header.GetHeader();
            var body = MessageBody.GetMessageBody();

            var combineArr = new byte[1 + MessageHeader.HeaderLength + body.Length + 1 + 1];
            Array.Copy(header, 0, combineArr, 1, header.Length);
            Array.Copy(body, 0, combineArr, header.Length + 1, body.Length);

            //hash
            _hashCode = GetHashCode(header, body);
            // encode
            combineArr[0] = _flagByte;
            combineArr[combineArr.Length - 2] = _hashCode;
            combineArr[combineArr.Length - 1] = _flagByte;

            return combineArr;
        }

        public void FromBytes(byte[] bytes)
        {

        }

        #endregion

        #region Private methods

        private void SetHeaderMessageProperty()
        {
            UInt16 bodyProperty = 0x0000;
            bodyProperty = (UInt16)(MessageHeader.HeaderLength + MessageBody.BodyPart.Length + 1);
            bodyProperty |= (UInt16)EncrptMethod;
            Header.MessageProperty = bodyProperty;
            //return ByteConverter.Uint16ToBytes(bodyProperty);
        }

        private byte GetHashCode(byte[] header, byte[] body)
        {
            byte hash = header[0];
            for (int i = 1; i < header.Length; i++)
            {
                hash ^= header[i];
            }

            for (int i = 0; i < body.Length; i++)
            {
                hash ^= body[i];
            }

            return hash;
        }

        #endregion

        #region IDispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDispose)
        {
            if (isDispose) { this.MessageBody = null; }
        }

        #endregion
    }
}
