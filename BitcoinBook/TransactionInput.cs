using System;

namespace BitcoinBook
{
    public class TransactionInput : ICloneable
    {
        public byte[] PreviousTransaction { get; }
        public int PreviousIndex { get; }
        public Script SigScript { get; }
        public uint Sequence { get; }

        public TransactionInput(byte[] previousTransaction, int previousIndex, Script sigScript, uint sequence)
        {
            PreviousTransaction = previousTransaction ?? throw new ArgumentNullException(nameof(previousIndex));
            PreviousIndex = previousIndex;
            SigScript = sigScript ?? throw new ArgumentNullException(nameof(sigScript));
            Sequence = sequence;
        }

        public override string ToString()
        {
            return $"{Cipher.ToHex(PreviousTransaction)}:{PreviousIndex}";
        }

        public TransactionInput Clone()
        {
            return new TransactionInput(PreviousTransaction, PreviousIndex, SigScript.Clone(), Sequence);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public TransactionInput CloneWithoutSigScript()
        {
            return CloneWithSigScript(new Script());
        }

        public TransactionInput CloneWithSigScript(Script script)
        {
            return new TransactionInput(PreviousTransaction, PreviousIndex, script, Sequence);
        }
    }
}
