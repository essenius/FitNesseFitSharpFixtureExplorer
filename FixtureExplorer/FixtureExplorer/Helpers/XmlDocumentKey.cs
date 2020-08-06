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
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FixtureExplorer.Helpers
{
    /// <summary>Converts the method base into a key to be used in XML documentation</summary>
    internal static class XmlDocumentKey
    {
        /// <summary>Gets the key for the Method Base being handed in</summary>
        internal static string MethodBaseKey(MethodBase methodBase)
        {
            if (methodBase == null) return null;
            var parameters = methodBase.GetParameters();
            var typeKey = TypeSpec(methodBase.DeclaringType, false);
            var name = methodBase.IsConstructor ? "#ctor" : methodBase.Name;

            var parameterList = string.Empty;
            var keyPrefix = "M:";
            var realName = new GracefulNamer(methodBase.Name).RealName;
            if (methodBase.DeclaringType?.GetProperty(realName) != null)
            {
                keyPrefix = "P:";
                name = realName;
            }
            else
            {
                if (parameters.Length > 0)
                {
                    parameterList = "(" + string.Join(",", parameters.Select(p => TypeSpec(p.ParameterType, true))) + ")";
                }
            }
            return keyPrefix + typeKey + "." + name + parameterList;
        }

        /// <summary>Gets the key for the Type being handed in</summary>
        internal static string TypeKey(Type type) => "T:" + TypeSpec(type, false);

        /// <param name="type">the type to generate a documentation key for</param>
        /// <param name="isParam">is the key used as a parameter in method or not</param>
        /// <returns>the documentation key to search the XML documentation with</returns>
        /// <remarks>does not support generic parameters, as we can't use those in the FitSharp interface anyway</remarks>
        internal static string TypeSpec(Type type, bool isParam)
        {
            if (type.HasElementType)
            {
                var elementTypeKey = TypeSpec(type.GetElementType(), isParam);

                if (type.IsPointer)
                {
                    return elementTypeKey + "*";
                }
                if (type.IsByRef)
                {
                    return elementTypeKey + "@";
                }
                // only thing left is Array now, as per the HasElementType definition
                var rank = type.GetArrayRank();
                var arrayDimension = "[" + (rank > 1 ? string.Join(",", Enumerable.Repeat("0:", rank)) : string.Empty) + "]";
                return elementTypeKey + arrayDimension;
            }
            var prefix = (type.IsNested ? TypeSpec(type.DeclaringType, isParam) : type.Namespace) + ".";
            var suffix = "";
            if (type.IsGenericType && isParam)
            {
                suffix = "{" + string.Join(",", type.GetGenericArguments().Select(arg => TypeSpec(arg, true))) + "}";
            }
            var typeName = isParam ? Regex.Replace(type.Name, @"`\d+", string.Empty) : type.Name;
            return prefix + typeName + suffix;
        }
    }
}
