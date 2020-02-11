using System;

namespace BitcoinBook
{
    public class TransactionOutput : ICloneable
    {
        public long Amount { get; }
        public Script ScriptPubKey { get; }

        public TransactionOutput(long amount, Script scriptPubKey)
        {
            Amount = amount;
            ScriptPubKey = scriptPubKey ?? throw new ArgumentNullException(nameof(scriptPubKey));
        }

        public TransactionOutput Clone()
        {
            return new TransactionOutput(Amount, ScriptPubKey.Clone());
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}