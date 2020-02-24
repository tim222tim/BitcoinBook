using System.Threading.Tasks;

namespace BitcoinBook
{
    public class PayToPublicKeyHashSigner : ITransactionSigner
    {
        readonly ITransactionFetcher fetcher;
        readonly TransactionHasher hasher;

        public PayToPublicKeyHashSigner(ITransactionFetcher fetcher, TransactionHasher hasher)
        {
            this.fetcher = fetcher;
            this.hasher = hasher;
        }

        public async Task<Script> CreateSigScript(PrivateKey privateKey, Transaction transaction,
            TransactionInput input, SigHashType sigHashType)
        {
            var hash = await hasher.ComputeSigHash(transaction, input, sigHashType);
            var signature = privateKey.Sign(hash).ToDer().Concat((byte) sigHashType);
            var sec = privateKey.PublicKey.ToSec();
            return new Script(signature, sec);
        }
    }
}
