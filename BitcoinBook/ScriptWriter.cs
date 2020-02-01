using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BitcoinBook
{
    public class ScriptWriter : WriterBase
    {
        public ScriptWriter(BinaryWriter writer) : base(writer)
        {
        }

        public ScriptWriter(Stream stream) : base(stream)
        {
        }

        public void Write(IEnumerable<object> commands)
        {
            foreach (var command in commands)
            {
                if (command is OpCode opCode)
                {
                    Write(opCode);
                }
                else
                {
                    if (!(command is byte[] bytes))
                    {
                        throw new FormatException("Wrong type in script commands");
                    }

                    var length = bytes.Length;
                    if (length < (int) OpCode.OP_PUSHDATA1)
                    {
                        Write(length, 1);
                    }
                    else if (length < 0x100)
                    {
                        Write(OpCode.OP_PUSHDATA1);
                        Write(length, 1);
                    }
                    else if (length < 520)
                    {
                        Write(OpCode.OP_PUSHDATA2);
                        Write(length, 2);
                    }
                    else
                    {
                        throw new FormatException("Script command has invalid length");
                    }
                    Write(bytes);
                }
            }
        }
    }
}
