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
    }
}
