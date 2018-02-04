using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestQP.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var bytes = BitConverter.GetBytes(0x01020304);
            var str = BitConverter.ToString(bytes, 0, bytes.Length);


            var a = new byte[] { 1, 2, 3 };
            var b = new byte[] { 4, 5, 6, 7, 8 };

            //Buffer.BlockCopy(a, 0, b, 0, a.Length);
            //a.CopyTo(b, 0);
            //Array.Copy(a, b, a.Length);
            var newArr = new byte[a.Length + b.Length];
            Array.Copy(a, newArr, a.Length);
            Array.Copy(b, 0, newArr, a.Length, b.Length);

            var test = new byte[] { 0x09, 0xA6, 0x00, 0x16, 0x01, 0x00, 0x60, 0x00, 0x00, 0x01, 0x03, 0x2F, 0x00, 0xA0, 0x18, 0x02, 0x02, 0x05, 0x20, 0x25, 0x05 };
            byte hash = test[0];
            for (int i = 1; i < test.Length; i++)
            {
                hash ^= test[i];
            }

        }
    }
}
