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
            var commands = new List<object>();
            var count = 0;
            while (count < length)
            {
                var b = ReadByte();
                ++count;
                if (b > 0 && b < (int) OpCode.OP_PUSHDATA1)
                {
                    commands.Add(ReadUnsignedLong(b));
                    count += b;
                }
                else if (b == (int) OpCode.OP_PUSHDATA1)
                {
                    var l = ReadInt(1);
                    commands.Add(ReadBytes(l));
                    count += l + 1;
                }
                else if (b == (int)OpCode.OP_PUSHDATA2)
                {
                    var l = ReadInt(2);
                    commands.Add(ReadBytes(l));
                    count += l + 2;
                }
                else
                {
                    commands.Add((OpCode)b);
                }
            }

            if (count != length)
            {
                throw new FormatException("Script parsing ended at wrong length");
            }
            return commands;
        }
    }
}
