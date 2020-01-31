﻿using System;

namespace BitcoinBook
{
    public class TransactionInput
    {
        public byte[] PreviousTransaction { get; }
        public int PreviousIndex { get; }
        public ScriptSig ScriptSig { get; }
        public uint Sequence { get; }

        public TransactionInput(byte[] previousTransaction, int previousIndex, ScriptSig scriptSig, uint sequence)
        {
            PreviousTransaction = previousTransaction ?? throw new ArgumentNullException(nameof(previousIndex));
            PreviousIndex = previousIndex;
            ScriptSig = scriptSig ?? new ScriptSig();
            Sequence = sequence;
        }

        public override string ToString()
        {
            return $"{Cipher.ToHex(PreviousTransaction)}:{PreviousIndex}";
        }
    }
}