using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Converters;

namespace TestQP.Extensions
{
    public static class ByteAarryExtensions
    {
        public static UInt16 GetUInt16PropertyWithOffset(this byte[] bytes, int offset)
        {
            byte[] tempBytes = new byte[2];
            for (int i = 0; i < 2 && i < bytes.Length - offset; i++)
            {
                tempBytes[i] = bytes[offset + i];
            }

            return ByteConverter.BytesToUint16(tempBytes);
        }

        public static UInt32 GetUInt32PropertyWithOffset(this byte[] bytes, int offset)
        {
            byte[] tempBytes = new byte[4];
            for (int i = 0; i < 4 && i < bytes.Length - offset; i++)
            {
                tempBytes[i] = bytes[offset + i];
            }

            return ByteConverter.BytesToUInt32(tempBytes);
        }

        public static void SetUInt16PropertyWithOffset(this byte[] bytes, int offset, UInt16 val)
        {
            byte[] tempBytes = ByteConverter.Uint16ToBytes(val);
            for (int i = 0; i < 2; i++)
            {
                bytes[i + offset] = tempBytes[i];
            }
        }

        public static void SetUInt32PropertyWithOffset(this byte[] bytes, int offset, UInt32 val)
        {
            byte[] tempBytes = ByteConverter.UInt32ToBytes(val);
            for (int i = 0; i < 4; i++)
            {
                bytes[i + offset] = tempBytes[i];
            }
        }

        public static string GetBCDStringWithOffset(this byte[] bytes, int offset, int count)
        {
            var tempBytes = new byte[count];
            Array.Copy(bytes, offset, tempBytes, 0, count);
            return ByteConverter.BCDBytesToString(tempBytes);
        }

        public static void SetBCDFromStringWithOffset(this byte[] bytes, string bcd, int offset)
        {
            if (bcd == null) return;

            var tempBytes = ByteConverter.BCDStringToBytes(bcd);
            Array.Copy(tempBytes, 0, bytes, offset, tempBytes.Length);
        }
    }
}
