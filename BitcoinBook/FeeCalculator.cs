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
            var priorOutputs = await fetcher.GetPriorOutputs(transaction.Inputs);
            var inputAmounts = priorOutputs.Sum(o => o.Amount);
            var outputAmounts = transaction.Outputs.Sum(o => o.Amount);
            return inputAmounts - outputAmounts;
        }
    }
}
