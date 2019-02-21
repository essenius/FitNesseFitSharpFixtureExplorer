using FixtureExplorer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FixtureExplorerTest
{
    [TestClass]
    public class DocumentationAttributeTest
    {
        [TestMethod]
        public void DocumentationAttributeTest1()
        {
            Assert.AreEqual("test", new DocumentationAttribute("test").Message);
        }
    }
}