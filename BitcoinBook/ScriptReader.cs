using System;
using System.Collections.Generic;
using System.IO;

namespace BitcoinBook
{
    public class ScriptReader : ReaderBase
    {
        public ScriptReader(BinaryReader reader) : base(reader)
        {
        }

        public ScriptReader(Stream stream) : base(stream)
        {
        }

        public ScriptReader(string hex) : base(hex)
        {
        }

        public IList<object> ReadScript()
        {
            var length = (int)ReadVarLong();
            return ReadScript(length);
        }

        public IList<object> ReadScript(int length)
        {
            throw new NotImplementedException();
        }
    }
}
