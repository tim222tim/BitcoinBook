using System;

namespace BitcoinBook
{
    public class TransactionInput : ICloneable
    {
        public byte[] PreviousTransaction { get; }
        public int PreviousIndex { get; }
        public Script SigScript { get; }
        public Script Witness { get; }
        public uint Sequence { get; }

        public TransactionInput(byte[] previousTransaction, int previousIndex, Script sigScript, Script witness, uint sequence)
        {
            PreviousTransaction = previousTransaction ?? throw new ArgumentNullException(nameof(previousIndex));
            PreviousIndex = previousIndex;
            SigScript = sigScript ?? throw new ArgumentNullException(nameof(sigScript));
            Witness = witness ?? throw new ArgumentNullException(nameof(witness));
            Sequence = sequence;
        }

        public override string ToString()
        {
            return $"{PreviousTransaction.ToHex()}:{PreviousIndex}";
        }

        public TransactionInput Clone()
        {
            return new TransactionInput(PreviousTransaction, PreviousIndex, SigScript.Clone(), Witness.Clone(), Sequence);
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
            return new TransactionInput(PreviousTransaction, PreviousIndex, script, Witness, Sequence);
        }

        public TransactionInput CloneWithWitness(Script witness)
        {
            return new TransactionInput(PreviousTransaction, PreviousIndex, SigScript, witness, Sequence);
        }
    }
}
