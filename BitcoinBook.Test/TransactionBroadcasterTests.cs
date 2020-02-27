using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BitcoinBook.Test
{
    public class TransactionBroadcasterTests
    {
        [Fact]
        public async Task FirstTestnetTransactionTest()
        {
            var previousId = "b7773b4204686925e0cf607fb03250f0a18ce35cda48ac3ca8c004c33a9c3841";
            var targetHash = PublicKey.HashFromAddress("mwJn1YPMq7y5F8J3LkC5Hxg9PHyZ5K4cFv");
            var changeHash = PublicKey.HashFromAddress("mvzHKaHbDMaLdNbDrPuiSbGV91o6ADjCAK");
            var transacion = new Transaction(1, true,
                new []{ new TransactionInput(Cipher.ToBytes(previousId), 1, new Script(), 0) },
                new []
                {
                    new TransactionOutput(600000, StandardScripts.PayToPubKeyHash(targetHash)),
                    new TransactionOutput(395000, StandardScripts.PayToPubKeyHash(changeHash)),
                }, 
                0, true);

            var hash = Cipher.Hash256(Encoding.ASCII.GetBytes("Tim's testnet address"));
            var privateKey = new PrivateKey(hash.ToBigInteger());
            var wallet = new Wallet(new [] { privateKey });
            var fetcher = new TransactionFetcher(new HttpClient { BaseAddress = new Uri("http://testnet.programmingbitcoin.com") });
            var hasher = new TransactionHasher(fetcher);
            var signer = new PayToPubKeyHashSigner(fetcher, hasher);
            var sigScript = await signer.CreateSigScript(wallet, transacion, transacion.Inputs[0], SigHashType.All);

            var signedTranaction = transacion.CloneWithReplacedSigScript(transacion.Inputs[0], sigScript);

            var broadcaster = new TransactionBroadcaster(new HttpClient
                {BaseAddress = new Uri("https://live.blockcypher.com/btc/pushtx")});
            await broadcaster.Broadcast(transacion);
        }

        [Fact]
        public void SameTest()
        {
            Assert.Equal("0200000000010209c06e8f02709bbd24df4601aaf7aec0622dc054ccf1efdef05c2496cc2bda360100000017160014920824945937eea43c243e24c396516da1297a27feffffff1f84bc94408b4629e817241bb510b0270750521d067d36bb134dbbbace5827890000000017160014b4eb55d256755ac15b9331a46d29a874cac6130efeffffff02d65715000000000017a9147edc432af47075df934929131c67afe121c759488740420f00000000001976a914a9b50000d3ffa8d58cafd00f192ea9730315f4f588ac02473044",
                         "0200000000010209c06e8f02709bbd24df4601aaf7aec0622dc054ccf1efdef05c2496cc2bda360100000017160014920824945937eea43c243e24c396516da1297a27feffffff1f84bc94408b4629e817241bb510b0270750521d067d36bb134dbbbace5827890000000017160014b4eb55d256755ac15b9331a46d29a874cac6130efeffffff02d65715000000000017a9147edc432af47075df934929131c67afe121c759488740420f00000000001976a914a9b50000d3ffa8d58cafd00f192ea9730315f4f588ac02473044");
        }
    }
}
