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
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FixtureExplorer.Helpers
{
    /// <summary>Use the Documentation attribute to retrieve documentation. Superseded by XML Documentation, but still supported</summary>
    internal class AttributeDocumenter : IDocumenter
    {
        private const string DocumentationAttribute = "DocumentationAttribute";
        private readonly BindingFlags _requiredBindings;

        private readonly Type _type;

        public AttributeDocumenter(Type type, BindingFlags requiredBindings)
        {
            _type = type;
            _requiredBindings = requiredBindings;
        }

        /// <returns>Class deprecation message if obsolete, else an empty string</returns>
        public string TypeDeprecationMessage => DeprecationMessage(_type, "class");

        /// <returns>constructor documentation if available, else the class documentation.</returns>
        public string ConstructorDocumentation(ConstructorInfo constructor)
        {
            var result = MethodBaseDocumentation(constructor);
            return string.IsNullOrEmpty(result) ? DocumentationFor(_type) : result;
        }

        /// <returns>the documentation for the method base</returns>
        public string MethodBaseDocumentation(MethodBase methodBase) => DocumentationFor(AttributeBase(methodBase));

        /// <returns>The indicated attribute for the member if available, null otherwise</returns>
        private static T Attribute<T>(MemberInfo memberInfo) where T : Attribute
        {
            var attribs = memberInfo.GetCustomAttributes(typeof(T), false);
            if (attribs.Length == 0) return default;
            return (T) attribs[0];
        }

        /// <summary>
        ///     Under the hood Properties are get and set methods. But attributes are only linked to the Property members.
        ///     So if we have a property, we need to get the attributes from the Property member. Otherwise we take the methodBase.
        /// </summary>
        /// <remarks>Requires: methodBase != null and methodBase.DeclaringType != null</remarks>
        private MemberInfo AttributeBase(MemberInfo methodBase)
        {
            // Under the hood Properties are get and set methods. But attributes are only linked to the Property members.
            // So if we have a property, we need to get the attributes from the Property member. Otherwise we take the methodBase.
            Debug.Assert(methodBase != null, nameof(methodBase) + " != null");
            var namer = new GracefulNamer(methodBase.Name);
            var parent = methodBase.DeclaringType;
            Debug.Assert(parent != null, nameof(parent) + " != null");
            return namer.IsProperty
                ? parent.GetMembers(_requiredBindings).FirstOrDefault(m => m.MemberType == MemberTypes.Property && m.Name == namer.RealName)
                : methodBase;
        }

        /// <returns>[Deprecated] with the ObsoleteAttribute Message if available, empty string if not Obsolete</returns>
        private static string DeprecationMessage(MemberInfo memberInfo, string entity = null)
        {
            var attribute = Attribute<ObsoleteAttribute>(memberInfo);
            if (attribute == null) return string.Empty;
            if (string.IsNullOrEmpty(attribute.Message)) return "[Deprecated]";
            var delimiter = string.IsNullOrEmpty(entity) ? string.Empty : " ";
            return $"[Deprecated{delimiter}{entity}: {attribute.Message}]";
        }

        /// <returns>the Message property of the Documentation attribute of the member if available, an empty string otherwise</returns>
        private static string DocumentationFor(MemberInfo memberInfo)
        {
            // We need reflection here because the namespace of the DocumentationAttribute class may differ per assembly
            var documentationAttribute = memberInfo.GetCustomAttributes(false)
                .FirstOrDefault(obj => obj.GetType().Name.Equals(DocumentationAttribute, StringComparison.Ordinal));
            var documentation = documentationAttribute?.GetType().GetProperty("Message")?.GetValue(documentationAttribute)?.ToString();
            return documentation;
        }

        /// <returns>The Deprecation Message of the methodBase</returns>
        public string MethodBaseDeprecationMessage(MethodBase methodBase) => DeprecationMessage(AttributeBase(methodBase));
    }
}