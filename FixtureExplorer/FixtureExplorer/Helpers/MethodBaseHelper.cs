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

namespace FixtureExplorer.Helpers
{
    internal class MethodBaseHelper
    {
        protected const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        private readonly MethodBase _methodBase;

        public MethodBaseHelper(MethodBase methodBase, Type parentType)
        {
            _methodBase = methodBase;
            ParentType = parentType;
        }

        private MemberInfo AttributeBase
        {
            get
            {
                // Under the hood Properties are get and set methods. But attributes are only linked to the Property members.
                // So if we have a property, we need to get the attributes from the Property member. Otherwise we take the methodBase.
                var namer = new GracefulNamer(_methodBase.Name);
                return namer.IsProperty
                    ? ParentType.GetMembers(Flags).FirstOrDefault(m => m.MemberType == MemberTypes.Property && m.Name == namer.PropertyName)
                    : _methodBase;
            }
        }

        public List<string> Parameters =>
            _methodBase.GetParameters().Select(parameter => $"{parameter.Name}: {GracefulNamer.GracefulName(parameter.ParameterType)}").ToList();

        protected Type ParentType { get; }

        public string DeprecationMessage()
        {
            var returnValue = AttributeBase.Attribute<ObsoleteAttribute>()?.DeprecationMessage();
            return string.IsNullOrEmpty(returnValue) ? ParentType.DeprecationMessage() : returnValue;
        }

        public string Documentation()
        {
            var documentation = AttributeBase.Documentation();
            if (string.IsNullOrEmpty(documentation))
            {
                documentation = ParentType.DocumentationFor(_methodBase.Name + "`" + _methodBase.GetParameters().Length);
            }
            return documentation;
        }
    }
}