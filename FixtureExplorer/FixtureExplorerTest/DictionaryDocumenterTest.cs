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
    public class DictionaryDocumenterTest
    {
        [TestMethod]
        public void DictionaryDocumenterTest1()
        {
            var doctest = typeof(DictionaryDocumenterTestClass);
            var documenter = new DictionaryDocumenter(doctest);
            Assert.AreEqual("Class documentation", documenter.ConstructorDocumentation(doctest.GetConstructors()[0]));
            Assert.AreEqual("Test Method 1", documenter.MethodBaseDocumentation(doctest.GetMethod("TestMethod1")));
            Assert.AreEqual("Test Method 2", documenter.MethodBaseDocumentation(doctest.GetMethod("TestMethod2")));
            Assert.AreEqual("Property 1", documenter.MethodBaseDocumentation(doctest.GetMethod("get_Property1")));
            Assert.AreEqual("Property 1", documenter.MethodBaseDocumentation(doctest.GetMethod("set_Property1")));
            Assert.AreEqual("Constructor 1", documenter.MethodBaseDocumentation(doctest.GetConstructor(new[] {typeof(int)})));

            Assert.AreEqual(string.Empty, documenter.MethodBaseDocumentation(doctest.GetMethod("get_NoDocProperty")));
        }
    }
}
