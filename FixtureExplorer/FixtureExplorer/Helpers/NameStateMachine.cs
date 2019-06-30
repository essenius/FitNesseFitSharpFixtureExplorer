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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FixtureExplorer.Helpers
{
    internal enum NameState
    {
        Word,
        Number,
        OutOfWord
    }

    internal enum NameEvent
    {
        Letter,
        Digit,
        Other
    }

    internal enum NameAction
    {
        None,
        Upper,
        AsIs
    }

    internal struct StateTransitionKey
    {
        public NameState NameState;
        public NameEvent NameEvent;
    }

    internal struct StateTransitionValue
    {
        public NameState NextState;
        public NameAction Action;
    }

    // this state machine converts a graceful name (i.e. with spaces and potentially not capitalized)
    // into a C# method name. It takes over letters and digits, ignores other characters, and capitalizes the first
    // letter of each word (i.e. PascalCase)
    internal class NameStateMachine
    {
        private readonly Dictionary<StateTransitionKey, StateTransitionValue> _dict = new Dictionary
            <StateTransitionKey, StateTransitionValue>
            {
                {
                    new StateTransitionKey {NameState = NameState.Word, NameEvent = NameEvent.Letter},
                    new StateTransitionValue {NextState = NameState.Word, Action = NameAction.AsIs}
                },
                {
                    new StateTransitionKey {NameState = NameState.Word, NameEvent = NameEvent.Digit},
                    new StateTransitionValue {NextState = NameState.Number, Action = NameAction.AsIs}
                },
                {
                    new StateTransitionKey {NameState = NameState.Word, NameEvent = NameEvent.Other},
                    new StateTransitionValue {NextState = NameState.OutOfWord, Action = NameAction.None}
                },
                {
                    new StateTransitionKey {NameState = NameState.Number, NameEvent = NameEvent.Letter},
                    new StateTransitionValue {NextState = NameState.Word, Action = NameAction.Upper}
                },
                {
                    new StateTransitionKey {NameState = NameState.Number, NameEvent = NameEvent.Digit},
                    new StateTransitionValue {NextState = NameState.Number, Action = NameAction.AsIs}
                },
                {
                    new StateTransitionKey {NameState = NameState.Number, NameEvent = NameEvent.Other},
                    new StateTransitionValue {NextState = NameState.OutOfWord, Action = NameAction.None}
                },
                {
                    new StateTransitionKey {NameState = NameState.OutOfWord, NameEvent = NameEvent.Letter},
                    new StateTransitionValue {NextState = NameState.Word, Action = NameAction.Upper}
                },
                {
                    new StateTransitionKey {NameState = NameState.OutOfWord, NameEvent = NameEvent.Digit},
                    new StateTransitionValue {NextState = NameState.Number, Action = NameAction.AsIs}
                },
                {
                    new StateTransitionKey {NameState = NameState.OutOfWord, NameEvent = NameEvent.Other},
                    new StateTransitionValue {NextState = NameState.OutOfWord, Action = NameAction.None}
                }
            };

        private NameState _state;

        public NameStateMachine() => _state = NameState.OutOfWord;

        private static NameEvent NameEventFor(char c)
        {
            if (char.IsDigit(c)) return NameEvent.Digit;
            return char.IsLetter(c) ? NameEvent.Letter : NameEvent.Other;
        }

        [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases", Justification = "Included in default")]
        public string NextChar(char c)
        {
            var nameEvent = NameEventFor(c);
            var next = _dict[new StateTransitionKey {NameState = _state, NameEvent = nameEvent}];
            _state = next.NextState;
            switch (next.Action)
            {
                case NameAction.AsIs:
                    return c.ToString();
                case NameAction.Upper:
                    return c.ToString().ToUpperInvariant();
                default: // includes None
                    return string.Empty;
            }
        }
    }
}