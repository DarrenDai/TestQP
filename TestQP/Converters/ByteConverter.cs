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

        #region bytes int

        public static UInt32 BytesToUInt32(byte[] bytes)
        {
            return (BytesToUInt32(bytes, 0));
        }

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

        #endregion

        #region BCD Datetime

        /// <summary>
        /// 7 BCD 转 DateTime
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static DateTime BCDToDatetime(byte[] bytes)
        {
            return new DateTime(2000 + ConvertBCDToInt(bytes[0]),
                         ConvertBCDToInt(bytes[1]),
                         ConvertBCDToInt(bytes[2]),
                         ConvertBCDToInt(bytes[4]),
                         ConvertBCDToInt(bytes[5]),
                         ConvertBCDToInt(bytes[6]));
        }

        /// <summary>
        /// DateTime 转 7 BCD
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static Byte[] DatetimeToBCD(DateTime date)
        {
            byte[] ret = new byte[7];
            ret[0] = ConvertIntToBCD((byte)(date.Year - 2000));
            ret[1] = ConvertIntToBCD((byte)date.Month);
            ret[2] = ConvertIntToBCD((byte)date.Day);
            // [3]week
            ret[4] = ConvertIntToBCD((byte)date.Hour);
            ret[5] = ConvertIntToBCD((byte)date.Minute);
            ret[6] = ConvertIntToBCD((byte)date.Second);

            return ret;
        }

        #endregion

        #region BCD STRING

        /// <summary>
        /// 60000001 to 0x60, 0x00, 0x00, 0x01 
        /// </summary>
        /// <param name="bcd"></param>
        /// <returns></returns>
        public static byte[] BCDStringToBytes(string bcd)
        {
            var bytes = new byte[bcd.Length / 2];
            var chars = bcd.ToArray();
            int counter = 0;
            for (int i = 0; i < chars.Length; i = i + 2)
            {
                byte a = (byte)((GetByteFromChar(chars[i])) << 4);
                byte b = GetByteFromChar(chars[i + 1]);

                bytes[counter++] = (byte)(a | b);
            }

            return bytes;
        }

        private static byte GetByteFromChar(char c)
        {
            //0x30 ==0
            if (c >= '0' && c <= '9')
            {
                return (byte)((byte)c - 0x30);
            }

            //0x41==A
            if (c >= 'A' && c <= 'Z')
            {
                return (byte)((byte)c - 0x41 + 10);
            }

            //0x61==a
            if (c >= 'a' && c <= 'z')
            {
                return (byte)((byte)c - 0x61 + 10);
            }

            return 0x00;
        }

        /// <summary>
        /// 0x60, 0x00, 0x00, 0x01 to 60000001
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BCDBytesToString(byte[] bytes)
        {
            var retString = BitConverter.ToString(bytes).Replace("-", "");

            return retString;
        }

        #endregion

        #region Private methods

        private static byte ConvertIntToBCD(byte b)
        {
            byte b1 = (byte)(b / 10);
            byte b2 = (byte)(b % 10);
            return (byte)((b1 << 4) | b2);
        }

        public static byte ConvertBCDToInt(byte b)
        {
            byte b1 = (byte)((b >> 4) & 0xF);
            byte b2 = (byte)(b & 0xF);

            return (byte)(b1 * 10 + b2);
        }

        #endregion
    }
}
