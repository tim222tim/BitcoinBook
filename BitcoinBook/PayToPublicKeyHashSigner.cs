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

        public Script CreateSigScript(IHashSigner hashSigner, Transaction transaction, TransactionInput input, SigHashType sigHashType)
        {
            throw new System.NotImplementedException();
        }
    }
}
