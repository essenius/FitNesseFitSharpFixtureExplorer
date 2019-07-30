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
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace TestAssembly
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Used by FitSharp")]
    public class PublicClass
    {
        [Documentation("Just a demo public class constructor with one parameter")]
        public PublicClass(int input) => PrivateProperty = input;

        [Documentation("Just a demo public class constructor with two parameters")]
        public PublicClass(int input1, int input2)
        {
            PrivateProperty = input1;
            PublicProperty = input2;
        }

        [Obsolete("Use Public Property instead", false)]
        public bool AnObsoleteProperty { get; set; }

        [Obsolete] public bool AnObsoletePropertyWithoutMessage { get; set; }

        private int PrivateProperty { get; set; }
        public bool PublicGetProperty { get; private set; }

        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global", Justification = "Needed for testing")]
        public int PublicProperty { get; set; }

        public string PublicSetProperty { private get; set; }

        [Documentation("Dummy Public Static Property doing absolutely nothing")]
        public static double? PublicStaticProperty { get; set; }

        [Obsolete("Use Public Method instead", false)]
        public string AnObsoleteMethod() => "obsolete";

        internal void InternalMethod() => PrivateMethod();
        private void PrivateMethod() => PrivateProperty = 1;
        public void PublicMethodNoParams() => PublicGetProperty = false;
        public object PublicMethodOneParam(long longParam) => string.IsNullOrEmpty(PublicSetProperty) ? longParam : PrivateProperty;
        public int PublicMethodParamArray(params int[] list) => list.Length;

        public Dictionary<int, PublicClass> PublicMethodReturningDictionary(KeyValuePair<int, PublicClass> keyValuePair) =>
            new Dictionary<int, PublicClass> {{keyValuePair.Key, keyValuePair.Value}};

        public string PublicMethodTwoParams(double doubleParam, decimal decimalParam) =>
            (doubleParam + Convert.ToDouble(decimalParam)).ToString(CultureInfo.InvariantCulture);

        [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Global", Justification = "FitSharp doesn't deal with enumerables")]
        public static IEnumerable<byte> PublicStaticMethod(List<byte> list) => list;

        public Collection<object> Query() => null;

        [Documentation("returns the element count of the input parameter")]
        public List<object> Table(List<List<string>> list) => new List<object> {list.Count};

        public class NestedClass
        {
            public int PublicProperty { get; set; }
        }
    }
}