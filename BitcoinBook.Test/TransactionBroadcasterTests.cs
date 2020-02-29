using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BitcoinBook.Test
{
    public class TransactionBroadcasterTests
    {
        // this works, but can't easily be tested
        [Fact]
        public async Task FirstTestnetTransactionTest()
        {
            var previousId = "b7773b4204686925e0cf607fb03250f0a18ce35cda48ac3ca8c004c33a9c3841";
            var targetHash = PublicKey.HashFromAddress("mwJn1YPMq7y5F8J3LkC5Hxg9PHyZ5K4cFv");
            var changeHash = PublicKey.HashFromAddress("mvzHKaHbDMaLdNbDrPuiSbGV91o6ADjCAK");
            var transacion = new Transaction(1, false,
                new []{ new TransactionInput(Cipher.ToBytes(previousId), 1, new Script(), new Script(), 0) },
                new []
                {
                    new TransactionOutput(600000, StandardScripts.PayToPubKeyHash(targetHash)),
                    new TransactionOutput(395000, StandardScripts.PayToPubKeyHash(changeHash)),
                }, 
                0, true);

            var privateKey = new PrivateKey(Cipher.Hash256(Encoding.ASCII.GetBytes("Tim's testnet address")).ToBigInteger());
            var wallet = new Wallet(new [] { privateKey });

            var signedTransaction = await IntegrationSetup.Testnet.TransactionSigner.SignTransaction(wallet, transacion, SigHashType.All);
            Assert.NotNull(signedTransaction.ToHex());

            // Broadcaster is not an API
            // var handler = new HttpClientHandler {CookieContainer = new CookieContainer()};
            // var broadcaster = new TransactionBroadcaster(new HttpClient(handler)
            //     {BaseAddress = new Uri("https://live.blockcypher.com/btc-testnet/pushtx")});
            // await broadcaster.Broadcast(signedTranaction);
        }
    }
}
