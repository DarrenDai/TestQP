using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestQP.Utils;

namespace TestQP.Tests.Utils
{
    [TestClass]
    public class RestfulHelperTests
    {
        [TestMethod]
        public void TestGetMessage()
        {
            new RestfulHelper().GetStationInfo();
        }
    }
}
