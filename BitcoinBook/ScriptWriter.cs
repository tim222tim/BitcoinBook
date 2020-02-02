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
            var stream = new MemoryStream();
            var writer = new TransactionWriter(stream);
            Write(commands, writer);
            var bytes = stream.ToArray();
            WriteVarBytes(bytes);
        }

        static void Write(IEnumerable<object> commands, TransactionWriter writer)
        {
            foreach (var command in commands)
            {
                if (command is OpCode opCode)
                {
                    writer.Write(opCode);
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
                        writer.Write(length, 1);
                    }
                    else if (length < 0x100)
                    {
                        writer.Write(OpCode.OP_PUSHDATA1);
                        writer.Write(length, 1);
                    }
                    else if (length < 520)
                    {
                        writer.Write(OpCode.OP_PUSHDATA2);
                        writer.Write(length, 2);
                    }
                    else
                    {
                        throw new FormatException("Script value is too long");
                    }
                    writer.Write(bytes);
                }
            }
        }
    }
}
