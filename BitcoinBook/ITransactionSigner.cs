using System.Threading.Tasks;

namespace BitcoinBook
{
    public interface ITransactionSigner
    {
        Task<Script> CreateSigScript(PrivateKey privateKey, Transaction transaction, TransactionInput input,
            SigHashType sigHashType);
    }
}
