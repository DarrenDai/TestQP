using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Constants;
using TestQP.Sockets;

namespace TestQP.Tests
{
    [TestClass]
    public class MesssageHeaderTests
    {
        private MessageHeader _header = null;

        // [ClassInitialize]

        [TestInitialize]
        public void Initialize()
        {
            _header = new MessageHeader()
            {
                MessageId = FunctionEnum.CLIENT_LOGON,
                MessageProperty = 0x0025,
                ProtocolVersion = 0x01,
                Token = 0x00,
                StationId = 0x60000001,
                SequenceNO = 0x0004
            };
        }

        [TestMethod]
        public void TestPropertyMessageId()
        {
            var temp = _header.MessageId;

            Assert.IsTrue(FunctionEnum.CLIENT_LOGON == temp);
        }

        [TestMethod]
        public void TestPropertyMessageProperty()
        {
            var temp = _header.MessageProperty;

            Assert.IsTrue(0x0025 == temp);
        }

        [TestMethod]
        public void TestPropertyProtocolVersion()
        {
            var temp = _header.ProtocolVersion;

            Assert.IsTrue(0x01 == temp);
        }

        [TestMethod]
        public void TestPropertyToken()
        {
            var temp = _header.Token;

            Assert.IsTrue(0x00 == temp);
        }

        [TestMethod]
        public void TestPropertyStationId()
        {
            var temp = _header.StationId;

            Assert.IsTrue(0x60000001 == temp);
        }

        [TestMethod]
        public void TestPropertySequenceNO()
        {
            var temp = _header.SequenceNO;

            Assert.IsTrue(0x0004 == temp);
        }

        [TestMethod]
        public void TestHeader()
        {
            var bytes = _header.GetHeader();

            var data = BitConverter.ToString(bytes).Replace("-", " ");
            Assert.AreEqual("06 02 00 25 01 00 60 00 00 01 00 04", data, true);
        }

        [TestMethod]
        public void TestSetHeaderValue()
        {
            var bytesArr = new byte[] { 0x06, 0x02, 0x00, 0x25, 0x01, 0x00, 0x60, 0x00, 0x00, 0x01, 0x00, 0x04 };

            _header.FromBytes(bytesArr);
            var bytes = _header.GetHeader();

            var data = BitConverter.ToString(bytes).Replace("-", " ");
            Assert.AreEqual("06 02 00 25 01 00 60 00 00 01 00 04", data, true);
        }
    }
}
