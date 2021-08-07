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

using FixtureExplorer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TestAssembly;

namespace FixtureExplorerTest
{
    [TestClass]
    public class FixtureClassesTest
    {
        [TestMethod, DeploymentItem("TestAssemblyClasses.txt"), DeploymentItem("TestAssembly.xml")]
        public void FixtureClassesDoTableTest()
        {
            var publicClass = new PublicClass(5);
            var location = Assembly.GetAssembly(typeof(PublicClass)).Location;
            // forcing use of the abstract DoTable function, which should delegate to the subclass
            TableTypeFixture fixture = new FixtureClasses(location);
            var result = fixture.DoTable(null);

            using var expectedFile = new StreamReader("TestAssemblyClasses.txt");
            foreach (List<string> row in result)
            {
                var line = expectedFile.ReadLine();
                var rowString = row.Aggregate("|", (current, cell) => current + cell + "|");
                Assert.AreEqual(line, rowString);
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
