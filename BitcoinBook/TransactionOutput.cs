using System;

namespace BitcoinBook
{
    public class TransactionOutput
    {
        public long Amount { get; }
        public Script Script { get; }

        public TransactionOutput(long amount, Script script)
        {
            Amount = amount;
            Script = script ?? throw new ArgumentNullException(nameof(script));
        }
    }
}