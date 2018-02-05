using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace TestQP.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //  var bytes = BitConverter.GetBytes(0x01020304);
            //  var str = BitConverter.ToString(bytes);
            //
            //
            //  var a = new byte[] { 1, 2, 3 };
            //  var b = new byte[] { 4, 5, 6, 7, 8 };
            //
            //  //Buffer.BlockCopy(a, 0, b, 0, a.Length);
            //  //a.CopyTo(b, 0);
            //  //Array.Copy(a, b, a.Length);
            //  var newArr = new byte[a.Length + b.Length];
            //  Array.Copy(a, newArr, a.Length);
            //  Array.Copy(b, 0, newArr, a.Length, b.Length);
            //
            //  var test = new byte[] { 0x09, 0xA6, 0x00, 0x16, 0x01, 0x00, 0x60, 0x00, 0x00, 0x01, 0x03, 0x2F, 0x00, 0xA0, 0x18, 0x02, 0x02, 0x05, 0x20, 0x25, 0x05 };
            //  byte hash = test[0];
            //  for (int i = 1; i < test.Length; i++)
            //  {
            //      hash ^= test[i];
            //  }


            //login body
            //var loginBytes = new byte[] { 0x46, 0x72, 0x65, 0x65, 0x52, 0x54, 0x4F, 0x53, 0x20, 0x56, 0x38, 0x2E, 0x32, 0x2E, 0x33, 0x00, 0x00, 0x00, 0x32, 0x2E, 0x30, 0x2E, 0x30, 0x37 };
            var bytes = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x00, 0x41, 0x6E, 0x64, 0x72, 0x6F, 0x69, 0x64, 0x20, 0x35, 0x2E, 0x31, 0x2E, 0x31, 0x00, 0x31, 0x2E, 0x30, 0x2E, 0x30, 0x00 };
            var str1 = Encoding.GetEncoding("gbk").GetString(bytes, 0, bytes.Length);

        }
    }
}
