using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public class TransactionBroadcaster
    {
        readonly HttpClient httpClient;

        public TransactionBroadcaster(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        // This broadcaster does not work -- not an API
        public async Task Broadcast(Transaction transaction)
        {
            await httpClient.GetAsync("/btc-testnet/pushtx/");
            var response = await httpClient.PutAsync("/btc-testnet/pushtx/", new StringContent(transaction.ToHex()));
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new BroadcastException($"Received {response.StatusCode} broadcasting transaction. Message: " + await response.Content.ReadAsStringAsync());
            }
        }
    }
}