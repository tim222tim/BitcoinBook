using System;
using System.Collections.Generic;

namespace BitcoinBook
{
    public class Script : ICloneable
    {
        public IList<object> Commands { get; }

        public Script() : this (new object[0])
        {
        }

        public Script(params object[] commands) :
            this(commands ?? throw new ArgumentNullException(nameof(commands)), new object[0])
        {
        }

        public Script(IEnumerable<object> commands) : 
            this(commands ?? throw new ArgumentNullException(nameof(commands)), new object[0])
        {
        }

        public Script(IEnumerable<object> commands1, IEnumerable<object> commands2)
        {
            var commands = new List<object>(commands1 ?? throw new ArgumentNullException(nameof(commands1)));
            commands.AddRange(commands2 ??  throw new ArgumentNullException(nameof(commands2)));
            Commands = commands;
        }

        public Script(Script scriptSig, Script scriptPubKey) : 
            this((scriptSig ?? throw new ArgumentNullException(nameof(scriptSig))).Commands, 
                (scriptPubKey ?? throw new ArgumentNullException(nameof(scriptPubKey))).Commands)
        {
        }

        public Script Clone()
        {
            return new Script(Commands);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
