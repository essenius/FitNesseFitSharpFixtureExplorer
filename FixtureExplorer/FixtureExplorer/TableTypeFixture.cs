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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FixtureExplorer
{
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Used by FitSharp")]
    public abstract class TableTypeFixture
    {
        private readonly string _assemblyName;

        protected TableTypeFixture(string assemblyName) => _assemblyName = assemblyName;

        protected IEnumerable<Type> ClassesVisibleToFitNesse
        {
            get
            {
                var asm = Assembly.LoadFrom(Path.GetFullPath(_assemblyName));

                // FitNesse needs public non-static classes to work with. If a class is sealed and abstract, it's a static class.
                // we also don't want to report exception or attribute classes

                return asm.GetTypes().Where(t =>
                    t.IsPublic && t.IsClass && !(t.IsSealed && t.IsAbstract) &&
                    !t.IsSubclassOf(typeof(Exception)) && !t.IsSubclassOf(typeof(Attribute)));
            }
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "FitSharp signature")]
        public abstract List<object> DoTable(List<List<string>> table);

        protected static string Report(string input) => "report:" + input;
    }
}