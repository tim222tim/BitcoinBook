using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace BitcoinBook.Test
{
    public class PayToPubKeyHashSignerTests
    {
        readonly Mock<ITransactionFetcher> mockFetcher = new Mock<ITransactionFetcher>();

        [Fact]
        public async Task SignTest()
        {
            var privateKey = new PrivateKey(8732874329871);
            var previousId = new BigInteger(12345).ToLittleBytes();
            var input = new TransactionInput(previousId, 0, new Script(), 0);
            var transaction = new Transaction(1,
                new[] {input},
                new[] {new TransactionOutput(100, new Script()),},
                0);
            var previousOutput = new TransactionOutput(2, StandardScripts.PayToPublicKeyHash(privateKey.PublicKey));

            mockFetcher.Setup(f => f.GetPriorOutput(It.Is<TransactionInput>(i =>
                i.PreviousTransaction.SequenceEqual(input.PreviousTransaction) && 
                i.PreviousIndex == input.PreviousIndex))).Returns(Task.FromResult(previousOutput));

            var signer = new PayToPubKeyHashSigner(new TransactionHasher(mockFetcher.Object));
            var sigScript = await signer.CreateSigScript(privateKey, transaction, input, SigHashType.All);

            transaction = transaction.CloneWithReplacedSigScript(input, sigScript);
            var verifier = new TransactionVerifier(mockFetcher.Object);
            Assert.True(await verifier.Verify(transaction, 0));
        }
    }
}
