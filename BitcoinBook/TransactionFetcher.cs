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

        public async Task<Transaction> Fetch(string transactionId, bool fresh = false)
        {
            return (fresh ? null : cache.Get<Transaction>(transactionId)) ?? await InternalFetch(transactionId);
        }

        public Task<Transaction> Fetch(byte[] transactionId, bool fresh = false)
        {
            return Fetch(transactionId.ToHex(), fresh);
        }

        public async Task<TransactionOutput[]> GetPriorOutputs(IEnumerable<TransactionInput> inputs)
        {
            var priorTasks = inputs.Select(async i => await GetPriorOutput(i));
            return await Task.WhenAll(priorTasks);
        }

        public async Task<TransactionOutput> GetPriorOutput(TransactionInput input)
        {
            return await GetOutput(input.PreviousTransaction, input.PreviousIndex);
        }

        public async Task<TransactionOutput> GetOutput(byte[] transactionId, int index)
        {
            var transaction = await Fetch(transactionId) ??
                              throw new FetchException($"Transaction {transactionId} not found");

            return transaction.Outputs.Count > index
                ? transaction.Outputs[index]
                : throw new FetchException($"Transaction output {transactionId}:{index} does not exist");
        }

        async Task<Transaction> InternalFetch(string transactionId)
        {
            var response = await httpClient.GetAsync($"/tx/{transactionId}.hex");
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
            if (hex.Substring(8, 2) == "00")
            {
                hex = hex.Substring(0, 8) + hex.Substring(12); // cut out two bytes?
            }

            var transaction = new TransactionReader(hex).ReadTransaction();
            if (transaction.Id != transactionId)
            {
                throw new FetchException("Got wrong transaction ID");
            }

            cache.Set(transactionId, transaction);
            return transaction;
        }
    }
}
