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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FixtureExplorer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAssembly;

namespace FixtureExplorerTest
{
    [TestClass]
    public class FixtureFunctionsTest
    {
        [TestMethod, DeploymentItem("TestAssemblyFunctions.txt"), DeploymentItem("TestAssembly.xml")]
        public void FixtureFunctionsDoTableTest()
        {
            var location = Assembly.GetAssembly(typeof(PublicClass)).Location;

            var fixture = new FixtureFunctions(location);
            var result = fixture.DoTable(null);

            using var expectedFile = new StreamReader("TestAssemblyFunctions.txt");
            foreach (List<string> row in result)
            {
                var line = expectedFile.ReadLine();
                var rowString = row.Aggregate("|", (current, cell) => current + cell + "|");
                Assert.AreEqual(line, rowString);
            }
        }

    }
}
