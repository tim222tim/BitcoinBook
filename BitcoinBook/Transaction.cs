using System;
using System.Collections.Generic;
using System.IO;

namespace BitcoinBook
{
    public class Transaction
    {
        public string Id { get; }
        public int Version { get; }
        public IList<TransactionInput> Inputs { get; }
        public IList<TransactionOutput> Outputs { get; }
        public uint LockTime { get; }
        public bool Testnet { get; }

        public Transaction(int version, IList<TransactionInput> inputs, IList<TransactionOutput> outputs, uint lockTime, bool testnet = false)
        {
            Version = version;
            Inputs = inputs ?? throw new ArgumentNullException(nameof(inputs));
            Outputs = outputs ?? throw new ArgumentNullException(nameof(outputs));
            LockTime = lockTime;
            Testnet = testnet;
            Id = ComputeId();
        }

        string ComputeId()
        {
            var stream = new MemoryStream();
            var writer = new TransactionWriter(stream);
            writer.Write(this);
            var hash = Cipher.Hash256(stream.ToArray());
            Array.Reverse(hash);
            return Cipher.ToHex(hash);
        }
    }
}
