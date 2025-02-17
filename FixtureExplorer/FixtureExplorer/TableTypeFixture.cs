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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using FixtureExplorer.Helpers;

namespace FixtureExplorer
{
    /// <summary>Definition of a generic TableType fixture. Uses the Template pattern</summary>
    public abstract class TableTypeFixture
    {
        private readonly string _assemblyName;
        private Assembly _assembly;

        /// <summary>Initialize TableTypeFixture with an assembly name</summary>
        protected TableTypeFixture(string assemblyName) => _assemblyName = assemblyName;

        /// <returns>The list of public non-static classes in the assembly (which FitSharp can work with)</returns>
        /// <remarks>If a class is sealed and abstract, it's a static class. We exclude exception or attribute classes</remarks>
        protected IEnumerable<Type> ClassesVisibleToFitNesse
        {
            get
            {
                return FixtureAssembly().GetTypes().Where(t =>
                    t.IsPublic && t.IsClass && !(t.IsSealed && t.IsAbstract) &&
                    !t.IsSubclassOf(typeof(Exception)) && !t.IsSubclassOf(typeof(Attribute)));
            }
        }

        /// <summary>Create the DoTable result list with a header row.</summary>
        protected abstract List<object> ListWithHeaderRow { get; }

        /// <summary>Add an item to the DoTable result list.</summary>
        protected abstract void AddToList(List<object> result, Type type);

        /// <summary>The Table Table interface for FitSharp</summary>
        /// <param name="table">ignored, required for the interface</param>
        /// <remarks>uses the Template pattern, ListWithHeaderRow and AddToList are overriden in derived classes</remarks>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "FitSharp signature")]
        public List<object> DoTable(List<List<string>> table)
        {
            var returnList = ListWithHeaderRow;

            foreach (var type in ClassesVisibleToFitNesse.OrderBy(type => type.Name))
            {
                AddToList(returnList, type);
            }
            return returnList;
        }

        /// <summary>Memory function delivering the assembly to work with</summary>
        private Assembly FixtureAssembly()
        {
            if (_assembly != null) return _assembly;
            var locator = new AssemblyLocator(_assemblyName, ".");
            _assembly = Assembly.LoadFrom(Path.GetFullPath(locator.FindAssemblyPath()));
            return _assembly;
        }

        /// <returns>the input in the format for reporting in an output cell</returns>
        protected static string Report(string input) => "report:" + input;
    }
}
