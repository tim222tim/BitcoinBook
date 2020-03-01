using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BitcoinBook.Test
{
    public class PaymentMakerTests
    {
        [Fact]
        public async Task CreatePaymentTest()
        {
            var paymentMaker = new PaymentMaker(IntegrationSetup.Testnet.Fetcher, IntegrationSetup.Testnet.Signer);

            var privateKey = new PrivateKey(Cipher.Hash256(Encoding.ASCII.GetBytes("Tim's testnet address")).ToBigInteger());
            var wallet = new Wallet(new[] { privateKey });

            var transaction = await paymentMaker.CreatePaymenTransaction(wallet,
                new[] { "62cdf979c66a63598e1bd355356da4c8ce32c231d7f14e90590060ee1e980b36:1" }, 90000, 5000,
                "mwJn1YPMq7y5F8J3LkC5Hxg9PHyZ5K4cFv", "mvzHKaHbDMaLdNbDrPuiSbGV91o6ADjCAK");
            Assert.Equal(2, transaction.Outputs.Count);
            Assert.Equal(90000, transaction.Outputs[0].Amount);
            Assert.Equal(300000, transaction.Outputs[1].Amount);
        }
    }
}
