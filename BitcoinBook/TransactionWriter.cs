using System.Collections.Generic;
using System.IO;

namespace BitcoinBook
{
    public class TransactionWriter : WriterBase
    {
        public TransactionWriter(BinaryWriter writer) : base(writer)
        {
        }

        public TransactionWriter(Stream stream) : base(stream)
        {
        }

        public void Write(Transaction transaction)
        {
            Write(transaction.Version, 4);
            Write(transaction.Inputs);
            Write(transaction.Outputs);
            Write(transaction.LockTime, 4);
        }

        void Write(ICollection<TransactionInput> inputs)
        {
            WriteVar((ulong) inputs.Count);
            foreach (var input in inputs)
            {
                Write(input);
            }
        }

        void Write(TransactionInput input)
        {
            WriteReverse(input.PreviousTransaction);
            Write(input.PreviousIndex, 4);
            Write(input.ScriptSig);
            Write(input.Sequence, 4);
        }

        void Write(ICollection<TransactionOutput> outputs)
        {
            WriteVar(outputs.Count);
            foreach (var output in outputs)
            {
                Write(output);
            }
        }

        void Write(TransactionOutput output)
        {
            Write(output.Amount, 8);
            Write(output.ScriptPubKey);
        }

        void Write(ScriptSig scriptSig)
        {
            WriteVarBytes(scriptSig.Bytes);
        }

        void Write(ScriptPubKey scriptPubKey)
        {
            WriteVarBytes(scriptPubKey.Bytes);
        }
    }
}
