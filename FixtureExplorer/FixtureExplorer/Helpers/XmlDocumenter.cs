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
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace FixtureExplorer.Helpers
{
    /// <summary>
    ///     Use the XML documentation facility of C# to provide documentation. This expects the documentation XML file in the same folder
    ///     as the assembly, with the same base name.
    /// </summary>
    internal class XmlDocumenter : IDocumenter
    {
        private static bool _isLoaded;
        private static Type _type;
        private static readonly Dictionary<string, string> Documentation = new Dictionary<string, string>();

        /// <summary>Load the documentation file into a dictionary</summary>
        /// <remarks>As multiple calls with the same assembly are likely, we're only loading the  dictionary if the type changes</remarks>
        public XmlDocumenter(Type type)
        {
            if (type == _type && _isLoaded) return;
            _type = type;
            ReadDocumentation();
            _isLoaded = true;
        }

        /// <returns>the path of the documentation XML file. Assumes that the assembly (+code base) exists</returns>
        private static string XmlFilePath
        {
            get
            {
                var assembly = _type.Assembly;
                var codeBase = Path.GetDirectoryName(assembly.CodeBase);
                Debug.Assert(codeBase != null, nameof(codeBase) + " != null");
                var uri = new UriBuilder(codeBase);
                var assemblyPath = Uri.UnescapeDataString(uri.Path);
                return Path.Combine(assemblyPath, assembly.GetName().Name + ".xml");
            }
        }

        /// <remarks>IDocumenter interface implementation</remarks>
        public string ConstructorDocumentation(ConstructorInfo constructor)
        {
            var result = MethodBaseDocumentation(constructor);
            if (string.IsNullOrEmpty(result)) return DocumentationFor(XmlDocumentKey.TypeKey(_type));
            return result;
        }

        /// <remarks>IDocumenter interface implementation</remarks>
        public string MethodBaseDocumentation(MethodBase methodBase) => DocumentationFor(XmlDocumentKey.MethodBaseKey(methodBase));

        /// <summary>
        ///     As XML documentation has multiple sections (summary, remarks, returns, etc.) we need to merge these. We start
        ///     with the summary if it's there, and then we add all other available sections based on the sequence they were entered.
        /// </summary>
        private static string AssembleResult(Dictionary<string, string> docDict)
        {
            var result = new List<string>();
            if (docDict.ContainsKey("summary")) result.Add(TrimWhitespace(docDict["summary"]));
            foreach (var docKey in docDict.Keys.Except(new[] {"summary"}))
            {
                result.Add(Capitalize(docKey) + ": " + TrimWhitespace(docDict[docKey]));
            }

            return string.Join(". ", result);
        }

        ///<requires>key must be at least one character</requires>
        private static string Capitalize(string key) => key.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture) + key.Substring(1);

        /// <summary>
        ///     Get documentation from the loaded dictionary. Parses the XML section to extract the sections belonging to the element,
        ///     and combines them into a single string
        /// </summary>
        /// <returns>the combined documentation</returns>
        /// <remarks>the XML file must be valid XML documentation format. Invalid entries are ignored.</remarks>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc/" />
        private static string DocumentationFor(string key)
        {
            Documentation.TryGetValue(key, out var documentation);
            if (documentation == null) return null;
            var xmlDocument = ParseXml(documentation);

            var docDict = new Dictionary<string, string>();
            var parameters = new List<string>();
            var rootNode = xmlDocument.FirstChild;
            foreach (XmlNode entry in rootNode.ChildNodes)
            {
                if (entry.Name == "param")
                {
                    // if there is no attribute named 'name', or the value is empty, ignore it.
                    var name = entry.Attributes?.GetNamedItem("name")?.InnerText;
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(entry.InnerText))
                    {
                        parameters.Add(name + ": " + entry.InnerText);
                    }
                }
                else
                {
                    docDict.Add(entry.Name, entry.InnerText);
                }
            }
            if (parameters.Count > 0)
            {
                docDict.Add("params", "{ " + string.Join("; ", parameters) + " }");
            }

            return AssembleResult(docDict);
        }

        /// <summary>Parse an XML string into an XmlDocument.</summary>
        /// <remarks>Assumes that the XML string is valid</remarks>
        private static XmlDocument ParseXml(string input)
        {
            var xmlDocument = new XmlDocument {XmlResolver = null};
            var stringReader = new StringReader(input);
            using (var reader = XmlReader.Create(stringReader, new XmlReaderSettings {XmlResolver = null}))
            {
                xmlDocument.Load(reader);
            }
            return xmlDocument;
        }

        /// <summary>
        ///     Read the documentation from the XML file into the Documentation dictionary. Each member element key gets its own entry in
        ///     the dictionary, and the XML section of that member becomes the value (with a root section around it).
        /// </summary>
        private static void ReadDocumentation()
        {
            var xmlFile = XmlFilePath;
            if (!File.Exists(xmlFile)) return;
            using (var streamReader = new StreamReader(xmlFile))
            using (var xmlReader = XmlReader.Create(streamReader))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType != XmlNodeType.Element || xmlReader.Name != "member") continue;
                    var memberName = xmlReader["name"];

                    // should not occur, would imply a corrupted XML file. Checking just in case.
                    if (memberName == null) continue;
                    Documentation[memberName] = "<root>" + xmlReader.ReadInnerXml().Trim() + "</root>";
                }
            }
        }

        /// <summary>
        ///     Remove all new line characters and leading/trailing spaces from the text lines in the input, to transform documentation
        ///     entries (like this) into a single line without excess whitespace.
        /// </summary>
        private static string TrimWhitespace(string input)
        {
            var lines = input.Split(Environment.NewLine.ToCharArray())
                .Select(x => x.Trim())
                .ToArray()
                .Where(s => !string.IsNullOrEmpty(s));
            var result = string.Join(" ", lines);
            return result;
        }
    }
}
