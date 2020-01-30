using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public class TransactionFetcher
    {
        readonly HttpClient httpClient;

        public TransactionFetcher(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Transaction> Fetch(string transactionId, bool fresh = false)
        {
            var response = await httpClient.GetAsync($"/tx/{transactionId}.hex");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new FetchException($"Received {response.StatusCode} fetching transaction");
            }

            var hex = await response.Content.ReadAsStringAsync();
            return new TransactionReader(hex).ReadTransaction();
        }

        static string GetUrl(bool testnet = false)
        {
            return testnet ? "http://testnet.programmingbitcoin.com" : "http://testnet.programmingbitcoin.com";
        }
    }
}
