using System;

namespace BitcoinBook
{
    public class TransactionOutput
    {
        public long Amount { get; }
        public ScriptPubKey ScriptPubKey { get; }

        public TransactionOutput(long amount, ScriptPubKey scriptPubKey)
        {
            Amount = amount;
            ScriptPubKey = scriptPubKey ?? throw new ArgumentNullException(nameof(scriptPubKey));
        }
    }
}