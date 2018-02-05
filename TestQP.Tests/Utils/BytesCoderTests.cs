using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Utils;

namespace TestQP.Tests
{
    [TestClass]
    public class BytesCoderTests
    {
        [TestMethod]
        public void TestEncode()
        {
            byte[] temp = new byte[] { 0x8e, 0x30, 0x8e, 0x08, 0x8d, 0x55, 0x8e };
            var bytes = BytesCoder.Encode(temp);
            var encodedStr = BitConverter.ToString(bytes).Replace("-", " ");
            Assert.AreEqual("8e 30 8d 01 08 8d 02 55 8e", encodedStr, true);
        }

        [TestMethod]
        public void TestDecode()
        {
            byte[] temp = new byte[] { 0x8e, 0x30, 0x8d, 0x01, 0x08, 0x8d, 0x02, 0x55, 0x8e };
            var bytes = BytesCoder.Decode(temp);
            var encodedStr = BitConverter.ToString(bytes).Replace("-", " ");
            Assert.AreEqual("8e 30 8e 08 8d 55 8e", encodedStr, true);
        }
    }
}
