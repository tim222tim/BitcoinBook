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
            var tranaction = await fetcher.Fetch(Cipher.ToHex(input.PreviousTransaction)) ??
                             throw new FetchException($"Transaction {input.PreviousTransaction} not found");

            return tranaction.Outputs.Count > input.PreviousIndex ? 
                tranaction.Outputs[input.PreviousIndex] :
                throw new FetchException($"Transaction output {input.PreviousTransaction}:{input.PreviousIndex} does not exist");
        }
    }
}
