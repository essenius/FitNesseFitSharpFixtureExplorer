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

using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FixtureExplorer.Helpers
{
    /// <summary>
    ///     Get documentation from a method via reflection, and determines based on the signatures what kind of fixture tables
    ///     the method can support.
    /// </summary>
    /// <remarks>It obtains access to the documentation retrieval via the sublassing of MethodBaseDocumenter</remarks>
    internal class FixtureDocumenter : MethodBaseDocumenter
    {
        private static readonly List<string> DecisionTableVoidMethods = new List<string>
        {
            "BeginTable",
            "EndTable",
            "Reset",
            "Execute"
        };

        private readonly MethodInfo _methodInfo;

        public FixtureDocumenter(MethodInfo methodInfo) : base(methodInfo) => _methodInfo = methodInfo;

        /// <summary>
        ///     A property signature means:
        ///     not having a return type and a single parameter (setter), or
        ///     having a return type and no parameters (getter) and not being a Query method
        /// </summary>
        private bool HasPropertySignature =>
            _methodInfo.ReturnType == typeof(void) && _methodInfo.GetParameters().Length == 1 ||
            _methodInfo.ReturnType != typeof(void) && _methodInfo.GetParameters().Length == 0 && !SupportsQueryTable;

        /// <summary> A Query signature means returning a List and not having parameters</summary>
        private bool HasQuerySignature =>
            typeof(IList).IsAssignableFrom(_methodInfo.ReturnType) &&
            _methodInfo.GetParameters().Length == 0;

        /// <summary>A Table fixtures must have a list as return value, and a list of lists as single parameter</summary>
        private bool HasTableSignature
        {
            get
            {
                // First we check whether both return value and parameters are lists
                var isCandidate =
                    typeof(IList).IsAssignableFrom(_methodInfo.ReturnType) &&
                    _methodInfo.GetParameters().Length == 1 &&
                    typeof(IList).IsAssignableFrom(_methodInfo.GetParameters()[0].ParameterType);
                if (!isCandidate) return false;

                // Now we check if the parameter is a list of lists
                var parameterType = _methodInfo.GetParameters()[0].ParameterType;
                var genericType = parameterType.GenericTypeArguments[0];
                return typeof(IList).IsAssignableFrom(genericType);
            }
        }

        // A void signature has no return type and no parameters
        private bool HasVoidSignature => _methodInfo.ReturnType == typeof(void) && _methodInfo.GetParameters().Length == 0;
        private bool SupportsDecisionTable => HasPropertySignature;

        /// <remarks>
        ///     Decision tables can have an optional Table method executed just after the constructor,
        ///     passing  a list of lists containing all cells of the table except the very first row.
        ///     It can also have a couple of void methods: BeginTable, EndTable, Reset and Execute.
        /// </remarks>
        private bool SupportsDecisionTableOptional =>
            DecisionTableVoidMethods.Contains(_methodInfo.Name) && HasVoidSignature ||
            _methodInfo.Name == "Table" && HasTableSignature;

        /// <summary>A method supports the Query table if it is called Query and has a Query signature</summary>
        private bool SupportsQueryTable => _methodInfo.Name == "Query" && HasQuerySignature;

        /// <summary>Query tables have an optional Table method with a Table signature</summary>
        private bool SupportsQueryTableOptional => _methodInfo.Name == "Table" && HasTableSignature;

        /// <summary>A Table Table requires a DoTable method with a Table signature</summary>
        private bool SupportsTableTable => _methodInfo.Name == "DoTable" && HasTableSignature;

        /// <summary>The list of FitNesse table types that the method can support.</summary>
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
    }
}