// Copyright 2016-2021 Rik Essenius
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
using TestAssemblyWithDocumentationAttribute;

#pragma warning disable 1591 // We're missing XML comments on purpose

namespace FixtureExplorerTest
{
    [TestClass]
    public class MethodBaseDocumenterTest
    {
        [TestMethod]
        /* [DeploymentItem("TestAssembly.xml")] -- once is enough */
        public void MethodBaseDocumenterTestTest1()
        {
            var c = typeof(WrongTableClass);
            var constructor = c.GetConstructors()[0];
            var documenter = new MethodBaseDocumenter(constructor);
            Assert.AreEqual(
                "WrongTableClass is a fixture with a wrong Table or Query Signature, so should not be recognized as table. Class with a wrong Table/Query signature, so only supports Script and Decision",
                documenter.ConstructorDocumentation);
        }
    }
}
