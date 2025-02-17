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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FixtureExplorer.Helpers;

namespace FixtureExplorer
{
    /// <summary>Fixture to show information about the classes in an assembly</summary>
    public class FixtureClasses : TableTypeFixture
    {
        /// <summary>Initialize the fixture with an assembly name</summary>
        public FixtureClasses(string assemblyName) : base(assemblyName)
        {
        }

        /// <summary>Return a new result list with a header row</summary>
        /// <remarks>Part of the Template pattern for DoTable</remarks>
        protected override List<object> ListWithHeaderRow => new List<object>
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

        /// <summary>Add a documentation row of all constructors of the given type to the result list</summary>
        /// <remarks>Part of the Template pattern for DoTable</remarks>
        protected override void AddToList(List<object> result, Type type)
        {
            Debug.Assert(result != null, "result != null");
            foreach (var constructor in type.GetConstructors())
            {
                var documenter = new MethodBaseDocumenter(constructor);
                var resultEntry = documenter.Parameters;
                var deprecatedMessage = documenter.DeprecationMessage;
                var documentation = documenter.ConstructorDocumentation;
                documentation = string.Empty + deprecatedMessage + documentation;
                result.Add(Row(type.Namespace, type.Name, resultEntry, SupportedTables(type), documentation));
            }
        }

        /// <summary>A row of constructor documentation elements</summary>
        private List<string> Row(
            string nameSpace, string className, IEnumerable<string> parameters, IEnumerable<string> supports, string documentation) =>
            new List<string>
            {
                Report(new GracefulNamer(nameSpace).Regrace),
                Report(new GracefulNamer(className).Regrace),
                Report(string.Join(", ", parameters)),
                Report(string.Join(", ", supports)),
                Report(documentation)
            };

        /// <param name="type">the fixture type to inspect</param>
        /// <returns>a list of FitNesse tables that the type supports</returns>
        internal static IList<string> SupportedTables(Type type)
        {
            var tables = new List<string>();
            tables = MethodBaseDocumenter.RelevantMethods(type)
                .Select(methodInfo => new FixtureDocumenter(methodInfo))
                .Aggregate(tables, (current, method) => current.Union(method.TablesSupported).ToList());

            // no sense reporting the presence of optional methods here. if the mandatory ones are there, we report it, and if not we don't.
            tables.Remove("Decision-Optional");
            tables.Remove("Query-Optional");
            tables.Sort();
            return tables;
        }
    }
}