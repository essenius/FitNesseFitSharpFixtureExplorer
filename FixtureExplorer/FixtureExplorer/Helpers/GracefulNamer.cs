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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace FixtureExplorer.Helpers
{
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "API for FitSharp")]
    internal class GracefulNamer
    {
        private readonly string _fixtureName;
        
        public GracefulNamer(string fixtureName) => _fixtureName = fixtureName;

        // Convert a graceful name into a method name using a state machine
        public string Disgrace
        {
            get
            {
                var machine = new NameStateMachine();
                return _fixtureName.Aggregate(string.Empty, (current, ch) => current + machine.NextChar(ch));
            }
        }

        public bool IsGetProperty => _fixtureName.StartsWith("get_", StringComparison.Ordinal);
        public bool IsProperty => IsGetProperty || IsSetProperty;
        public bool IsSetProperty => _fixtureName.StartsWith("set_", StringComparison.Ordinal);
        public string PropertyName => IsProperty ? _fixtureName.Substring(4) : _fixtureName;

        // Convert a method name into a graceful name
        public string Regrace
        {
            get
            {
                Debug.Assert(!string.IsNullOrEmpty(PropertyName));
                const char separator = '.';
                var result = new StringBuilder();

                var isGrabbingDigits = false;
                var wasSeparator = true;
                foreach (var currentChar in PropertyName)
                {
                    if (char.IsUpper(currentChar) ||
                        char.IsDigit(currentChar) && !isGrabbingDigits ||
                        currentChar == separator)
                    {
                        if (!wasSeparator && currentChar != separator) result.Append(" ");
                        wasSeparator = currentChar == separator;
                    }

                    isGrabbingDigits = char.IsDigit(currentChar);
                    result.Append(currentChar);
                }
                return result.ToString();
            }
        }

        public string Type => IsGetProperty ? "Property (Get)" : IsSetProperty ? "Property (Set)" : "Method";

        public static string GracefulName(Type type)
        {
            Debug.Assert(type.Namespace != null, "type.Namespace != null");
            if (type.IsPrimitive ||
                type.Namespace.Equals("System", StringComparison.InvariantCulture) && !type.IsGenericType)
            {
                // This is a built-in type, so simply return that
                return type.Name;
            }
            if (!type.IsGenericType)
            {
                // this is not a primitive type, nor one of the other core types, and not a list or collection. 
                // So we assume it is a custom type that needs to be converted to graceful format
                return new GracefulNamer(type.Name).Regrace;
            }

            // we have a list, collection, nullable, etc.; find the underlying types.
            var typeList = type.GenericTypeArguments.Select(GracefulName).ToList();
            return type.Name.Split('`')[0] + "<" + string.Join(", ", typeList) + ">";
        }
    }
}