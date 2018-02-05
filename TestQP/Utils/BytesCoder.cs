using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Utils
{
    /// <summary>
    /// 二进制流转义
    /// </summary>
    public class BytesCoder
    {
        //定义如下： 
        //0x8e <————> 0x8d 后紧跟一个0x01； 
        //0x8d <————> 0x8d 后紧跟一个0x02； 
        //转义处理过程如下： 
        //发送消息时：消息封装——>计算并填充校验码——>转义； 
        //接收消息时：转义还原——>验证校验码——>解析消息；

        /// <summary>
        /// 发送消息时转义
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <returns>字节流</returns>
        public static byte[] Encode(byte[] bytes)
        {
            //0x8d <————> 0x8d 后紧跟一个0x02； 
            //0x8e <————> 0x8d 后紧跟一个0x01； 
            int counter = 0;
            //count special charactors
            for (int i = 1; i < bytes.Length - 2; i++)
            {
                if (bytes[i] == 0x8e || bytes[i] == 0x8d) counter++;
            }

            if (counter == 0) return bytes;
            var retBytes = new byte[bytes.Length + counter];

            //encode
            int retIndex = 0;
            for (int i = 0; i < bytes.Length; i++, retIndex++)
            {
                // marks
                if (i == 0 || i == bytes.Length - 1 || (bytes[i] != 0x8e & bytes[i] != 0x8d))
                {
                    retBytes[retIndex] = bytes[i];
                    continue;
                }

                retBytes[retIndex] = 0x8d;
                retBytes[++retIndex] = bytes[i] == 0x8e ? (byte)0x01 : (byte)0x02;
            }

            return retBytes;
        }

        /// <summary>
        /// 接受消息时转义
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <returns>字节流</returns>
        public static byte[] Decode(byte[] bytes)
        {
            //0x8d 后紧跟一个0x02 <————> 0x8d ； 
            //0x8d 后紧跟一个0x01 <————> 0x8e ； 

            int counter = 0;
            for (int i = 1; i < bytes.Length - 2; i++)
            {
                if (bytes[i] == 0x8d) counter++;
            }
            if (counter == 0) return bytes;

            var retBytes = new byte[bytes.Length - counter];
            int retIndex = 0;
            for (int i = 0; i < bytes.Length; i++, retIndex++)
            {
                if (i == 0 || i == bytes.Length - 1 || (bytes[i] != 0x8d))
                {
                    retBytes[retIndex] = bytes[i];
                    continue;
                }

                retBytes[retIndex] = bytes[++i] == (byte)0x01 ? (byte)0x8e : (byte)0x8d;
            }

            return retBytes;
        }
    }
}
