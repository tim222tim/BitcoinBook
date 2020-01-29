using System;
using System.Collections.Generic;

namespace BitcoinBook
{
    public class Transaction
    {
        public uint Version { get; }
        public IList<TransactionInput> Inputs { get; }
        public IList<TransactionOutput> Outputs { get; }
        public uint LockTime { get; }
        public bool Testnet { get; }

        public Transaction(uint version, IList<TransactionInput> inputs, IList<TransactionOutput> outputs, uint lockTime, bool testnet = false)
        {
            Version = version;
            Inputs = inputs ?? throw new ArgumentNullException(nameof(inputs));
            Outputs = outputs ?? throw new ArgumentNullException(nameof(outputs));
            LockTime = lockTime;
            Testnet = testnet;
        }
    }
}
