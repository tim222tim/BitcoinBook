using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public class TransactionVerifier
    {
        readonly ITransactionFetcher fetcher;

        readonly ScriptEvaluator evaluator = new ScriptEvaluator();
        readonly FeeCalculator feeCalculator;
        readonly TransactionHasher hasher;

        public TransactionVerifier(ITransactionFetcher fetcher)
        {
            this.fetcher = fetcher;
            feeCalculator = new FeeCalculator(fetcher);
            hasher = new TransactionHasher(fetcher);
        }

        public async Task<bool> Verify(Transaction transaction)
        {
            try
            {
                if (await feeCalculator.CalculateFeesAsync(transaction) < 0)
                {
                    return false;
                }

                return await transaction.Inputs.Select(i => Verify(transaction, i)).All();
            }
            catch (FetchException)
            {
                return false;
            }
        }

        public async Task<bool> Verify(Transaction transaction, int inputIndex)
        {
            CheckInputIndex(transaction, inputIndex);
            return await Verify(transaction, transaction.Inputs[inputIndex]);
        }

        async Task<bool> Verify(Transaction transaction, TransactionInput input)
        {
            var priorOutput = await fetcher.GetPriorOutput(input);
            var script = new Script(input.SigScript, priorOutput.ScriptPubKey);
            var sigHash = hasher.ComputeSigHash(transaction, input, priorOutput, SigHashType.All);

            return evaluator.Evaluate(script.Commands, sigHash);
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
