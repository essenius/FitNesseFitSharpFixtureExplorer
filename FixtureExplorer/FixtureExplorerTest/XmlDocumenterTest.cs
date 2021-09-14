// Copyright 2016-2020 Rik Essenius
//
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
//   except in compliance with the License. You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software distributed under the License 
//   is distributed on an "AS IS" BASIS WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and limitations under the License.

using FixtureExplorer.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#pragma warning disable 1591 // We're missing XML comments on purpose

namespace FixtureExplorerTest
{
    [TestClass]
    [DeploymentItem("FixtureExplorerTest.xml")]

    public class XmlDocumenterTest
    {
        [TestMethod]
        public void TypeDocumentationTest()
        {
            var classType = typeof(XmlDocumenterTestClass);
            var doc = new XmlDocumenter(classType);
            Assert.AreEqual("XmlDocumenterTestClass()", doc.ConstructorDocumentation(classType.GetConstructors()[0]));
            var echoMethod = classType.GetMethod("Echo");
            Assert.AreEqual("Echo(object). Remarks: Not very interesting. See Method 1. Requires: nothing. Returns: itself",
                doc.MethodBaseDocumentation(echoMethod));
            var method1 = classType.GetMethod("Method1");
            Assert.AreEqual("Returns: the length of the string representation of Field1. See Echo", doc.MethodBaseDocumentation(method1));
            var property = classType.GetMethod("get_ArrayProperty");
            Assert.AreEqual("Remarks: ArrayProperty. Guarantees: nothing", doc.MethodBaseDocumentation(property));
            var constructor = classType.GetConstructor(new[] { typeof(double?) });
            Assert.AreEqual("XmlDocumenterTestClass(double?). Params: { input: nullable double }", doc.MethodBaseDocumentation(constructor));
        }
    }
}
