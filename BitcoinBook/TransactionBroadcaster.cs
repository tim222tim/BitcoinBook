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

        public async Task Broadcast(Transaction transaction)
        {
            var stream = new MemoryStream();
            var writer = new TransactionWriter(stream);
            writer.Write(transaction);
            var hex = stream.ToArray().ToHex();

            var response = await httpClient.PutAsync("/btc/pushtx/", new StringContent(hex));
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new BroadcastException($"Received {response.StatusCode} broadcasting transaction");
            }
        }
    }
}