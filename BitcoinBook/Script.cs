using System.Collections.Generic;

namespace BitcoinBook
{
    public class Script
    {
        public IList<object> Commands { get; }

        public Script() : this (null)
        {
        }

        public Script(IList<object> commands)
        {
            Commands = commands ?? new List<object>();
        }

        public Script(IEnumerable<object> commands1, IEnumerable<object> commands2)
        {
            var commands = new List<object>(commands1);
            commands.AddRange(commands2);
            Commands = commands;
        }

        public Script(Script scriptSig, Script scriptPubKey) : this(scriptSig.Commands, scriptPubKey.Commands)
        {
        }
    }
}
