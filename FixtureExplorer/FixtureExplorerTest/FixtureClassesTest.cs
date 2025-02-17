// Copyright 2016-2025 Rik Essenius
//
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
//   except in compliance with the License. You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software distributed under the License 
//   is distributed on an "AS IS" BASIS WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and limitations under the License.

using FixtureExplorer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestAssemblyWithDocumentationAttribute;

#pragma warning disable 1591 // We're missing XML comments on purpose

namespace FixtureExplorerTest
{
    [TestClass]
    public class FixtureClassesTest
    {
        [TestMethod]
        public void FixtureClassesDoTableTest()
        {
            var expected = new List<string>
            {
                "|report:Namespace|report:Class|report:Parameters|report:Supports Table Type|report:Documentation|",
                "|report:Test Assembly With Documentation Attribute|report:Class Not Supporting Decision Table|report:|report:Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Deprecated Class|report:|report:Decision, Script|report:[Deprecated class: Use Public Class instead]A deprecated class|",
                "|report:Test Assembly With Documentation Attribute|report:Deprecated Class|report:parameter: String[,]|report:Decision, Script|report:[Deprecated class: Use Public Class instead]Documentation for constructor with one parameter. Params: { parameter: documentation for the parameter }. Documentation attribute for constructor with 1 parameter|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:input: Int32|report:Decision, Query, Script|report:Just a demo public class constructor with one parameter. Documentation attribute for public class constructor with one parameter|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:input1: Int32, input2: Nullable<Int32>|report:Decision, Query, Script|report:Just a demo public class constructor with two parameters. Params: { input1: input 1 doc; input2: input 2 doc }. Documentation attribute for public class constructor with two parameters|",
                "|report:Test Assembly With Documentation Attribute|report:Wrong Table Class|report:|report:Decision, Script|report:WrongTableClass is a fixture with a wrong Table or Query Signature, so should not be recognized as table. Class with a wrong Table/Query signature, so only supports Script and Decision|"
            };
            var location = Assembly.GetAssembly(typeof(PublicClass)).Location;
            // forcing use of the abstract DoTable function, which should delegate to the subclass
            TableTypeFixture fixture = new FixtureClasses(location);
            var result = fixture.DoTable(null);

            var expectedIndex = 0;
            foreach (List<string> row in result)
            {
                var line = expected[expectedIndex];
                var rowString = row.Aggregate("|", (current, cell) => current + cell + "|");
                Assert.AreEqual(line, rowString, $"Row {expectedIndex}");
                expectedIndex++;
            }
        }

        [TestMethod]
        public void FixtureClassesSupportedTableTest1()
        {
            var list = FixtureClasses.SupportedTables(typeof(PublicClass));
            var expectedList = new List<string> {"Decision", "Query", "Script"};
            Assert.IsTrue(expectedList.SequenceEqual(list));
            Assert.IsFalse(FixtureClasses.SupportedTables(typeof(WrongTableClass)).Contains("Query"));
            Assert.IsFalse(FixtureClasses.SupportedTables(typeof(TableTypeFixture)).Contains("Query"));
        }

        [TestMethod]
        public void FixtureClassesSupportedTableTest2()
        {
            var list = FixtureClasses.SupportedTables(typeof(WrongTableClass));
            var expectedList = new List<string> {"Decision", "Script"};
            Assert.IsTrue(expectedList.SequenceEqual(list));
            Assert.IsFalse(FixtureClasses.SupportedTables(typeof(TableTypeFixture)).Contains("Query"));
        }

        [TestMethod]
        public void FixtureClassesSupportedTableTest3()
        {
            var list = FixtureClasses.SupportedTables(typeof(TableTypeFixture));
            var expectedList = new List<string> {"Script", "Table"};
            Assert.IsTrue(expectedList.SequenceEqual(list));
        }

    }
}
