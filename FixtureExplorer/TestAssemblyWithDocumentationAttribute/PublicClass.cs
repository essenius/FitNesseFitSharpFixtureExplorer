// Copyright 2016-2024 Rik Essenius
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
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#pragma warning disable 1591 // We're missing XML comments on purpose
#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CA1822 // Mark members as static - intentional instance, for testing

namespace TestAssemblyWithDocumentationAttribute
{
    [ExcludeFromCodeCoverage]
    public class PublicClass
    {
        /// <summary>
        ///     Just a demo public class constructor with one parameter
        /// </summary>
        /// <param name="input"></param>
        [Documentation("Documentation attribute for public class constructor with one parameter")]
        public PublicClass(int input) => PrivateProperty = input;

        /// <summary>
        ///     Just a demo public class constructor with two parameters
        /// </summary>
        /// <param name="input1">input 1 doc</param>
        /// <param name="input2">input 2 doc</param>
        [Documentation("Documentation attribute for public class constructor with two parameters")]
        public PublicClass(int input1, int? input2)
        {
            PrivateProperty = input1;
            PublicProperty = input2!.Value;
        }

        [Obsolete("Use Public Property instead", false)]
        public bool AnObsoleteProperty { get; set; }

        [Obsolete] public bool AnObsoletePropertyWithoutMessage { get; set; }

        private int PrivateProperty { get; set; }
        public bool PublicGetProperty { get; private set; }

        public int PublicProperty { get; set; }

        public string PublicSetProperty { private get; set; }

        /// <summary>
        ///     Dummy Public Static Property doing absolutely nothing
        /// </summary>
        public static double? PublicStaticProperty { get; set; }

        [Obsolete("Use Public Method instead", false)]
        public string AnObsoleteMethod() => "obsolete";

        internal void InternalMethod() => PrivateMethod();
        private void PrivateMethod() => PrivateProperty = 1;
        public void PublicMethodNoParams() => PublicGetProperty = false;
        public object PublicMethodOneParam(long longParam) => string.IsNullOrEmpty(PublicSetProperty) ? longParam : PrivateProperty;
        public int PublicMethodParamArray(params int[] list) => list.Length;

        public Dictionary<int, PublicClass> PublicMethodReturningDictionary(KeyValuePair<int, PublicClass> keyValuePair) =>
            new Dictionary<int, PublicClass> { { keyValuePair.Key, keyValuePair.Value } };

        public string PublicMethodTwoParams(double doubleParam, decimal decimalParam) =>
            (doubleParam + Convert.ToDouble(decimalParam)).ToString(CultureInfo.InvariantCulture);

        public static IEnumerable<byte> PublicStaticMethod(List<byte> list) => list;

        public Collection<object> Query() => null;

        /// <returns> the element count of the input parameter</returns>
        public List<object> Table(List<List<string>> list) => new List<object> { list.Count };

        public class NestedClass
        {
            public int PublicProperty { get; set; }
        }
    }
}
