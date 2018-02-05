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
    /// 消息体部分，分为消息体属性和消息体
    /// </summary>
    public class MessageBody
    {
        #region Private fields

        #endregion

        #region Constructor

        public MessageBody()
        {
            //0x00,0x04,
            BodyPart = new byte[] { 0x46, 0x72, 0x65, 0x65, 0x52, 0x54, 0x4F, 0x53, 0x20, 0x56, 0x38, 0x2E, 0x32, 0x2E, 0x33, 0x00, 0x00, 0x00, 0x32, 0x2E, 0x30, 0x2E, 0x30, 0x37 };
        }

        #endregion

        #region Public properties

        public byte[] BodyPart { get; set; }

        #endregion

        #region Public methods

        public byte[] GetMessageBody()
        {
            if (BodyPart == null)
                BodyPart = new byte[0];

            //int length = 2 + BodyPart.Length;
            //byte[] ret = new byte[length];
            //var propertyBytes = GetBodyPropertyBytes();

            //Array.Copy(propertyBytes, ret, propertyBytes.Length);
            //Array.Copy(BodyPart, 0, ret, propertyBytes.Length, BodyPart.Length);
            return BodyPart;
        }

        public void FromBytes(byte[] bytes)
        {
            if (bytes == null)
                throw new ApplicationException("参数不合法，提供的字节为空！");

            BodyPart = new byte[bytes.Length];
            Array.Copy(bytes, BodyPart, bytes.Length);
        }

        #endregion

        #region private methods



        #endregion
    }
}
