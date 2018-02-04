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
    public class MessageTests
    {
        [TestMethod]
        public void TestMessage()
        {
            var message = new Message();
            message.MessageBody = new MessageBody();
            message.Header = new MessageHeader()
            {
                MessageId = (UInt16)FunctionEnum.CLIENT_LOGON,
                MessageProperty = 0x0025,
                ProtocolVersion = 0x01,
                Token = 0x00,
                StationId = 0x60000001,
                SequenceNO = 0x0004
            };

            var bytes = message.GetMessageBytes();
            var data = BitConverter.ToString(bytes, 0, bytes.Length).Replace("-", " ");
            Assert.AreEqual("8E 06 02 00 25 01 00 60 00 00 01 00 04 46 72 65 65 52 54 4F 53 20 56 38 2E 32 2E 33 00 00 00 32 2E 30 2E 30 37 21 8E",
                data, true);
        }
    }
}
