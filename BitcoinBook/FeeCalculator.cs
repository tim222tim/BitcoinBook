using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public class FeeCalculator
    {
        readonly ITransactionFetcher fetcher;

        public FeeCalculator(ITransactionFetcher fetcher)
        {
            this.fetcher = fetcher;
        }

        public async Task<long> CalculateFeesAsync(Transaction transaction)
        {
            var priorOutputs = await GetPriorOutputs(transaction.Inputs);
            var inputAmounts = priorOutputs.Sum(o => o.Amount);
            var outputAmounts = transaction.Outputs.Sum(o => o.Amount);
            return inputAmounts - outputAmounts;
        }

        async Task<TransactionOutput[]> GetPriorOutputs(IEnumerable<TransactionInput> inputs)
        {
            var priorTasks = inputs.Select(async i => await GetPriorOutput(i));
            return await Task.WhenAll(priorTasks);
        }

        async Task<TransactionOutput> GetPriorOutput(TransactionInput input)
        {
            return await GetOutput(Cipher.ToHex(input.PreviousTransaction), input.PreviousIndex);
        }

        async Task<TransactionOutput> GetOutput(string transactionId, int index)
        {
            var transaction = await fetcher.Fetch(transactionId) ??
                             throw new FetchException($"Transaction {transactionId} not found");

            return transaction.Outputs.Count > index
                ? transaction.Outputs[index]
                : throw new FetchException($"Transaction output {transactionId}:{index} does not exist");
        }
    }
}
