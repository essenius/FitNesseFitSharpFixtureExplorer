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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FixtureExplorer.Helpers;

namespace FixtureExplorer
{
    /// <summary>FitSharp fixture to show the valid methods that fixture supports</summary>
    public class FixtureFunctions : TableTypeFixture
    {
        /// <summary>Initialization with an assembly name</summary>
        public FixtureFunctions(string assemblyName) : base(assemblyName)
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
                Report("Scope"),
                Report("Fixture Name"),
                Report("Method Type"),
                Report("Return Type"),
                Report("Parameters"),
                Report("Supports Table Type"),
                Report("Documentation")
            }
        };

        /// <summary>Add a documentation row of all constructors of the given type to the result list</summary>
        /// <remarks>Part of the Template pattern for DoTable</remarks>
        protected override void AddToList(List<object> result, Type type)
        {
            foreach (var methodInfo in MethodBaseDocumenter.RelevantMethods(type).OrderBy(method => RealName(method.Name)))
            {
                var methodHelper = new FixtureDocumenter(methodInfo);
                var deprecationMessage = methodHelper.DeprecationMessage;
                var documentation = methodHelper.MethodBaseDocumentation;
                documentation = string.Empty + deprecationMessage + documentation;
                var scope = methodHelper.Scope;
                if (string.IsNullOrEmpty(documentation) && scope.Contains("internal"))
                {
                    documentation = "[Internal use only. Do not use in tests]";
                }
                result.Add(Row(type, scope, methodInfo, methodHelper, documentation));
            }
        }

        /// <summary>The real name (i.e. without get_ or set) of a member</summary>
        private static string RealName(string reflectedName) => new GracefulNamer(reflectedName).RealName;

        /// <summary>A row with a function specification</summary>
        private static List<string> Row(Type type, IEnumerable<string> scope, MethodInfo methodInfo, FixtureDocumenter method, string documentation)
        {
            var namer = new GracefulNamer(methodInfo.Name);
            return new List<string>
            {
                Report(new GracefulNamer(type.Namespace).Regrace),
                Report(new GracefulNamer(type.Name).Regrace),
                Report(string.Join(" ", scope)),
                Report(namer.Regrace),
                Report(namer.Type),
                Report(GracefulNamer.GracefulName(methodInfo.ReturnType)),
                Report(string.Join(", ", method.Parameters)),
                Report(string.Join(", ", method.TablesSupported)),
                Report(documentation)
            };
        }
    }
}