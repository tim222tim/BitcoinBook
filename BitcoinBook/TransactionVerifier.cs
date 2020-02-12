using System;
using System.Collections.Generic;
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

        public TransactionVerifier(ITransactionFetcher fetcher)
        {
            this.fetcher = fetcher;
            feeCalculator = new FeeCalculator(fetcher);
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
            var sigHash = ComputeSigHashInternal(transaction, input, priorOutput);

            return evaluator.Evaluate(script.Commands, sigHash);
        }

        public async Task<byte[]> ComputeSigHash(Transaction transaction, int inputIndex)
        {
            CheckInputIndex(transaction, inputIndex);
            return await ComputeSigHash(transaction, transaction.Inputs[inputIndex]);
        }

        async Task<byte[]> ComputeSigHash(Transaction transaction, TransactionInput input)
        {
            var priorOutput = await fetcher.GetPriorOutput(input);
            return ComputeSigHashInternal(transaction, input, priorOutput);
        }

        byte[] ComputeSigHashInternal(Transaction transaction, TransactionInput input, TransactionOutput priorOutput)
        {
            transaction = transaction.CloneWithReplacedSigScript(input, priorOutput.ScriptPubKey);
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
