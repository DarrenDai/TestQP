using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Sockets;

namespace TestQP.Tests
{
    [TestClass]
    public class MessageBodyTests
    {
        [TestMethod]
        public void TestMessageBody()
        {
            var msgBody = new MessageBody();
            var bytes = msgBody.GetMessageBody();
            var data = BitConverter.ToString(bytes).Replace("-", " ");
            Assert.AreEqual("46 72 65 65 52 54 4F 53 20 56 38 2E 32 2E 33 00 00 00 32 2E 30 2E 30 37", data, true);
        }
    }
}
