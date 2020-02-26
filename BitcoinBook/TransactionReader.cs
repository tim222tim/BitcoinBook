using System;
using System.Collections.Generic;
using System.IO;

namespace BitcoinBook
{
    public class TransactionReader : ReaderBase
    {
        public TransactionReader(BinaryReader reader) : base(reader)
        {
        }

        public TransactionReader(Stream stream) : base(stream)
        {
        }

        public TransactionReader(string hex) : base(hex)
        {
        }

        public Transaction ReadTransaction()
        {
            return new Transaction(ReadInt(4), ReadInputs(ReadVarInt()), ReadOutputs(ReadVarInt()), ReadUnsignedInt(4));
        }

        IList<TransactionInput> ReadInputs(int count)
        {
            if (count == 0)
            {
                if (ReadByte() != 1)
                {
                    throw new FormatException("Invalid segwit marker");
                }

                count = ReadVarInt();
            }
            var inputs = new List<TransactionInput>();
            while (count-- > 0)
            {
                inputs.Add(ReadInput());
            }

            return inputs;
        }

        TransactionInput ReadInput()
        {
            return new TransactionInput(ReadBytesReverse(32), ReadInt(4), ReadScript(), ReadUnsignedInt(4));
        }

        IList<TransactionOutput> ReadOutputs(int count)
        {
            var outputs = new List<TransactionOutput>();
            while (count-- > 0)
            {
                outputs.Add(ReadOutput());
            }

            return outputs;
        }

        TransactionOutput ReadOutput()
        {
            return new TransactionOutput(ReadLong(8), ReadScript());
        }

        public Script ReadScript()
        {
            var length = ReadVarInt();
            return ReadScript(length);
        }

        public Script ReadScript(int length)
        {
            var commands = new List<object>();
            var count = 0;
            while (count < length)
            {
                var b = ReadByte();
                ++count;
                if (b > 0 && b < (int) OpCode.OP_PUSHDATA1)
                {
                    commands.Add(ReadBytes(b));
                    count += b;
                }
                else if (b == (int) OpCode.OP_PUSHDATA1)
                {
                    var size = ReadInt(1);
                    commands.Add(ReadBytes(size));
                    count += size + 1;
                }
                else if (b == (int) OpCode.OP_PUSHDATA2)
                {
                    var size = ReadInt(2);
                    commands.Add(ReadBytes(size));
                    count += size + 2;
                }
                else
                {
                    commands.Add((OpCode) b);
                }
            }

            if (count != length)
            {
                throw new FormatException("Script parsing ended at wrong length");
            }

            return new Script(commands);
        }
    }
}
