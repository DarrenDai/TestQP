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
    public class TestClientLogonBody
    {
        [TestMethod]
        public void TestGetBodyBytes()
        {
            var body = new ClientLogonBody()
            {
                Password = "123456",
                RuntimeVersion = "Android 5.1.1",
                FrontEndVersion = "1.0.0"
            };

            var bodyBytes = body.GetBodyBytes();
            Assert.AreEqual("31-32-33-34-35-36-00-41-6E-64-72-6F-69-64-20-35-2E-31-2E-31-00-31-2E-30-2E-30-00", BitConverter.ToString(bodyBytes), true);
        }

        [TestMethod]
        public void TestFromBytes_Password()
        {
            var bytes = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x00, 0x41, 0x6E, 0x64, 0x72, 0x6F, 0x69, 0x64, 0x20, 0x35, 0x2E, 0x31, 0x2E, 0x31, 0x00, 0x31, 0x2E, 0x30, 0x2E, 0x30, 0x00 };
            var body = new ClientLogonBody();
            body.FromBytes(bytes);

            Assert.AreEqual("123456", body.Password, false);
        }

        [TestMethod]
        public void TestFromBytes_RuntimeVersion()
        {
            var bytes = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x00, 0x41, 0x6E, 0x64, 0x72, 0x6F, 0x69, 0x64, 0x20, 0x35, 0x2E, 0x31, 0x2E, 0x31, 0x00, 0x31, 0x2E, 0x30, 0x2E, 0x30, 0x00 };
            var body = new ClientLogonBody();
            body.FromBytes(bytes);

            Assert.AreEqual("Android 5.1.1", body.RuntimeVersion, false);
        }

        [TestMethod]
        public void TestFromBytes_FrontEndVersion()
        {
            var bytes = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x00, 0x41, 0x6E, 0x64, 0x72, 0x6F, 0x69, 0x64, 0x20, 0x35, 0x2E, 0x31, 0x2E, 0x31, 0x00, 0x31, 0x2E, 0x30, 0x2E, 0x30, 0x00 };
            var body = new ClientLogonBody();
            body.FromBytes(bytes);

            Assert.AreEqual("1.0.0", body.FrontEndVersion, false);
        }
    }
}
