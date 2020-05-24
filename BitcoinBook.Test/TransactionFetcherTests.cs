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
        const string baseUri = "http://song/api/";
        const string transactionId = "4ea6e2222c4d59dea646e21a103d8b812a6db433f8ca331778a9408990fa17ee";

        readonly Mock<FakeHttpMessageHandler> mock = new Mock<FakeHttpMessageHandler>() { CallBase = true };
        readonly TransactionFetcher fetcher;

        public TransactionFetcherTests()
        {
            var httpClient = new HttpClient(mock.Object) { BaseAddress = new Uri(baseUri) };
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
            Assert.Equal(1, transaction.Version);
            Assert.Single(transaction.Inputs);
            Assert.Equal("74e09a06fdf7dca73972629f129b51167483a040a3ac53bcdb1cd9a3a2e92abe", 
                transaction.Inputs[0].PreviousTransaction.ToHex());
            Assert.Equal(0, transaction.Inputs[0].PreviousIndex);
            Assert.Equal(4, transaction.Outputs.Count);
            Assert.Equal(000639022, transaction.Outputs[0].Amount);
            Assert.Equal(000123640, transaction.Outputs[1].Amount);
            Assert.Equal(318444117, transaction.Outputs[2].Amount);
            Assert.Equal(001022351, transaction.Outputs[3].Amount);
        }

        [Fact]
        public async Task FetchFresh()
        {
            ExpectContent(transactionId, rawTransaction);
            var transaction = await fetcher.Fetch(transactionId);
            Assert.NotSame(transaction, await fetcher.Fetch(transactionId, true));
        }

        [Fact]
        public async Task FetchForRealTest()
        {
            var transaction = await IntegrationSetup.Mainnet.Fetcher.Fetch("0683c48ed57aad50b3c611366d522b11830c58f069de33bf5ceca7cafd44d98c");
            Assert.Single(transaction.Inputs);
            Assert.Equal(2, transaction.Outputs.Count);
            Assert.Equal(0005897938, transaction.Outputs[0].Amount);
        }

        [Fact]
        public async Task FetchOutputForRealTest()
        {
            var output = await IntegrationSetup.Mainnet.Fetcher.FetchOutput(new OutputPoint(Cipher.ToBytes("0683c48ed57aad50b3c611366d522b11830c58f069de33bf5ceca7cafd44d98c"), 0));
            Assert.Equal(0005897938, output.Amount);
        }

        [Fact]
        public async Task FetchOutputPointForRealTest()
        {
            var output = await IntegrationSetup.Mainnet.Fetcher.FetchOutput("0683c48ed57aad50b3c611366d522b11830c58f069de33bf5ceca7cafd44d98c:0");
            Assert.Equal(0005897938, output.Amount);
        }

        [Fact]
        public async Task FetchOutputPointsForRealTest()
        {
            var outputs = await IntegrationSetup.Mainnet.Fetcher.FetchOutputs(new [] { "0683c48ed57aad50b3c611366d522b11830c58f069de33bf5ceca7cafd44d98c:0", "0683c48ed57aad50b3c611366d522b11830c58f069de33bf5ceca7cafd44d98c:1" });
            Assert.Equal(2, outputs.Length);
            Assert.Equal(0005897938, outputs[0].Amount);
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
            var uri = new Uri($"{baseUri}tx/{id}/hex");
            mock.Setup(h => h.Send(It.Is<HttpRequestMessage>(m => 
                m.Method == HttpMethod.Get && m.RequestUri == uri))).Returns(message);
        }

        const string rawTransaction =
            "0100000001be2ae9a2a3d91cdbbc53aca340a0837416519b129f627239a7dcf7fd069ae074000000006a473044022035a874a246f4de3570295fa8e32ca48a3eb1cf3a4bea6cbea6d18f122f2da51a02204ee9fe995e4934445d381be89b5635bf16bcf9bd023d81e5dc54991d7124921101210279fc02b440c755d18e80add59b5f1ec9452ab8348e75ced61e47c0750408e028feffffff042ec00900000000001976a914664403549f16e3ded11e2a5049e7837269d129ed88acf8e20100000000001976a9149a0093bfcbd8cb1162fd3c3355c6a8fddba2df6488ac5512fb12000000001976a9141a225100a114159cd16dc22650bf39043ba5eaca88ac8f990f00000000001976a914888a6f53d62ea69aabd94f59e8356fc73ad1f37f88ac2dc70600";
    }
}
