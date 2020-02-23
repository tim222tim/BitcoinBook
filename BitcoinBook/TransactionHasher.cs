using System;
using System.IO;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public class TransactionHasher
    {
        readonly ITransactionFetcher fetcher;

        public TransactionHasher(ITransactionFetcher fetcher)
        {
            this.fetcher = fetcher;
        }

        public async Task<byte[]> ComputeSigHash(Transaction transaction, int inputIndex)
        {
            CheckInputIndex(transaction, inputIndex);
            return await ComputeSigHash(transaction, transaction.Inputs[inputIndex]);
        }

        public async Task<byte[]> ComputeSigHash(Transaction transaction, TransactionInput input)
        {
            var priorOutput = await fetcher.GetPriorOutput(input);
            return ComputeSigHash(transaction, input, priorOutput);
        }

        public byte[] ComputeSigHash(Transaction transaction, TransactionInput input, TransactionOutput priorOutput)
        {
            transaction = transaction.CloneWithReplacedSigScript(input, priorOutput.ScriptPubKey);
            var stream = new MemoryStream();
            var writer = new TransactionWriter(stream);
            writer.Write(transaction);
            writer.Write((int)SigHashType.All, 4);

            return Cipher.Hash256(stream.ToArray());
        }

        void CheckInputIndex(Transaction transaction, int inputIndex)
        {
            if (inputIndex < 0 || inputIndex >= transaction.Inputs.Count)
            {
                throw new IndexOutOfRangeException(
                    $"Input index {inputIndex} is invalid for transaction with {transaction.Inputs.Count} inputs");
            }
        }
    }
}
