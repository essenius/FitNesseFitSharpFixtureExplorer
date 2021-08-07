using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAssembly;
using FixtureExplorer.Helpers;

namespace FixtureExplorerTest
{
    [TestClass]
    public class MethodBaseDocumenterTest
    {
        [TestMethod, DeploymentItem("TestAssembly.xml")]
        public void MethodBaseDocumenterTestTest1()
        {
            var c = typeof(WrongTableClass);
            var constructor = c.GetConstructors()[0];
            var documenter = new MethodBaseDocumenter(constructor);
            Assert.AreEqual("WrongTableClass is a fixture with a wrong Table or Query Signature, so should not be recognized as table. Class with a wrong Table/Query signature, so only supports Script and Decision", 
                documenter.ConstructorDocumentation);
        }
    }
}
