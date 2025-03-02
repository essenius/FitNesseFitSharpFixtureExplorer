﻿// Copyright 2016-2025 Rik Essenius
//
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
//   except in compliance with the License. You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software distributed under the License 
//   is distributed on an "AS IS" BASIS WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and limitations under the License.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FixtureExplorer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAssemblyWithDocumentationAttribute;

#pragma warning disable 1591 // We're missing XML comments on purpose

namespace FixtureExplorerTest
{
    [TestClass]
    public class FixtureFunctionsTest
    {
        [TestMethod]
        [DeploymentItem("TestAssemblyWithDocumentationAttribute.xml")] // only needed for .NET Framework
        public void FixtureFunctionsDoTableTest()
        {
            var expected = new List<string>
            {
                "|report:Namespace|report:Class|report:Scope|report:Fixture Name|report:Method Type|report:Return Type|report:Parameters|report:Supports Table Type|report:Documentation|",
                "|report:Test Assembly With Documentation Attribute|report:Class Not Supporting Decision Table|report:public|report:Int Method With Param|report:Method|report:String|report:param: Tuple<Nullable<Int32>, Nullable<Decimal>>|report:Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Class Not Supporting Decision Table|report:public|report:Void Method With Two Params|report:Method|report:Void|report:param1: String, param2: Nullable<Int32>|report:Script|report:Writes the two params concatenated. Params: { param1: param1 doc; param2: param2 doc }. Documentation Attribute - Writes the two params concatenated|",
                "|report:Test Assembly With Documentation Attribute|report:Deprecated Class|report:public|report:Public Method In Obsolete Class|report:Method|report:String|report:|report:Decision, Script|report:[Deprecated class: Use Public Class instead]|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:An Obsolete Method|report:Method|report:String|report:|report:Decision, Script|report:[Deprecated: Use Public Method instead]|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:An Obsolete Property|report:Property (Get)|report:Boolean|report:|report:Decision, Script|report:[Deprecated: Use Public Property instead]|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:An Obsolete Property|report:Property (Set)|report:Void|report:value: Boolean|report:Decision, Script|report:[Deprecated: Use Public Property instead]|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:An Obsolete Property Without Message|report:Property (Get)|report:Boolean|report:|report:Decision, Script|report:[Deprecated]|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:An Obsolete Property Without Message|report:Property (Set)|report:Void|report:value: Boolean|report:Decision, Script|report:[Deprecated]|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:internal|report:Internal Method|report:Method|report:Void|report:|report:Script|report:[Internal use only. Do not use in tests]|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:Public Get Property|report:Property (Get)|report:Boolean|report:|report:Decision, Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:Public Method No Params|report:Method|report:Void|report:|report:Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:Public Method One Param|report:Method|report:Object|report:longParam: Int64|report:Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:Public Method Param Array|report:Method|report:Int32|report:list: Int32[]|report:Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:Public Method Returning Dictionary|report:Method|report:Dictionary<Int32, Public Class>|report:keyValuePair: KeyValuePair<Int32, Public Class>|report:Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:Public Method Two Params|report:Method|report:String|report:doubleParam: Double, decimalParam: Decimal|report:Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:Public Property|report:Property (Get)|report:Int32|report:|report:Decision, Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:Public Property|report:Property (Set)|report:Void|report:value: Int32|report:Decision, Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:Public Set Property|report:Property (Set)|report:Void|report:value: String|report:Decision, Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public static|report:Public Static Method|report:Method|report:IEnumerable<Byte>|report:list: List<Byte>|report:Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public static|report:Public Static Property|report:Property (Get)|report:Nullable<Double>|report:|report:Decision, Script|report:Dummy Public Static Property doing absolutely nothing|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public static|report:Public Static Property|report:Property (Set)|report:Void|report:value: Nullable<Double>|report:Decision, Script|report:Dummy Public Static Property doing absolutely nothing|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:Query|report:Method|report:Collection<Object>|report:|report:Query, Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Public Class|report:public|report:Table|report:Method|report:List<Object>|report:list: List<List<String>>|report:Decision-Optional, Query-Optional, Script|report:Returns: the element count of the input parameter|",
                "|report:Test Assembly With Documentation Attribute|report:Wrong Table Class|report:public|report:Begin Table|report:Method|report:Void|report:|report:Decision-Optional, Script|report:Executed just before a Decision table|",
                "|report:Test Assembly With Documentation Attribute|report:Wrong Table Class|report:public|report:Do Table|report:Method|report:List<Object>|report:table: List<Object>|report:Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Wrong Table Class|report:public|report:Execute|report:Method|report:Void|report:command: String|report:Decision, Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Wrong Table Class|report:public|report:Property|report:Property (Get)|report:Int32|report:|report:Decision, Script|report:Property for testing FixtureFor|",
                "|report:Test Assembly With Documentation Attribute|report:Wrong Table Class|report:public|report:Property|report:Property (Set)|report:Void|report:value: Int32|report:Decision, Script|report:Property for testing FixtureFor|",
                "|report:Test Assembly With Documentation Attribute|report:Wrong Table Class|report:public|report:Property Without Documentation|report:Property (Get)|report:Int32|report:|report:Decision, Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Wrong Table Class|report:public|report:Query|report:Method|report:List<Object>|report:input: Int32|report:Script|report:Doesn't really do anything|",
                "|report:Test Assembly With Documentation Attribute|report:Wrong Table Class|report:public|report:Reset|report:Method|report:Int32|report:|report:Decision, Script|report:|",
                "|report:Test Assembly With Documentation Attribute|report:Wrong Table Class|report:public|report:Table|report:Method|report:List<Object>|report:a: Int32, b: Int32|report:Script|report:|"
            };
            var location = Assembly.GetAssembly(typeof(PublicClass)).Location;

            var fixture = new FixtureFunctions(location);
            var result = fixture.DoTable(null);
            var expectedIndex = 0;
            foreach (List<string> row in result)
            {
                var line = expected[expectedIndex];
                var rowString = row.Aggregate("|", (current, cell) => current + cell + "|");
                Assert.AreEqual(line, rowString, $"Row {expectedIndex}");
                expectedIndex++;
            }
        }

    }
}
