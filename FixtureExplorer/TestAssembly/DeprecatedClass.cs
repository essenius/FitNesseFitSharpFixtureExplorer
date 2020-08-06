﻿// Copyright 2016-2020 Rik Essenius
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
using System.Diagnostics.CodeAnalysis;

namespace TestAssembly
{
    /// <summary>A deprecated class</summary>
    [ExcludeFromCodeCoverage, Obsolete("Use Public Class instead"),
     SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Used by FixtureExplorer"),
     SuppressMessage("ReSharper", "UnusedMember.Local", Justification = "Used by FixtureExplorer")]
    public class DeprecatedClass
    {
        private readonly string _parameter;

        public DeprecatedClass() => _parameter = "none";

        /// <summary>
        ///     Documentation for constructor with one parameter
        /// </summary>
        /// <param name="parameter">documentation for the parameter</param>
        [Documentation("Documentation attribute for constructor with 1 parameter")]
        public DeprecatedClass(string[,] parameter) => _parameter = parameter[0, 0];

        public string PublicMethodInObsoleteClass() => _parameter;
    }
}