using System;

namespace BitcoinBook
{
    public class TransactionInput
    {
        public byte[] PreviousTransaction { get; }
        public int PreviousIndex { get; }
        public Script SigScript { get; }
        public uint Sequence { get; }

        public TransactionInput(byte[] previousTransaction, int previousIndex, Script scriptSig, uint sequence)
        {
            PreviousTransaction = previousTransaction ?? throw new ArgumentNullException(nameof(previousIndex));
            PreviousIndex = previousIndex;
            SigScript = scriptSig ?? new Script();
            Sequence = sequence;
        }

        public override string ToString()
        {
            return $"{Cipher.ToHex(PreviousTransaction)}:{PreviousIndex}";
        }
    }
}
