using System;
using System.Collections.Generic;
using System.IO;

namespace BitcoinBook
{
    public class TransactionReader
    {
        readonly BinaryReader reader;

        public TransactionReader(BinaryReader reader)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public TransactionReader(Stream stream) : this(new BinaryReader(stream ?? throw new ArgumentNullException(nameof(stream))))
        {
        }

        public Transaction ReadTransaction()
        {
            return new Transaction(ReadInt(4), ReadInputs(ReadVarInt()), ReadOutputs(ReadVarInt()), ReadInt(4));
        }

        IList<TransactionOutput> ReadOutputs(ulong count)
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
            return new TransactionOutput(ReadLong(8), ReadScriptPubKey());
        }

        ScriptPubKey ReadScriptPubKey()
        {
            return new ScriptPubKey(ReadVarBytes());
        }

        IList<TransactionInput> ReadInputs(ulong count)
        {
            var inputs = new List<TransactionInput>();
            while (count-- > 0)
            {
                inputs.Add(ReadInput());
            }

            return inputs;
        }

        TransactionInput ReadInput()
        {
            return new TransactionInput(reader.ReadBytes(32), ReadInt(4), ReadScript(), ReadInt(4));
        }

        uint ReadVersion()
        {
            return ReadInt(4);
        }

        public ulong ReadVarInt()
        {
            var i = ReadLong(1);
            var len = GetVarLength(i);
            return len == 1 ? i : ReadLong(len);
        }

        ScriptSig ReadScript()
        {
            return new ScriptSig(ReadVarBytes());
        }

        byte[] ReadVarBytes()
        {
            return reader.ReadBytes((int) ReadVarInt());
        }

        int GetVarLength(ulong i)
        {
            switch (i)
            {
                case 0xFD:
                    return 2;
                case 0xFE:
                    return 4;
                case 0xFF:
                    return 8;
                default:
                    return 1;
            }
        }

        public uint ReadInt(int length)
        {
            uint i = 0;
            uint factor = 1;
            while (length-- > 0)
            {
                i += reader.ReadByte() * factor;
                factor *= 256;
            }

            return i;
        }

        ulong ReadLong(int length)
        {
            ulong i = 0L;
            ulong factor = 1L;
            while (length-- > 0)
            {
                i += reader.ReadByte() * factor;
                factor *= 256;
            }

            return i;
        }
    }
}
