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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FixtureExplorer.Helpers
{
    internal class MethodHelper : MethodBaseHelper
    {
        public const string FixtureDocumentationProperty = "FixtureDocumentation";

        private static readonly List<string> DecisionTableVoidMethods = new List<string>
        {
            "BeginTable",
            "EndTable",
            "Reset",
            "Execute"
        };

        // We hide some methods inherited from Object - those are always there and generally not useful to mention -
        // and the static Documentation property that we defined in previous versions of the fixtures to provide more input about the functions.
        private static readonly List<string> MethodsToSkip = new List<string>
        {
            "get_" + FixtureDocumentationProperty,
            "Equals",
            "Finalize",
            "GetHashCode",
            "GetType",
            "MemberwiseClone",
            "ToString"
        };

        public MethodHelper(MethodInfo methodInfo, Type parentType) : base(methodInfo, parentType) => Info = methodInfo;

        private bool HasPropertySignature =>
            Info.ReturnType == typeof(void) && Info.GetParameters().Length == 1 ||
            Info.ReturnType != typeof(void) && Info.GetParameters().Length == 0 && !HasQuerySignature;

        private bool HasQuerySignature => typeof(IList).IsAssignableFrom(Info.ReturnType)
                                          && Info.GetParameters().Length == 0;

        private bool HasTableSignature
        {
            get
            {
                // Table fixtures must have a list as return value, and a list as single parameter.
                var isCandidate =
                    typeof(IList).IsAssignableFrom(Info.ReturnType) &&
                    Info.GetParameters().Length == 1 &&
                    typeof(IList).IsAssignableFrom(Info.GetParameters()[0].ParameterType);
                if (!isCandidate) return false;
                // Now we check if the parameter is a list of lists
                var parameterType = Info.GetParameters()[0].ParameterType;
                var genericType = parameterType.GenericTypeArguments[0];
                return typeof(IList).IsAssignableFrom(genericType);
            }
        }

        private bool HasVoidSignature => Info.ReturnType == typeof(void) && Info.GetParameters().Length == 0;
        private MethodInfo Info { get; }

        public List<string> Scope
        {
            get
            {
                var scope = new List<string>();
                if (Info.IsPrivate)
                {
                    scope.Add("private");
                }
                else if (Info.IsPublic)
                {
                    scope.Add("public");
                }
                else
                {
                    scope.Add("internal");
                }
                if (Info.IsStatic) scope.Add("static");
                return scope;
            }
        }

        private bool SupportsDecisionTable => HasPropertySignature;

        // Decision tables can have an optional Table method executed just after the constructor,
        // passing  a list of lists containing all cells of the table except the very first row
        private bool SupportsDecisionTableOptional => 
            DecisionTableVoidMethods.Contains(Info.Name) && HasVoidSignature || Info.Name == "Table" && HasTableSignature;

        private bool SupportsQueryTable => Info.Name == "Query" && HasQuerySignature;
        private bool SupportsQueryTableOptional => Info.Name == "Table" && HasTableSignature;
        private bool SupportsTableTable => Info.Name == "DoTable" && HasTableSignature;

        public IEnumerable<string> TablesSupported
        {
            get
            {
                var supportList = new List<string> {"Script"};
                if (SupportsDecisionTable) supportList.Add("Decision");
                if (SupportsDecisionTableOptional) supportList.Add("Decision-Optional");
                if (SupportsQueryTable) supportList.Add("Query");
                if (SupportsQueryTableOptional) supportList.Add("Query-Optional");
                if (SupportsTableTable) supportList.Add("Table");
                supportList.Sort();
                return supportList;
            }
        }

        // Return all public or internal, static or non-static methods. This includes properties (those are just methods under the hood)
        // Although FitSharp can see protected and private methods, convention is not to deliberately expose them. 
        public static IEnumerable<MethodInfo> RelevantMethods(Type type) =>
            type.GetMethods(Flags).Where(x => !x.IsFamily && !x.IsPrivate && !MethodsToSkip.Contains(x.Name));
    }
}