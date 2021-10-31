// Copyright 2016-2021 Rik Essenius
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

namespace FixtureExplorer
{
    /// <summary>Documentation mechanism for fixtures.</summary>
    /// <remarks>Superseded by using XML documentation, but still supported for older fixture versions</remarks>
    [AttributeUsage(AttributeTargets.All)]
    public class DocumentationAttribute : Attribute
    {
        /// <summary>Define the documentation</summary>
        public DocumentationAttribute(string message) => Message = message;

        /// <summary>the documentation to be shown</summary>
        public string Message { get; }
    }
}
