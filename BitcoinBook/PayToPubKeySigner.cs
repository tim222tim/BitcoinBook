using System.Threading.Tasks;

namespace BitcoinBook
{
    public class PayToPubKeySigner : TransactionSignerBase
    {
        public PayToPubKeySigner(ITransactionFetcher fetcher, TransactionHasher hasher) : base(fetcher, hasher)
        {
        }

        public override async Task<Script> CreateSigScript(PrivateKey privateKey, Transaction transaction, TransactionInput input, SigHashType sigHashType)
        {
            var signature = await ComputeSignatureBytes(privateKey, transaction, input, sigHashType);
            return new Script(signature);
        }
    }
}
