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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable 1591 // We're missing XML comments on purpose
#pragma warning disable CA1822 // Mark members as static - intentional instance, for testing

namespace TestAssemblyWithDocumentationAttribute
{
    /// <summary>
    ///     WrongTableClass is a fixture with a wrong Table or Query Signature, so should not be recognized as table
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WrongTableClass
    {
        public static Dictionary<string, string> FixtureDocumentation { get; } = new Dictionary<string, string>
        {
            {string.Empty, "Class with a wrong Table/Query signature, so only supports Script and Decision"},
            {"BeginTable`0", "Executed just before a Decision table"},
            {"FixtureDocumentation", "Test data for DocumentationFor function"},
            {"Property", "Property for testing FixtureFor"},
            {"Query", "Doesn't really do anything"}
        };

        public int Property { get; set; }

        public int PropertyWithoutDocumentation { get; } = 1;

        public void BeginTable()
        {
        }

        public List<object> DoTable(List<object> table) => table;

        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "FitNesse API")]
        public void Execute(string command)
        {
        }

        public List<object> Query(int input) => new List<object> {input};

        public int Reset() => 0;

        public List<object> Table(int a, int b) => new List<object> {a, b};
    }
}
