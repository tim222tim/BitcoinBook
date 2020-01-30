using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace BitcoinBook
{
    public class TransactionWriter
    {
        readonly BinaryWriter writer;

        public TransactionWriter(BinaryWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public TransactionWriter(Stream stream) : this(new BinaryWriter(stream ?? throw new ArgumentNullException(nameof(stream))))
        {
        }

        public void Write(Transaction transaction)
        {
            Write((ulong) transaction.Version, 4);
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

        void WriteReverse(byte[] bytes)
        {
            var newBytes = (byte[])bytes.Clone();
            Array.Reverse(newBytes);
            writer.Write(newBytes);
        }

        void Write(ScriptSig scriptSig)
        {
            WriteVarBytes(scriptSig.Bytes);
        }

        void WriteVarBytes(byte[] bytes)
        {
            WriteVar((ulong) bytes.Length);
            writer.Write(bytes);
        }

        void Write(ICollection<TransactionOutput> outputs)
        {
            WriteVar((ulong)outputs.Count);
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

        void Write(ScriptPubKey scriptPubKey)
        {
            WriteVarBytes(scriptPubKey.Bytes);
        }

        public void Write(ulong i, int length)
        {
            var bytes = new BigInteger(i).ToByteArray();
            var bx = 0;
            var wx = 0;
            while (bx < bytes.Length && wx++ < length)
            {
                writer.Write(bytes[bx++]);
            }
            while (wx++ < length)
            {
                writer.Write((byte)0);
            }
        }

        public void WriteVar(ulong i)
        {
            var length = GetVarLength(i);
            if (length > 1)
            {
                writer.Write(GetVarPrefix(i));
            }
            Write(i, length);
        }

        int GetVarLength(ulong i)
        {
            return i < 0xfd ? 1 : i < 0x10000 ? 2 : i < 0x100000000 ? 4 : 8;
        }

        byte GetVarPrefix(ulong i)
        {
            return i < 0x10000 ? (byte)0xfd : i < 0x100000000 ? (byte)0xfe : (byte)0xff;
        }
    }
}
