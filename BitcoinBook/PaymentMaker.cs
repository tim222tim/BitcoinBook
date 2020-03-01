using System;
using System.Collections.Generic;
using System.Linq;

namespace BitcoinBook
{
    public class PaymentMaker
    {
        readonly TransactionFetcher fetcher;

        public PaymentMaker(TransactionFetcher fetcher)
        {
            this.fetcher = fetcher;
        }

        public Transaction CreatePaymenTransaction(IEnumerable<string> previousOutputPoints, long amount,
            string targetAddress, string changeAddress)
        {
            var previousOutputs = previousOutputPoints.Select(async p => await fetcher.FetchOutput(p));
            throw new NotImplementedException();
        }

        public Transaction CreatePaymenTransaction(IEnumerable<TransactionOutput> previousOutputs, long amount,
            string targetAddress, string changeAddress)
        {
            throw new NotImplementedException();
        }
    }
}
