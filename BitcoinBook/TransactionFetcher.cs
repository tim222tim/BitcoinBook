using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public class TransactionFetcher : ITransactionFetcher
    {
        readonly HttpClient httpClient;
        readonly IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

        public TransactionFetcher(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Transaction?> Fetch(string transactionId, bool fresh = false)
        {
            return (fresh ? null : cache.Get<Transaction>(transactionId))?.Clone() ?? await InternalFetch(transactionId);
        }

        public Task<Transaction?> Fetch(byte[] transactionId, bool fresh = false)
        {
            return Fetch(transactionId.ToHex(), fresh);
        }

        public async Task<TransactionOutput[]> FetchPriorOutputs(IEnumerable<TransactionInput> inputs)
        {
            var priorTasks = inputs.Select(async i => await FetchPriorOutput(i));
            return await Task.WhenAll(priorTasks);
        }

        public async Task<TransactionOutput> FetchPriorOutput(TransactionInput input)
        {
            return await FetchOutput(new OutputPoint(input.PreviousTransaction, input.PreviousIndex));
        }

        public async Task<TransactionOutput[]> FetchOutputs(IEnumerable<OutputPoint> outputPoints)
        {
            var priorTasks = outputPoints.Select(async p => await FetchOutput(p));
            return await Task.WhenAll(priorTasks);
        }

        public async Task<TransactionOutput[]> FetchOutputs(IEnumerable<string> outputPoints)
        {
            var priorTasks = outputPoints.Select(async p => await FetchOutput(p));
            return await Task.WhenAll(priorTasks);
        }

        public async Task<TransactionOutput> FetchOutput(string outputPoint)
        {
            return await FetchOutput(new OutputPoint(outputPoint));
        }

        public async Task<TransactionOutput> FetchOutput(OutputPoint outputPoint)
        {
            var transactionId = outputPoint.TransactionId;
            var index = outputPoint.Index;
            var transaction = await Fetch(transactionId) ??
                              throw new FetchException($"Transaction {transactionId} not found");

            return transaction.Outputs.Count > index
                ? transaction.Outputs[index]
                : throw new FetchException($"Transaction output {transactionId.ToHex()}:{index} does not exist");
        }

        async Task<Transaction?> InternalFetch(string transactionId)
        {
            var response = await httpClient.GetAsync($"tx/{transactionId}/hex");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                cache.Remove(transactionId);
                return null;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new FetchException($"Received {response.StatusCode} fetching transaction");
            }

            var hex = await response.Content.ReadAsStringAsync();
            hex = hex.Trim();

            var transaction = new TransactionReader(hex).ReadTransaction();
            if (transaction.Id != transactionId)
            {
                throw new FetchException("Got wrong transaction ID");
            }

            cache.Set(transactionId, transaction);
            return transaction.Clone();
        }
    }
}
