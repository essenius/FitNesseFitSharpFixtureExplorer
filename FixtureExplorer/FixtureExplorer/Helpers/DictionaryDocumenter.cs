// Copyright 2016-2021 Rik Essenius
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
using System.Reflection;

namespace FixtureExplorer.Helpers
{
    /// <summary>
    ///     This is the classical way of documenting a fixture: via a public static Dictionary&lt;string, string&gt;
    ///     FixtureDocumentation property. It was replaced by the XMLDocumenter mechanism, but for backward compatibility with
    ///     older fixtures it is still suppported.
    /// </summary>
    internal class DictionaryDocumenter : IDocumenter
    {
        private readonly Dictionary<string, string> _documentation = new Dictionary<string, string>();

        /// <summary>Get the dictionary for the indicate type</summary>
        public DictionaryDocumenter(Type type)
        {
            var docProperty = type.GetProperty(FixtureDocumentationProperty, BindingFlags.Public | BindingFlags.Static);
            if (docProperty != null)
            {
                _documentation = docProperty.GetValue(null) as Dictionary<string, string>;
            }
        }

        internal static string FixtureDocumentationProperty { get; } = "FixtureDocumentation";

        /// <remarks>IDocumenter interface implementation. Defers to MethodBaseDocumentation as it's the same</remarks>
        public string ConstructorDocumentation(ConstructorInfo constructor) => MethodBaseDocumentation(constructor);

        /// <remarks>IDocumenter interface implementation</remarks>
        public string MethodBaseDocumentation(MethodBase methodBase)
        {
            var key = methodBase.IsConstructor ? string.Empty : methodBase.Name;
            key += "`" + methodBase.GetParameters().Length;
            return DocumentationFor(key);
        }

        /// <summary>
        ///     If the documentation is specific about the number of parameters, prioritize it. If not, split out the number
        ///     of parameters and take the base name. If documentation exists return it, else return the empty string
        /// </summary>
        /// <remarks>With constructors splitting out implies returning the class documentation</remarks>
        private string DocumentationFor(string key)
        {
            if (_documentation.ContainsKey(key)) return _documentation[key];
            var keyWithoutParamCount = key.Split('`')[0];
            var nameToSeach = new GracefulNamer(keyWithoutParamCount).RealName;
            return _documentation.ContainsKey(nameToSeach) ? _documentation[nameToSeach] : string.Empty;
        }
    }
}
