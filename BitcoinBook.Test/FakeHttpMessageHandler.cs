using System.Net.Http;
using System.Threading.Tasks;

namespace BitcoinBook.Test
{
    public abstract class FakeHttpMessageHandler : HttpMessageHandler
    {
        public abstract HttpResponseMessage Send(HttpRequestMessage request);

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }
    }
}
