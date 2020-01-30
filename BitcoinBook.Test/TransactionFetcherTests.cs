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
        const string baseUri = "http://song";
        const string transactionId = "transactionId";

        readonly Mock<FakeHttpMessageHandler> mock = new Mock<FakeHttpMessageHandler>() { CallBase = true };
        readonly HttpClient httpClient;

        public TransactionFetcherTests()
        {
            httpClient = new HttpClient(mock.Object) { BaseAddress = new Uri(baseUri) };
        }

        [Fact]
        public async Task FetchNotFoundTest()
        {
            ExspectStatusCode(transactionId, HttpStatusCode.NotFound);
            Assert.Null(await new TransactionFetcher(httpClient).Fetch(transactionId));
        }

        [Fact]
        public async Task FetchBadRequestTest()
        {
            ExspectStatusCode(transactionId, HttpStatusCode.BadRequest);
            await Assert.ThrowsAsync<FetchException>(async () => await new TransactionFetcher(httpClient).Fetch(transactionId));
        }

        void ExspectStatusCode(string id, HttpStatusCode statusCode)
        {
            var uri = new Uri($"{baseUri}/tx/{id}.hex");
            mock.Setup(h => h.Send(It.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get &&
                m.RequestUri == uri))).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
            });
        }
    }
}
