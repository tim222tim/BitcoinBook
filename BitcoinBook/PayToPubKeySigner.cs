using System.Threading.Tasks;

namespace BitcoinBook
{
    public class PayToPubKeySigner : TransactionSignerBase
    {
        public PayToPubKeySigner(TransactionHasher hasher) : base(hasher)
        {
        }

        public override async Task<Script> CreateSigScript(PrivateKey privateKey, Transaction transaction, TransactionInput input, SigHashType sigHashType)
        {
            var signature = await ComputerSignatureBytes(privateKey, transaction, input, sigHashType);
            return new Script(signature);
        }
    }
}
