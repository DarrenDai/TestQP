using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQP.Converters
{
    /// <summary>
    /// 字节 整形 转换类 网络格式转换为内存格式
    /// </summary>
    public class ByteConverter
    {
        public static byte[] UInt32ToBytes(UInt32 source, int number)
        {
            byte[] t = new byte[number];
            t = BitConverter.GetBytes(source);
            byte temp;
            for (int i = t.Length - 1; i > t.Length / 2; i--)
            {
                temp = t[i];
                t[i] = t[t.Length - 1 - i];
                t[t.Length - 1 - i] = temp;
            }
            return (t);
        }

        /// <summary>
        /// 返回字节数组代表的整数数字，4个数组
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static UInt32 BytesToUInt32(byte[] bytes, int startIndex)
        {
            byte[] t = new byte[4];
            for (int i = 0; i < 4 && i < bytes.Length - startIndex; i++)
            {
                t[i] = bytes[startIndex + i];
            }

            byte b = t[0];
            t[0] = t[3];
            t[3] = b;
            b = t[1];
            t[1] = t[2];
            t[2] = b;

            return BitConverter.ToUInt32(t, 0);
        }

        public static UInt32 BytesToUInt32(byte[] b, int startIndex, int number)
        {
            byte[] t = new Byte[number];
            for (int i = 0; i < number && i < b.Length - startIndex; i++)
            {
                t[i] = b[startIndex + i];
            }

            byte temp;
            for (int i = t.Length - 1; i > t.Length / 2; i--)
            {
                temp = t[i];
                t[i] = t[t.Length - 1 - i];
                t[i] = temp;
            }
            return (BitConverter.ToUInt32(t, 0));
        }

        /// <summary>
        /// 没有指定起始索引
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static UInt32 BytesToUInt32(byte[] bytes)
        {
            return (BytesToUInt32(bytes, 0));
        }

        /// <summary>
        /// 转换整形数据网络次序的字节数组
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static byte[] UInt32ToBytes(UInt32 val)
        {
            byte[] t = BitConverter.GetBytes(val);
            byte b = t[0];

            t[0] = t[3];
            t[3] = b;

            b = t[1];
            t[1] = t[2];
            t[2] = b;
            return (t);
        }

        public static byte[] Uint16ToBytes(UInt16 number)
        {
            byte[] temp = BitConverter.GetBytes(number);
            byte b = temp[0];

            temp[0] = temp[1];
            temp[1] = b;
            return (temp);
        }

        public static UInt16 BytesToUint16(byte[] bytes)
        {
            byte b = bytes[0];
            bytes[0] = bytes[1];
            bytes[1] = b;

            return BitConverter.ToUInt16(bytes, 0);
        }
    }
}
