using System;
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
            var content = await response.Content.ReadAsStringAsync();
            throw new NotImplementedException();
        }

        static string GetUrl(bool testnet = false)
        {
            return testnet ? "http://testnet.programmingbitcoin.com" : "http://testnet.programmingbitcoin.com";
        }
    }
}
