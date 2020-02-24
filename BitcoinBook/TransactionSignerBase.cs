using System.Threading.Tasks;

namespace BitcoinBook
{
    public abstract class TransactionSignerBase : ITransactionSigner
    { 
        readonly TransactionHasher hasher;

        protected TransactionSignerBase(TransactionHasher hasher)
        {
            this.hasher = hasher;
        }

        public abstract Task<Script> CreateSigScript(PrivateKey privateKey, Transaction transaction, TransactionInput input, SigHashType sigHashType);

        protected async Task<byte[]> ComputerSignatureBytes(PrivateKey privateKey, Transaction transaction, TransactionInput input, SigHashType sigHashType)
        {
            var hash = await hasher.ComputeSigHash(transaction, input, sigHashType);
            return privateKey.Sign(hash).ToDer().Concat((byte) sigHashType);
        }
    }
}