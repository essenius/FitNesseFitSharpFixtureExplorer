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

namespace FixtureExplorerTest
{
    internal class DictionaryDocumenterTestClass
    {
        public DictionaryDocumenterTestClass() { }
        public DictionaryDocumenterTestClass(int input) => NoDocProperty = input;

        public static Dictionary<string, string> FixtureDocumentation { get; } = new Dictionary<string, string>
        {
            {string.Empty, "Class documentation"},
            {"TestMethod1`0", "Test Method 1"},
            {"TestMethod2", "Test Method 2"},
            {"Property1", "Property 1"},
            {"`1", "Constructor 1"}
        };

        public int NoDocProperty { get; }

        public int Property1 { get; set; }

        public string TestMethod1() => string.Empty;

        public string TestMethod2() => string.Empty;
    }
}
