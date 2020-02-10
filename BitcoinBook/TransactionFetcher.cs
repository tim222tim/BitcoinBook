using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public class TransactionFetcher
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
                throw new FetchException("Got wrong transaciton ID");
            }

            cache.Set(transactionId, transaction);
            return transaction;
        }
    }
}
