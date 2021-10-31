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
using System.Diagnostics.CodeAnalysis;

#pragma warning disable 1591 // We're missing XML comments on purpose

namespace TestAssemblyWithDocumentationAttribute
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Intentional instance methods for testing")]
    public class ClassNotSupportingDecisionTable
    {
        public string IntMethodWithParam(Tuple<int?, decimal?> param) => param.ToString();

        /// <summary>
        /// Writes the two params concatenated
        /// </summary>
        /// <param name="param1">param1 doc</param>
        /// <param name="param2">param2 doc</param>
        [Documentation("Documentation Attribute - Writes the two params concatenated")]
        public void VoidMethodWithTwoParams(string param1, int? param2) => Console.WriteLine(param1 + param2);
    }
}
