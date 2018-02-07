using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestQP.Converters;

namespace TestQP.Tests.Converters
{
    [TestClass]
    public class ByteConverterTests
    {
        [TestMethod]
        public void TestBCDStringToBytes()
        {
            var bytes = ByteConverter.BCDStringToBytes("60D0Af01");

            Assert.AreEqual("60-D0-AF-01", BitConverter.ToString(bytes), true);
        }

        [TestMethod]
        public void TestBCDBytesToString()
        {
            var str = ByteConverter.BCDBytesToString(new byte[] { 0x60, 0x00, 0x00, 0x01 });

            Assert.AreEqual("60000001", str, true);
        }
    }
}
