namespace BitcoinBook
{
    public interface ITransactionSigner
    {
        Script CreateSigScript(IHashSigner hashSigner, Transaction transaction, TransactionInput input, SigHashType sigHashType);
    }
}
