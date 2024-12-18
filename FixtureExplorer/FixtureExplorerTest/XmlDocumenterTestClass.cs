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

using System.Diagnostics;

namespace FixtureExplorerTest
{
    /// <summary>Class Def</summary>
    /// <remarks>These documentation entries are used in tests</remarks>
    internal class XmlDocumenterTestClass
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable - intentional, for testing
        private readonly double _input;

        /// <summary>Field1</summary>
        public long Field1;

        /// <summary>XmlDocumenterTestClass()</summary>
        public XmlDocumenterTestClass()
        {
        }

        /// <summary>XmlDocumenterTestClass(double?)</summary>
        /// <param name="input">nullable double</param>
        public XmlDocumenterTestClass(double? input)
        {
            Debug.Assert(input != null, nameof(input) + " != null");
            _input = input!.Value;
            Field1 = _input.GetHashCode();
        }

        /// <remarks>ArrayProperty</remarks>
        /// <guarantees>nothing</guarantees>
        public int[][] ArrayProperty { get; set; }

        /// <summary>Echo(object)</summary>
        /// <remarks>Not very interesting. See <seealso cref="Method1" /></remarks>
        /// <requires>nothing</requires>
        /// <returns>itself</returns>
        /// <summary>forcing ignore of summary</summary>
        public object Echo(ref object input) => input;

        /// <returns>
        ///     the length of
        ///     the string representation of Field1. See <see cref="Echo" />
        /// </returns>
        public int Method1() => Field1.ToString().Length;

        internal class NestedClass
        {
        }
    }
}
