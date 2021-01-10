using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace BitcoinBook.Test
{
    public class PayToPubKeySignerTests
    {
        readonly Mock<ITransactionFetcher> mockFetcher = new();
        readonly PrivateKey privateKey = new(8732874329871);
        readonly Transaction transaction;
        readonly TransactionInput input;
        readonly IInputSigner signer;

        public PayToPubKeySignerTests()
        {
            var previousId = new BigInteger(12345).ToLittleBytes();
            input = new TransactionInput(previousId, 0, new Script(), new Script(), 0);
            transaction = new Transaction(1, true,
                new[] { input },
                new[] { new TransactionOutput(100, new Script()), },
                0);
            var previousOutput = new TransactionOutput(2, StandardScripts.PayToPubKey(privateKey.PublicKey));

            mockFetcher.Setup(f => f.FetchPriorOutput(It.Is<TransactionInput>(i =>
                i.PreviousTransaction.SequenceEqual(input.PreviousTransaction) &&
                i.PreviousIndex == input.PreviousIndex))).Returns(Task.FromResult(previousOutput));
            signer = new PayToPubKeySigner(mockFetcher.Object, new TransactionHasher(mockFetcher.Object));
        }

        [Fact]
        public async Task SignWithKeyTest()
        {
            var sigScript = await signer.CreateSigScript(privateKey, transaction, input, SigHashType.All);
            await AssertSigScript(sigScript);
        }

        [Fact]
        public async Task SignWithWalletTest()
        {
            var wallet = new Wallet(new [] {new PrivateKey(8732873784), privateKey, new PrivateKey(9823498743), });
            var sigScript = await signer.CreateSigScript(wallet, transaction, input, SigHashType.All);
            await AssertSigScript(sigScript);
        }

        [Fact]
        public async Task SignWithWrongWalletTest()
        {
            var wallet = new Wallet(new[] { new PrivateKey(8732873784), new PrivateKey(9823498743), });
            await Assert.ThrowsAsync<PrivateKeyNotFoundException>(async () => await signer.CreateSigScript(wallet, transaction, input, SigHashType.All));
        }

        async Task AssertSigScript(Script sigScript)
        {
            var signedTransaction = transaction.CloneWithReplacedSigScript(input, sigScript);
            var verifier = new TransactionVerifier(mockFetcher.Object);
            Assert.True(await verifier.Verify(signedTransaction, 0));
        }
    }
}
