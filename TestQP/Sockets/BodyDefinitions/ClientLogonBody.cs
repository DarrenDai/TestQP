using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Sockets.BodyDefinitions
{
    public class ClientLogonBody : IMessageBody
    {
        #region Private fields

        #endregion

        #region Public Properties

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 运行环境版本
        /// </summary>
        public string RuntimeVersion { get; set; }

        /// <summary>
        /// 前端程序版本
        /// </summary>
        public string FrontEndVersion { get; set; }

        #endregion

        #region  Interface implements

        public byte[] GetBodyBytes()
        {
            return GetBytes();
        }

        public void FromBytes(byte[] bytes)
        {
            var stringArr = Encoding.GetEncoding("gbk").GetString(bytes, 0, bytes.Length).Split('\0');
            if (stringArr.Length < 3) return;
            Password = stringArr[0];
            RuntimeVersion = stringArr[1];
            FrontEndVersion = stringArr[2];
        }

        public int GetBodyLength()
        {
            return GetBytes().Length;
        }

        private byte[] GetBytes()
        {
            return Encoding.GetEncoding("gbk").GetBytes(string.Format("{0}\0{1}\0{2}\0", Password, RuntimeVersion, FrontEndVersion));
        }

        #endregion
    }
}
