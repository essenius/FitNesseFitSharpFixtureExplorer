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
using System.Linq;
using System.Reflection;

namespace FixtureExplorer.Helpers
{
    /// <remarks>
    ///     Using the Strategy pattern to support multiple documentation mechanisms (attribute, dictionary and xml).
    ///     MethodBase supports both constructors and methods
    /// </remarks>
    internal class MethodBaseDocumenter
    {
        /// <remarks>
        ///     All public or internal, static or non-static methods. Includes properties (those are just methods under the hood).
        ///     Although FitSharp can see protected and private methods, convention is not to deliberately expose them.
        /// </remarks>
        private const BindingFlags RelevantBindings =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        /// <summary>AttributeDocumenter is a special case because of the Obsolete attributes</summary>
        private readonly AttributeDocumenter _attributeDocumenter;

        /// <summary>the documenters we use</summary>
        private readonly List<IDocumenter> _documenters;

        /// <summary>the MethodBase we're working with</summary>
        private readonly MethodBase _methodBase;

        public MethodBaseDocumenter(MethodBase methodBase)
        {
            _methodBase = methodBase;
            var parentType = methodBase.DeclaringType;
            _attributeDocumenter = new AttributeDocumenter(parentType, RelevantBindings);
            _documenters = new List<IDocumenter>
                { new XmlDocumenter(parentType), _attributeDocumenter, new DictionaryDocumenter(parentType) };
        }

        /// <returns>The concatenation of type documentation retrieved via the different mechanisms.</returns>
        /// <remarks>here is where we use the Strategy pattern to hide the actual mechanism</remarks>
        public string ConstructorDocumentation =>
            ConcatenateDocumentation(x => x.ConstructorDocumentation((ConstructorInfo)_methodBase));

        /// <returns>The corresponding message if the method or the class is declared obsolete, an empty string otherwise.</returns>
        /// <remarks>This only uses the AttributeDocumenter as we have no choice here</remarks>
        public string DeprecationMessage
        {
            get
            {
                var returnValue = _attributeDocumenter.MethodBaseDeprecationMessage(_methodBase);
                return !string.IsNullOrEmpty(returnValue) ? returnValue : _attributeDocumenter.TypeDeprecationMessage;
            }
        }

        /// <returns>the concatenation of method documentation retrieved via the different mechanisms</returns>
        /// <remarks>Another use of the Strategy pattern</remarks>
        public string MethodBaseDocumentation => ConcatenateDocumentation(x => x.MethodBaseDocumentation(_methodBase));

        /// <remarks>
        ///     Hide some methods inherited from Object - those are always there and generally not useful to mention -
        ///     and the static Documentation property that we defined in previous versions of the fixtures to provide more input
        ///     about the functions.
        /// </remarks>
        private static List<string> MethodsToSkip { get; } = new List<string>
        {
            "get_" + DictionaryDocumenter.FixtureDocumentationProperty,
            "Equals",
            "Finalize",
            "GetHashCode",
            "GetType",
            "MemberwiseClone",
            "ToString"
        };

        /// <summary>Returns a list parameters of the method, with gracefully named type</summary>
        public List<string> Parameters => _methodBase
            .GetParameters()
            .Select(parameter => $"{parameter.Name}: {GracefulNamer.GracefulName(parameter.ParameterType)}")
            .ToList();

        /// <summary>list of public, internal or static qualifiers of the method</summary>
        /// <remarks>Private not supported by design (not deliberately exposing private methods)</remarks>
        public List<string> Scope
        {
            get
            {
                var scope = new List<string> { _methodBase.IsPublic ? "public" : "internal" };
                if (_methodBase.IsStatic)
                {
                    scope.Add("static");
                }
                return scope;
            }
        }

        /// <summary>
        ///     Since we allow use of more than one documentation mechanism, concatenate any non-empty responses
        ///     with a period and a space as separators.
        /// </summary>
        /// <remarks>
        ///     We're using a lambda function here to allow using both function (Constructor) and class (MethodBase)
        ///     documentation
        /// </remarks>
        /// <param name="documentationRetriever"></param>
        private string ConcatenateDocumentation(Func<IDocumenter, string> documentationRetriever)
        {
            var documentation = _documenters.Select(documentationRetriever).ToList();
            documentation = documentation.Where(s => !string.IsNullOrEmpty(s)).ToList();
            var result = string.Join(". ", documentation);
            return result;
        }

        /// <summary>Relevant methods are not protected or private, and are not methods we want to ignore.</summary>
        public static IEnumerable<MethodInfo> RelevantMethods(Type type) =>
            type
                .GetMethods(RelevantBindings)
                .Where(x => !x.IsFamily && !x.IsPrivate && !MethodsToSkip.Contains(x.Name));
    }
}
