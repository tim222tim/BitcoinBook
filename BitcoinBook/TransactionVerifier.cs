using System;

namespace BitcoinBook
{
    public class TransactionVerifier
    {
        public byte[] ComputeSigHash(Transaction transaction, int inputIndex)
        {
            if (inputIndex < 0 || inputIndex >= transaction.Inputs.Count)
            {
                throw new IndexOutOfRangeException($"Input index {inputIndex} is invalid for transaction with {transaction.Inputs.Count} inputs");
            }

            throw new NotImplementedException();
        }
    }
}
