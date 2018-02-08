using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Constants;
using TestQP.Sockets;
using TestQP.Sockets.BodyDefinitions;

namespace TestQP.Tests
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void TestGetMessageBytes()
        {
            var message = new Message();
            message.MessageBody = new ClientLogonBody()
            {
                Password = "123456",
                RuntimeVersion = "Android 5.1.1",
                FrontEndVersion = "1.0.0"
            };
            //var loginBytes = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x00, 0x41, 0x6E, 0x64, 0x72, 0x6F, 0x69, 0x64, 0x20, 0x35, 0x2E, 0x31, 0x2E, 0x31, 0x00, 0x31, 0x2E, 0x30, 0x2E, 0x30, 0x00 };
            //message.MessageBody.FromBytes(loginBytes);

            message.Header = new MessageHeader()
            {
                MessageId = FunctionEnum.CLIENT_LOGON,
                MessageProperty = 0x0025,
                //ProtocolVersion = 0x01,
                Token = 0x00,
                //StationNo = "60000001",
                //SequenceNO = 0x0004
            };

            var bytes = message.GetMessageBytes();
            var data = BitConverter.ToString(bytes).Replace("-", " ");
            Assert.AreEqual("8E 06 02 00 28 01 00 60 00 00 01 00 04 31 32 33 34 35 36 00 41 6E 64 72 6F 69 64 20 35 2E 31 2E 31 00 31 2E 30 2E 30 00 30 8E",
                data, true);
        }

        #region Header

        [TestMethod]
        public void TestFromBytes_Header_MsgId()
        {
            var message = new Message();
            //8E 09 01 00 12 01 00 60 00 00 01 27 6F 00 04 06 02 00 32 8E
            var bytes = new byte[] { 0x8E, 0x09, 0x01, 0x00, 0x12, 0x01, 0x00, 0x60, 0x00, 0x00, 0x01, 0x27, 0x6F, 0x00, 0x04, 0x06, 0x02, 0x00, 0x32, 0x8E };
            message.FromBytes(bytes);

            Assert.AreEqual(FunctionEnum.SERVER_ANS, message.Header.MessageId);
        }

        [TestMethod]
        public void TestFromBytes_Header_MsgLength()
        {
            var message = new Message();
            //8E 09 01 00 12 01 00 60 00 00 01 27 6F 00 04 06 02 00 32 8E
            var bytes = new byte[] { 0x8E, 0x09, 0x01, 0x00, 0x12, 0x01, 0x00, 0x60, 0x00, 0x00, 0x01, 0x27, 0x6F, 0x00, 0x04, 0x06, 0x02, 0x00, 0x32, 0x8E };
            message.FromBytes(bytes);

            Assert.AreEqual(0x0012, message.Header.MessageProperty);
        }

        [TestMethod]
        public void TestFromBytes_Header_Version()
        {
            var message = new Message();
            //8E 09 01 00 12 01 00 60 00 00 01 27 6F 00 04 06 02 00 32 8E
            var bytes = new byte[] { 0x8E, 0x09, 0x01, 0x00, 0x12, 0x01, 0x00, 0x60, 0x00, 0x00, 0x01, 0x27, 0x6F, 0x00, 0x04, 0x06, 0x02, 0x00, 0x32, 0x8E };
            message.FromBytes(bytes);

            Assert.AreEqual(0x01, message.Header.ProtocolVersion);
        }

        [TestMethod]
        public void TestFromBytes_Header_Token()
        {
            var message = new Message();
            //8E 09 01 00 12 01 00 60 00 00 01 27 6F 00 04 06 02 00 32 8E
            var bytes = new byte[] { 0x8E, 0x09, 0x01, 0x00, 0x12, 0x01, 0x00, 0x60, 0x00, 0x00, 0x01, 0x27, 0x6F, 0x00, 0x04, 0x06, 0x02, 0x00, 0x32, 0x8E };
            message.FromBytes(bytes);

            Assert.AreEqual(0x00, message.Header.Token);
        }

        [TestMethod]
        public void TestFromBytes_Header_StationNo()
        {
            var message = new Message();
            //8E 09 01 00 12 01 00 60 00 00 01 27 6F 00 04 06 02 00 32 8E
            var bytes = new byte[] { 0x8E, 0x09, 0x01, 0x00, 0x12, 0x01, 0x00, 0x60, 0x00, 0x00, 0x01, 0x27, 0x6F, 0x00, 0x04, 0x06, 0x02, 0x00, 0x32, 0x8E };
            message.FromBytes(bytes);

            Assert.AreEqual("60000001", message.Header.StationNo, true);
        }

        [TestMethod]
        public void TestFromBytes_Header_SequenceNO()
        {
            var message = new Message();
            //8E 09 01 00 12 01 00 60 00 00 01 27 6F 00 04 06 02 00 32 8E
            var bytes = new byte[] { 0x8E, 0x09, 0x01, 0x00, 0x12, 0x01, 0x00, 0x60, 0x00, 0x00, 0x01, 0x27, 0x6F, 0x00, 0x04, 0x06, 0x02, 0x00, 0x32, 0x8E };
            message.FromBytes(bytes);

            Assert.AreEqual(0x276F, message.Header.SequenceNO);
        }

        #endregion
    }
}
