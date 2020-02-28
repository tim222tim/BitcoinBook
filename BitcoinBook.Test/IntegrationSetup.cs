using System;
using System.Net.Http;

namespace BitcoinBook.Test
{
    public static class IntegrationSetup
    {
        // needs mainnet and testnet version
        public static TransactionFetcher Fetcher { get; } = new TransactionFetcher(new HttpClient
            {BaseAddress = new Uri("http://testnet.programmingbitcoin.com")});
        public static TransactionHasher Hasher { get; }
        public static SignerMap SignerMap { get; }
        public static TransactionSigner TransactionSigner { get; }

        static IntegrationSetup()
        {
            Hasher = new TransactionHasher(Fetcher);
            SignerMap = new SignerMap(new PayToPubKeySigner(Fetcher, Hasher), new PayToPubKeyHashSigner(Fetcher, Hasher));
            TransactionSigner = new TransactionSigner(Fetcher, new ScriptClassifier(), SignerMap);
        }
    }
}
