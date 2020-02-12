using System;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using Xunit;

namespace BitcoinBook.Test
{
    public class TransactionVerifierTests
    {
        readonly TransactionFetcher fetcher = new TransactionFetcher(new HttpClient { BaseAddress = new Uri("http://mainnet.programmingbitcoin.com") });

        readonly Transaction transaction = new TransactionReader(rawTransaction).ReadTransaction();
        readonly TransactionVerifier verifier;

        public TransactionVerifierTests()
        {
            verifier = new TransactionVerifier(fetcher);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void ShouldThrowWhenInputOutOfRange(int inputIndex)
        {
            Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await verifier.ComputeSigHash(transaction, inputIndex));
        }

        [Fact]
        public async Task ComputeSigHashTest()
        {
            var sigHash = await verifier.ComputeSigHash(transaction, 0);
            Assert.Equal(BigInteger.Parse("63326093402361683501375285352199124474471992799478479955283731889684039098489"), sigHash.ToBigInteger());
            Assert.Equal("8c014c7778702072e30f15efd6884510f9cc04d3ac988aa16e024230f1586079", sigHash.ToHex());
        }

        [Fact]
        public async Task VerifyInputTest()
        {
            Assert.True(await verifier.Verify(transaction, 0));
        }

        [Fact]
        public async Task VerifyAnotherInputTest()
        {
            var trans = await fetcher.Fetch("ef24f67c2ce44fc89718654c642bcb401dcf441f6ef7c7132413c3c2a818faea");
            Assert.NotNull(trans);
            Assert.True(await verifier.Verify(trans, 0));
        }

        [Fact]
        public async Task VerifyTest()
        {
            Assert.True(await verifier.Verify(transaction));
        }

        [Fact]
        public async Task VerifyAnotherTest()
        {
            var trans = await fetcher.Fetch("ef24f67c2ce44fc89718654c642bcb401dcf441f6ef7c7132413c3c2a818faea");
            Assert.NotNull(trans);
            Assert.True(await verifier.Verify(trans));
        }

        const string rawTransaction =
            "0100000001be2ae9a2a3d91cdbbc53aca340a0837416519b129f627239a7dcf7fd069ae074000000006a473044022035a874a246f4de3570295fa8e32ca48a3eb1cf3a4bea6cbea6d18f122f2da51a02204ee9fe995e4934445d381be89b5635bf16bcf9bd023d81e5dc54991d7124921101210279fc02b440c755d18e80add59b5f1ec9452ab8348e75ced61e47c0750408e028feffffff042ec00900000000001976a914664403549f16e3ded11e2a5049e7837269d129ed88acf8e20100000000001976a9149a0093bfcbd8cb1162fd3c3355c6a8fddba2df6488ac5512fb12000000001976a9141a225100a114159cd16dc22650bf39043ba5eaca88ac8f990f00000000001976a914888a6f53d62ea69aabd94f59e8356fc73ad1f37f88ac2dc70600";
    }
}
