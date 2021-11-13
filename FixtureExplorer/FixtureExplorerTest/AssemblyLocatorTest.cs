using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FixtureExplorer;
using FixtureExplorer.Helpers;

#pragma warning disable 1591 // documentation not needed

namespace FixtureExplorerTest
{
    [TestClass]
    public class AssemblyLocatorTest
    {
        [TestMethod]
        public void TestAssemblyFoundInCurrentFolder()
        {
            var locator = new AssemblyLocator("FixtureExplorer.dll", ".");
            Assert.AreEqual("FixtureExplorer.dll", locator.FindAssemblyPath());
        }

        [TestMethod]
        [DeploymentItem("bogus.xml")] // only needed for .NET Framework
        public void TestAssemblyNotFound()
        {
            var locator = new AssemblyLocator("bogus.dll", ".");
            Assert.IsNull(locator.FindAssemblyPath());
        }

        [TestMethod]
        [DeploymentItem("TestConfig.xml")] // only needed for .NET Framework
        public void TestAssemblySearchXml()
        {
            var locator = new AssemblyLocator("MyTest2.dll", ".");
            Assert.AreEqual("MyTest2/MyTest2.dll", locator.FindAssemblyPath());
        }

    }
}
