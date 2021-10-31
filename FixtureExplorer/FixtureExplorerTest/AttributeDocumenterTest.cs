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

using System;
using System.Reflection;
using FixtureExplorer.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#pragma warning disable 1591 // We're missing XML comments on purpose

namespace FixtureExplorerTest
{
    [Obsolete("use DocTest instead")]
    public class ObsoleteClass
    {
        [Obsolete("Overriding obsolete message")]
        public string Method1() => string.Empty;

        public string Method2() => string.Empty;
    }

    [TestClass]
    public class AttributeDocumenterTest
    {
        private const BindingFlags RelevantBindings =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        [TestMethod]
        [Obsolete("Using obsolete classes")]
        public void AttributeDocumenterDeprecationTest()
        {
            var doctest = typeof(AttributeDocumenterTestClass);
            var documenter = new AttributeDocumenter(doctest, RelevantBindings);
            Assert.AreEqual(string.Empty, documenter.TypeDeprecationMessage);
            Assert.AreEqual(string.Empty, documenter.MethodBaseDeprecationMessage(doctest.GetMethod("TestMethod1")));
            Assert.AreEqual(
                "[Deprecated: use TestMethod1 instead]",
                documenter.MethodBaseDeprecationMessage(doctest.GetMethod("TestMethod2"))
                );
            Assert.AreEqual(
                "[Deprecated]",
                documenter.MethodBaseDeprecationMessage(doctest.GetMethod("NullDocMethod"))
                );

            var obsoleteClass = typeof(ObsoleteClass);
            var documenter2 = new AttributeDocumenter(obsoleteClass, RelevantBindings);
            Assert.AreEqual("[Deprecated class: use DocTest instead]", documenter2.TypeDeprecationMessage);
            Assert.AreEqual(
                "[Deprecated: Overriding obsolete message]",
                documenter2.MethodBaseDeprecationMessage(obsoleteClass.GetMethod("Method1"))
                );
            Assert.AreEqual(string.Empty, documenter2.MethodBaseDeprecationMessage(obsoleteClass.GetMethod("Method2")));
        }

        [TestMethod]
        public void AttributeDocumenterDocumentationTest()
        {
            var doctest = typeof(AttributeDocumenterTestClass);
            var documenter = new AttributeDocumenter(doctest, RelevantBindings);
            Assert.AreEqual("Class documentation", documenter.ConstructorDocumentation(doctest.GetConstructors()[0]));
            Assert.AreEqual(
                "TestMethod1 Documentation",
                documenter.MethodBaseDocumentation(doctest.GetMethod("TestMethod1")));
            Assert.AreEqual(
                "Property1 Documentation",
                documenter.MethodBaseDocumentation(doctest.GetMethod("get_Property1")));
            Assert.IsTrue(
                string.IsNullOrEmpty(documenter.MethodBaseDocumentation(doctest.GetMethod("set_NoDocProperty")))
                );
            Assert.IsTrue(string.IsNullOrEmpty(documenter.MethodBaseDocumentation(doctest.GetMethod("NullDocMethod"))));
            // The only path we're not testing is if the DocumentationProperty doesn't contain a Message property,
            // which would be a mistake in the definition, so very unlikely in production. It would result in null, so no harm.
        }
    }
}
