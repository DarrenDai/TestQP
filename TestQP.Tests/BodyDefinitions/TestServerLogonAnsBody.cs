using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Sockets.BodyDefinitions;

namespace TestQP.Tests.BodyDefinitions
{
    [TestClass]
    public class TestServerLogonAnsBody
    {
        [TestMethod]
        public void TestBody_Status()
        {
            var body = new ServerLogonAnsBody();
            var bytes = new byte[] { 0x00, 0xA0, 0x18, 0x02, 0x04, 0x00, 0x19, 0x56, 0x09 };
            body.FromBytes(bytes);

            Assert.IsTrue(body.Status == 0);
        }

        [TestMethod]
        public void TestBody_Token()
        {
            var body = new ServerLogonAnsBody();
            var bytes = new byte[] { 0x00, 0xA0, 0x18, 0x02, 0x04, 0x00, 0x19, 0x56, 0x09 };
            body.FromBytes(bytes);

            Assert.IsTrue(body.Token == 0xA0);
        }

        [TestMethod]
        public void TestBody_DateTime()
        {
            var body = new ServerLogonAnsBody();
            var bytes = new byte[] { 0x00, 0xA0, 0x18, 0x02, 0x04, 0x00, 0x19, 0x56, 0x09 };
            body.FromBytes(bytes);

            Assert.IsTrue(body.Time == new DateTime(2018, 2, 4, 19, 56, 9));
        }

        [TestMethod]
        public void TestConstructor()
        {
            var body = new ServerLogonAnsBody() { Status = 0, Token = 0xA0, Time = new DateTime(2018, 2, 4, 19, 56, 9) };

            var bytes = body.GetBodyBytes();
            Assert.AreEqual("00-A0-18-02-04-00-19-56-09", BitConverter.ToString(bytes), true);
        }
    }
}
