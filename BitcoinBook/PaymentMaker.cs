using System;
using System.Collections.Generic;

namespace BitcoinBook
{
    public class PaymentMaker
    {
        public Transaction CreatePaymenTransaction(IEnumerable<TransactionOutput> previousOutputs, long amount,
            string targetAddress, string changeAddress)
        {
            throw new NotImplementedException();
        }
    }
}
