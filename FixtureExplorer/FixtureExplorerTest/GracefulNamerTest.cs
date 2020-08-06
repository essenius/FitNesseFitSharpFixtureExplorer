// Copyright 2016-2019 Rik Essenius
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

namespace FixtureExplorerTest
{
    [TestClass]
    public class GracefulNamerTest
    {
        [TestMethod]
        public void GracefulNamerDisgraceTest()
        {
            Assert.AreEqual("TestTable", new GracefulNamer("test table").Disgrace);
            Assert.AreEqual("Test123TAble", new GracefulNamer("test 123 <> tAble").Disgrace);
            Assert.AreEqual("Test123Table", new GracefulNamer("test123 table").Disgrace);
        }

        [TestMethod]
        public void GracefulNamerRegraceTest()
        {
            Assert.AreEqual("Test Table", new GracefulNamer("TestTable").Regrace);
            Assert.AreEqual("Test 123 Table", new GracefulNamer("Test123Table").Regrace);
            Assert.AreEqual("Assembly.Test 123 Table", new GracefulNamer("Assembly.Test123Table").Regrace);
        }
    }
}
