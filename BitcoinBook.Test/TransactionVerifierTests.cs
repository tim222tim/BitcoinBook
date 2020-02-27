using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BitcoinBook.Test
{
    public class TransactionVerifierTests
    {
        const string rawTransaction =
            "0100000001be2ae9a2a3d91cdbbc53aca340a0837416519b129f627239a7dcf7fd069ae074000000006a473044022035a874a246f4de3570295fa8e32ca48a3eb1cf3a4bea6cbea6d18f122f2da51a02204ee9fe995e4934445d381be89b5635bf16bcf9bd023d81e5dc54991d7124921101210279fc02b440c755d18e80add59b5f1ec9452ab8348e75ced61e47c0750408e028feffffff042ec00900000000001976a914664403549f16e3ded11e2a5049e7837269d129ed88acf8e20100000000001976a9149a0093bfcbd8cb1162fd3c3355c6a8fddba2df6488ac5512fb12000000001976a9141a225100a114159cd16dc22650bf39043ba5eaca88ac8f990f00000000001976a914888a6f53d62ea69aabd94f59e8356fc73ad1f37f88ac2dc70600";

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
            Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await verifier.Verify(transaction, inputIndex));
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

        [Theory]
        [InlineData("ef24f67c2ce44fc89718654c642bcb401dcf441f6ef7c7132413c3c2a818faea")]
        [InlineData("874d0ad92528d8cb6cd9fa449cc6ef1f38a84fd23426b359e5a8e51ddf47892f")]
        public async Task VerifyOnline(string transactionId)
        {
            var trans = await fetcher.Fetch(transactionId);
            Assert.NotNull(trans);
            Assert.True(await verifier.Verify(trans));
        }

        [Fact]
        public async Task VerifyFalseWhenNegativeFees()
        {
            var trans = await fetcher.Fetch("ef24f67c2ce44fc89718654c642bcb401dcf441f6ef7c7132413c3c2a818faea");
            trans.Outputs[0] = new TransactionOutput(trans.Outputs[0].Amount + 100, trans.Outputs[0].ScriptPubKey);
            Assert.False(await verifier.Verify(trans));
        }

        [Fact]
        public async Task VerifyFalseWhenBadSig()
        {
            var trans = await fetcher.Fetch("ef24f67c2ce44fc89718654c642bcb401dcf441f6ef7c7132413c3c2a818faea");
            ((byte[])trans.Inputs[0].SigScript.Commands[0])[0] += 1;
            Assert.False(await verifier.Verify(trans));
        }

        [Fact]
        public async Task VerifyFalseWhenBadSecondSig()
        {
            var trans = await fetcher.Fetch("874d0ad92528d8cb6cd9fa449cc6ef1f38a84fd23426b359e5a8e51ddf47892f");
            ((byte[])trans.Inputs[1].SigScript.Commands[0])[0] += 1;
            Assert.False(await verifier.Verify(trans));
        }

        [Fact]
        public async Task VerifyFalseWhenBadPrevious()
        {
            var trans = await fetcher.Fetch("ef24f67c2ce44fc89718654c642bcb401dcf441f6ef7c7132413c3c2a818faea");
            var input = trans.Inputs[0];
            trans.Inputs[0] = new TransactionInput(input.PreviousTransaction, 999, input.SigScript, new Script(), input.Sequence);
            Assert.False(await verifier.Verify(trans));
        }
    }
}
