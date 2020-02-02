using System;

namespace BitcoinBook
{
    public class TransactionOutput
    {
        public long Amount { get; }
        public Script ScriptPubKey { get; }

        public TransactionOutput(long amount, Script scriptPubKey)
        {
            Amount = amount;
            ScriptPubKey = scriptPubKey ?? throw new ArgumentNullException(nameof(scriptPubKey));
        }
    }
}