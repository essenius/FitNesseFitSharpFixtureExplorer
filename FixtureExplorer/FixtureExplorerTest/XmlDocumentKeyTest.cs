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

using System;
using FixtureExplorer.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FixtureExplorerTest
{
    [TestClass, DeploymentItem("FixtureExplorerTest.xml")]
    public class XmlDocumentKeyTest
    {
        [TestMethod]
        public void XmlDocumentKeyMethodBaseKeyNestedClassTest()
        {
            var testClass = typeof(XmlDocumenterTestClass.NestedClass);
            var voidConstructor = testClass.GetConstructor(Array.Empty<Type>());
            Assert.AreEqual("M:FixtureExplorerTest.XmlDocumenterTestClass.NestedClass.#ctor", XmlDocumentKey.MethodBaseKey(voidConstructor));
        }

        [TestMethod]
        public void XmlDocumentKeyMethodBaseKeyNullMethodTest()
        {
            Assert.IsNull(XmlDocumentKey.MethodBaseKey(null));
        }

        [TestMethod]
        public void XmlDocumentKeyMethodBaseKeyPointerTest()
        {
            var c = typeof(XmlDocumenterTestClassUnsafe);
            var methodWithPointerParam = c.GetMethod("UnsafeMethod");
            Assert.AreEqual("M:FixtureExplorerTest.XmlDocumenterTestClassUnsafe.UnsafeMethod(System.Int32*)",
                XmlDocumentKey.MethodBaseKey(methodWithPointerParam));
        }

        [TestMethod]
        public void XmlDocumentKeyMethodBaseKeyPropertyTest()
        {
            var c = typeof(XmlDocumenterTestClass);
            var setArrayProperty = c.GetMethod("set_ArrayProperty");
            Assert.AreEqual("P:FixtureExplorerTest.XmlDocumenterTestClass.ArrayProperty",
                XmlDocumentKey.MethodBaseKey(setArrayProperty));
            var getArrayProperty = c.GetMethod("get_ArrayProperty");
            Assert.AreEqual("P:FixtureExplorerTest.XmlDocumenterTestClass.ArrayProperty",
                XmlDocumentKey.MethodBaseKey(getArrayProperty));
        }

        [TestMethod]
        public void XmlDocumentKeyMethodBaseKeyTest()
        {
            var c = typeof(XmlDocumenterTestClass);
            var voidConstructor = c.GetConstructor(Array.Empty<Type>());
            Assert.AreEqual("M:FixtureExplorerTest.XmlDocumenterTestClass.#ctor", XmlDocumentKey.MethodBaseKey(voidConstructor));
            var nullableDoubleConstructor = c.GetConstructor(new[] {typeof(double?)});
            Assert.AreEqual("M:FixtureExplorerTest.XmlDocumenterTestClass.#ctor(System.Nullable{System.Double})",
                XmlDocumentKey.MethodBaseKey(nullableDoubleConstructor));
            var echoMethod = c.GetMethod("Echo");
            Assert.AreEqual("M:FixtureExplorerTest.XmlDocumenterTestClass.Echo(System.Object@)",
                XmlDocumentKey.MethodBaseKey(echoMethod));
        }

        [TestMethod]
        public void XmlDocumentKeyTypeKeyTest()
        {
            Assert.AreEqual("T:FixtureExplorerTest.XmlDocumenterTestClass", XmlDocumentKey.TypeKey(typeof(XmlDocumenterTestClass)));
        }

        [TestMethod]
        public void XmlDocumentKeyTypeSpecTest()
        {
            Assert.AreEqual("System.String", XmlDocumentKey.TypeSpec(typeof(string), true));
            Assert.AreEqual("System.Nullable{System.Int32}", XmlDocumentKey.TypeSpec(typeof(int?), true));
            // strange one. The documentation key reverses the dimensions, but that's per the specs.
            Assert.AreEqual("System.Int32[][0:,0:]", XmlDocumentKey.TypeSpec(typeof(int[,][]), true));
            Assert.AreEqual("FixtureExplorerTest.XmlDocumenterTestClass.NestedClass",
                XmlDocumentKey.TypeSpec(typeof(XmlDocumenterTestClass.NestedClass), false));
        }
    }
}
