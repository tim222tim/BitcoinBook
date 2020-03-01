using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public class TransactionVerifier
    {
        readonly ITransactionFetcher fetcher;
        readonly bool throwOnFailure;

        readonly ScriptEvaluator evaluator;
        readonly FeeCalculator feeCalculator;
        readonly TransactionHasher hasher;

        public TransactionVerifier(ITransactionFetcher fetcher, bool throwOnFailure = false)
        {
            this.fetcher = fetcher;
            this.throwOnFailure = throwOnFailure;
            feeCalculator = new FeeCalculator(fetcher);
            hasher = new TransactionHasher(fetcher);
            evaluator = new ScriptEvaluator(throwOnFailure);
        }

        public async Task<bool> Verify(Transaction transaction)
        {
            try
            {
                return await InternalVerify(transaction);
            }
            catch (FetchException ex)
            {
                if (throwOnFailure)
                {
                    throw new VerificationException("Unable to fetch prior transaction", ex);
                }

                return false;
            }
            catch (VerificationException)
            {
                if (throwOnFailure)
                {
                    throw;
                }

                return false;
            }
        }

        async Task<bool> InternalVerify(Transaction transaction)
        {
            if (await feeCalculator.CalculateFeesAsync(transaction) < 0)
            {
                throw new VerificationException("Inputs are less than outputs");
            }

            return await transaction.Inputs.Select(i => InternalVerify(transaction, i)).All();
        }

        public async Task<bool> Verify(Transaction transaction, int inputIndex)
        {
            CheckInputIndex(transaction, inputIndex);
            try
            {
                return await InternalVerify(transaction, transaction.Inputs[inputIndex]);
            }
            catch (FetchException ex)
            {
                if (throwOnFailure)
                {
                    throw new VerificationException("Unable to fetch prior transaction", ex);
                }

                return false;
            }
            catch (VerificationException)
            {
                if (throwOnFailure)
                {
                    throw;
                }

                return false;
            }
        }

        async Task<bool> InternalVerify(Transaction transaction, TransactionInput input)
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
