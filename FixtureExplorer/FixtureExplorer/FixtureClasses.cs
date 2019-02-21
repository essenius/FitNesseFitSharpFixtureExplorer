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

using System;
using System.Collections.Generic;
using System.Linq;
using FixtureExplorer.Helpers;

namespace FixtureExplorer
{
    public class FixtureClasses : TableTypeFixture
    {
        public FixtureClasses(string assemblyName) : base(assemblyName)
        {
        }

        private static List<object> ListWithHeaderRow => new List<object>
        {
            new List<string>
            {
                Report("Namespace"),
                Report("Class"),
                Report("Parameters"),
                Report("Supports Table Type"),
                Report("Documentation")
            }
        };

        public override List<object> DoTable(List<List<string>> table)
        {
            var returnList = ListWithHeaderRow;

            foreach (var type in ClassesVisibleToFitNesse.OrderBy(type => type.Name))
            {
                foreach (var constructor in type.GetConstructors())
                {
                    var helper = new MethodBaseHelper(constructor, type);
                    var resultEntry = helper.Parameters;
                    var deprecatedMessage = helper.DeprecationMessage();
                    var documentation = helper.Documentation() ?? type.Documentation();
                    if (string.IsNullOrEmpty(documentation)) documentation = type.DocumentationFor("`" + resultEntry.Count);
                    documentation = string.Empty + deprecatedMessage + documentation;
                    returnList.Add(Row(type.Namespace, type.Name, resultEntry, SupportedTables(type), documentation));
                }
            }
            return returnList;
        }

        private static List<string> Row(
            string nameSpace, string className, IEnumerable<string> parameters, IEnumerable<string> supports, string documentation) =>
            new List<string>
            {
                Report(new GracefulNamer(nameSpace).Regrace),
                Report(new GracefulNamer(className).Regrace),
                Report(string.Join(", ", parameters)),
                Report(string.Join(", ", supports)),
                Report(documentation)
            };

        public static IList<string> SupportedTables(Type type)
        {
            var tables = new List<string>();
            foreach (var methodInfo in MethodHelper.RelevantMethods(type))
            {
                var method = new MethodHelper(methodInfo, type);
                tables = tables.Union(method.TablesSupported).ToList();
                Console.WriteLine($"{methodInfo.Name} => {string.Join(",", tables)}");
            }
            // no sense reporting the presence of optional methods here. if the mandatory ones are there, we report it, and if not we don't.
            tables.Remove("Decision-Optional");
            tables.Remove("Query-Optional");
            tables.Sort();
            return tables;
        }
    }
}