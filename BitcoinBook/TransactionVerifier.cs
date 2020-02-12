using System;
using System.IO;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public class TransactionVerifier
    {
        readonly ITransactionFetcher fetcher;

        readonly ScriptEvaluator evaluator = new ScriptEvaluator();
        readonly FeeCalculator feeCalculator;

        public TransactionVerifier(ITransactionFetcher fetcher)
        {
            this.fetcher = fetcher;
            feeCalculator = new FeeCalculator(fetcher);
        }

        public async Task<byte[]> ComputeSigHash(Transaction transaction, int inputIndex)
        {
            CheckInputIndex(transaction, inputIndex);

            var priorOutput = await fetcher.GetPriorOutput(transaction.Inputs[inputIndex]);

            return ComputeSigHashInternal(transaction, inputIndex, priorOutput);
        }

        public async Task<bool> Verify(Transaction transaction)
        {
            if (await feeCalculator.CalculateFeesAsync(transaction) < 0)
            {
                return false;
            }

            for (int i = 0; i < transaction.Inputs.Count; i++)
            {
                if (! await Verify(transaction, i))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> Verify(Transaction transaction, int inputIndex)
        {
            CheckInputIndex(transaction, inputIndex);

            var input = transaction.Inputs[inputIndex];
            var priorOutput = await fetcher.GetPriorOutput(input);
            var script = new Script(input.SigScript, priorOutput.ScriptPubKey);
            var sigHash = ComputeSigHashInternal(transaction, inputIndex, priorOutput);

            return evaluator.Evaluate(script.Commands, sigHash);
        }

        byte[] ComputeSigHashInternal(Transaction transaction, int inputIndex, TransactionOutput priorOutput)
        {
            transaction = transaction.CloneWithReplacedSigScript(inputIndex, priorOutput.ScriptPubKey);
            var stream = new MemoryStream();
            var writer = new TransactionWriter(stream);
            writer.Write(transaction);
            writer.Write((int) SigHashType.All, 4);

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
