using System;

namespace BitcoinBook
{
    public class TransactionOutput
    {
        public int Amount { get; }
        public ScriptPubKey ScriptPubKey { get; }

        public TransactionOutput(int amount, ScriptPubKey scriptPubKey)
        {
            Amount = amount;
            ScriptPubKey = scriptPubKey ?? throw new ArgumentNullException(nameof(scriptPubKey));
        }
    }
}