using System;
using System.Net.Http;

namespace BitcoinBook.Test
{
    public class IntegrationDependencies
    {
        public TransactionFetcher Fetcher { get; }
        public TransactionHasher Hasher { get; }
        public SignerMap SignerMap { get; }
        public TransactionSigner TransactionSigner { get; }

        public IntegrationDependencies(string fetcherBaseAddress) : 
            this(new TransactionFetcher(new HttpClient { BaseAddress = new Uri(fetcherBaseAddress) }))
        {
        }

        public IntegrationDependencies(TransactionFetcher fetcher)
        {
            Fetcher = fetcher;
            Hasher = new TransactionHasher(Fetcher);
            SignerMap = new SignerMap(new PayToPubKeySigner(Fetcher, Hasher), new PayToPubKeyHashSigner(Fetcher, Hasher));
            TransactionSigner = new TransactionSigner(Fetcher, new ScriptClassifier(), SignerMap);
        }
    }
}
