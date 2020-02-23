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

        public async Task<byte[]> ComputeSigHash(Transaction transaction, int inputIndex, SigHashType sigHashType)
        {
            CheckInputIndex(transaction, inputIndex);
            return await ComputeSigHash(transaction, transaction.Inputs[inputIndex], sigHashType);
        }

        public async Task<byte[]> ComputeSigHash(Transaction transaction, TransactionInput input, SigHashType sigHashType)
        {
            var priorOutput = await fetcher.GetPriorOutput(input);
            return ComputeSigHash(transaction, input, priorOutput, sigHashType);
        }

        public byte[] ComputeSigHash(Transaction transaction, TransactionInput input, TransactionOutput priorOutput, SigHashType sigHashType)
        {
            if (sigHashType != SigHashType.All)
            {
                throw new NotImplementedException("Only supports SigHashType.All");
            }
            transaction = transaction.CloneWithReplacedSigScript(input, priorOutput.ScriptPubKey);
            var stream = new MemoryStream();
            var writer = new TransactionWriter(stream);
            writer.Write(transaction);
            writer.Write((int)sigHashType, 4);

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
