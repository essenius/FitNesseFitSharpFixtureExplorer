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
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FixtureExplorer.Helpers
{
    public static class ExtensionFunctions
    {
        public static T Attribute<T>(this MemberInfo memberInfo) where T : Attribute
        {
            var attribs = memberInfo.GetCustomAttributes(typeof(T), false);
            if (attribs.Length == 0) return default(T);
            return (T) attribs[0];
        }

        public static string DeprecationMessage(this Type type) => type.Attribute<ObsoleteAttribute>().DeprecationMessage("class");

        public static string DeprecationMessage(this ObsoleteAttribute attribute, string entity = null)
        {
            if (attribute == null) return string.Empty;
            if (string.IsNullOrEmpty(attribute.Message)) return "[Deprecated]";
            var delimiter = string.IsNullOrEmpty(entity) ? string.Empty : " ";
            return $"[Deprecated{delimiter}{entity}: {attribute.Message}]";
        }

        public static string Documentation(this MemberInfo info)
        {
            // We need reflection here because the namespace of the DocumentationAttribute class may differ per assembly
            var attrib = info.GetCustomAttributes(false).FirstOrDefault(obj => obj.GetType().Name.Equals("DocumentationAttribute"));
            var documentation = attrib?.GetType().GetProperty("Message")?.GetValue(attrib).ToString();
            return documentation;
        }

        public static string DocumentationFor(this Type type, string key)
        {
            // This is the classical way of documenting a fixture: via a public static Dictionary<string, string> FixtureDocumentation property.
            // It was replaced by the DocumentationAttribute mechanism, but for backward compatibility with older fixtures it is still suppported.
            var docProperty = type.GetProperty(MethodHelper.FixtureDocumentationProperty, BindingFlags.Public | BindingFlags.Static);
            if (docProperty == null) return null;
            var docDictionary = docProperty.GetValue(null) as Dictionary<string, string>;
            Debug.Assert(docDictionary != null, "docDictionary != null");
            if (docDictionary.ContainsKey(key)) return docDictionary[key];
            var keyWithoutParamCount = key.Split('`')[0];
            if (docDictionary.ContainsKey(keyWithoutParamCount)) return docDictionary[keyWithoutParamCount];
            if (!keyWithoutParamCount.StartsWith("get_") && !keyWithoutParamCount.StartsWith("set_")) return null;
            var propertyKey = keyWithoutParamCount.Substring(4);
            return docDictionary.ContainsKey(propertyKey) ? docDictionary[propertyKey] : null;
        }
    }
}