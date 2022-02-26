using System.Threading.Tasks;

namespace BitcoinBook;

public interface IInputSigner
{
    Task<Script> CreateSigScript(PrivateKey privateKey, Transaction transaction, TransactionInput input,
        SigHashType sigHashType);
    Task<Script> CreateSigScript(Wallet wallet, Transaction transaction, TransactionInput input,
        SigHashType sigHashType);
}