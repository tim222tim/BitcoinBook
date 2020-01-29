using System;

namespace BitcoinBook
{
    public class TransactionOutput
    {
        public ulong Amount { get; }
        public ScriptPubKey ScriptPubKey { get; }

        public TransactionOutput(ulong amount, ScriptPubKey scriptPubKey)
        {
            Amount = amount;
            ScriptPubKey = scriptPubKey ?? throw new ArgumentNullException(nameof(scriptPubKey));
        }
    }
}