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
            var inputs = new List<TransactionInput>();
            while (count-- > 0)
            {
                inputs.Add(ReadInput());
            }

            return inputs;
        }

        TransactionInput ReadInput()
        {
            return new TransactionInput(ReadBytesReverse(32), ReadInt(4), ReadScriptBytes(), ReadUnsignedInt(4));
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
            return new TransactionOutput((long)ReadLong(8), ReadScriptPubKey());
        }

        ScriptSig ReadScriptBytes()
        {
            return new ScriptSig(ReadVarBytes());
        }

        ScriptPubKey ReadScriptPubKey()
        {
            return new ScriptPubKey(ReadVarBytes());
        }
    }
}
