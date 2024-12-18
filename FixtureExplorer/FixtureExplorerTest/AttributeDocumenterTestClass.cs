// Copyright 2016-2024 Rik Essenius
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
using FixtureExplorer;

#pragma warning disable 1591 // We're missing XML comments on purpose

namespace FixtureExplorerTest
{
    [Documentation("Class documentation")]
    public class AttributeDocumenterTestClass
    {
        public AttributeDocumenterTestClass(int input) => Property1 = input;

        public int NoDocProperty { get; set; }

        [Documentation("Property1 Documentation")]
        public int Property1 { get; }

        [Obsolete]
        [Documentation(null)]
        public void NullDocMethod()
        {
        }

        [Documentation("TestMethod1 Documentation")]
        public string TestMethod1() => string.Empty;

        [Obsolete("use TestMethod1 instead")]
        public string TestMethod2() => string.Empty;
    }
}
