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
using System.Reflection;
using FixtureExplorer.Helpers;

namespace FixtureExplorer
{
    public class FixtureFunctions : TableTypeFixture
    {
        public FixtureFunctions(string assemblyName) : base(assemblyName)
        {
        }

        private static List<object> ListWithHeaderRow => new List<object>
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

        /// <summary>
        ///     Table Table interface - returns all properties methods from all classes that FitNesse can see
        ///     This shows which methods can be used in script tables.
        /// </summary>
        /// <param name="table">ignored - part of the FitNesse TableTable contract</param>
        /// <returns>List of methods/properties in the FitNesse Table Table format</returns>
        public override List<object> DoTable(List<List<string>> table)
        {
            var returnList = ListWithHeaderRow;

            foreach (var type in ClassesVisibleToFitNesse.OrderBy(type => type.Name))
            {
                foreach (var methodInfo in MethodHelper.RelevantMethods(type).OrderBy(method => RealName(method.Name)))
                {
                    var methodHelper = new MethodHelper(methodInfo, type);
                    var deprecationMessage = methodHelper.DeprecationMessage();
                    var documentation = methodHelper.Documentation();
                    documentation = string.Empty + deprecationMessage + documentation;
                    var scope = methodHelper.Scope;
                    if (string.IsNullOrEmpty(documentation) && (scope.Contains("internal") || scope.Contains("private")))
                    {
                        documentation = "Internal use only. Do not use in tests.";
                    }
                    returnList.Add(Row(type, scope, methodInfo, methodHelper, documentation));
                }
            }
            return returnList;
        }

        private static string RealName(string reflectedName)
        {
            if (reflectedName.StartsWith("get_") || reflectedName.StartsWith("set_")) return reflectedName.Substring(4);
            return reflectedName;
        }

        private static List<string> Row(Type type, IEnumerable<string> scope, MethodInfo methodInfo, MethodHelper method, string documentation)
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