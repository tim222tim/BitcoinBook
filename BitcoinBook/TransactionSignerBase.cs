using System.Threading.Tasks;

namespace BitcoinBook
{
    public abstract class TransactionSignerBase : ITransactionSigner
    {
        readonly TransactionHasher hasher;

        protected TransactionSignerBase(ITransactionFetcher fetcher, TransactionHasher hasher)
        {
            Fetcher = fetcher;
            this.hasher = hasher;
        }

        protected ITransactionFetcher Fetcher { get; }

        public abstract Task<Script> CreateSigScript(PrivateKey privateKey, Transaction transaction, TransactionInput input, SigHashType sigHashType);
        public abstract Task<Script> CreateSigScript(Wallet wallet, Transaction transaction, TransactionInput input, SigHashType sigHashType);

        protected async Task<byte[]> ComputeSignatureBytes(PrivateKey privateKey, Transaction transaction, TransactionInput input, SigHashType sigHashType)
        {
            var hash = await hasher.ComputeSigHash(transaction, input, sigHashType);
            return privateKey.Sign(hash).ToDer().Concat((byte) sigHashType);
        }
    }
}