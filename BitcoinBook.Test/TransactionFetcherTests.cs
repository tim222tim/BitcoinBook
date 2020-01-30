using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace BitcoinBook.Test
{
    public class TransactionFetcherTests
    {
        [Fact]
        public async Task TryItOnce()
        {
            var mockHandler = new Mock<FakeHttpMessageHandler>() {CallBase = true};
            mockHandler.Setup(h => h.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("here we go!")
            });
            await new TransactionFetcher(new HttpClient(mockHandler.Object){BaseAddress = new Uri("http://song")}).Fetch("x");
        }
    }
}
