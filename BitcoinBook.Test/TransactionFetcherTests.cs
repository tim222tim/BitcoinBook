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
        const string transactionId = "9489189d92d17860ff7cab2adf5a41433caa1a6f73aaa2f0139489847b5c2451";

        readonly Mock<FakeHttpMessageHandler> mock = new Mock<FakeHttpMessageHandler>() { CallBase = true };
        readonly HttpClient httpClient;
        readonly TransactionFetcher fetcher;

        public TransactionFetcherTests()
        {
            httpClient = new HttpClient(mock.Object) { BaseAddress = new Uri(baseUri) };
            fetcher = new TransactionFetcher(httpClient);
        }

        [Fact]
        public async Task FetchNotFoundTest()
        {
            ExpectStatusCode(transactionId, HttpStatusCode.NotFound);
            Assert.Null(await fetcher.Fetch(transactionId));
        }

        [Fact]
        public async Task FetchBadRequestTest()
        {
            ExpectStatusCode(transactionId, HttpStatusCode.BadRequest);
            await Assert.ThrowsAsync<FetchException>(async () => await fetcher.Fetch(transactionId));
        }

        [Fact]
        public async Task FetchGoodTest()
        {
            ExpectContent(transactionId, rawTransaction);
            var transaction = await fetcher.Fetch(transactionId);
            Assert.NotNull(transaction);
        }

        void ExpectStatusCode(string id, HttpStatusCode statusCode)
        {
            ExpectResponse(id, new HttpResponseMessage
            {
                StatusCode = statusCode,
            });
        }

        void ExpectContent(string id, string content)
        {
            ExpectResponse(id, new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content)
            });
        }

        void ExpectResponse(string id, HttpResponseMessage message)
        {
            var uri = new Uri($"{baseUri}/tx/{id}.hex");
            mock.Setup(h => h.Send(It.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get &&
                                                                  m.RequestUri == uri))).Returns(message);
        }

        const string rawTransaction =
            "0100000000010158f85521054b0ef5009046c01440f45a49851c613e4544a9c43e877511dbd96e0100000000ffffffff0220d06d06000000001976a9146c0e16d43b03f94c4673373b8fb8547eb4ff543588ac40fd260100000000220020701a8d401c84fb13e6baf169d59684e17abd9fa216c8cc5b9fc63d622ff8c58d0400473044022076d197b976513eafb7f518c922bfb5e7a5340570ede97141f230db68d352119802202aae5a5161b3fb6e73f13e919f9266bbbb14d13729a4c80ccc99c30bc16d8e2c0147304402200d0080177dc637521e57a7e24d1041f413c1461870db350765fcc86c9b7165a00220556957fb0f3b1bbf46397751ede7dc69bf6cd3358a7dd126c6f9125cd86a7069016952210375e00eb72e29da82b89367947f29ef34afb75e8654f6ea368e0acdfd92976b7c2103a1b26313f430c4b15bb1fdce663207659d8cac749a0e53d70eff01874496feff2103c96d495bfdd5ba4145e3e046fee45e84a8a48ad05bd8dbb395c011a32cf9f88053ae00000000";
    }
}
