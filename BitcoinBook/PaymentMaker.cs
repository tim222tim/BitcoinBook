using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public class PaymentMaker
    {
        readonly TransactionFetcher fetcher;
        readonly TransactionSigner signer;

        public PaymentMaker(TransactionFetcher fetcher, TransactionSigner signer)
        {
            this.fetcher = fetcher;
            this.signer = signer;
        }

        public async Task<Transaction> CreatePaymenTransaction(Wallet wallet,
            IEnumerable<string> previousOutputPoints, long amount, long fee,
            string targetAddress, string changeAddress)
        {
            return await CreatePaymenTransaction(wallet, previousOutputPoints.Select(p => new OutputPoint(p)), amount,
                fee, targetAddress, changeAddress);
        }

        public async Task<Transaction> CreatePaymenTransaction(Wallet wallet, 
            IEnumerable<OutputPoint> previousOutputPoints, long amount, long fee,
            string targetAddress, string changeAddress)
        {
            var pointsList = previousOutputPoints.ToList();
            var previousOutputs = await fetcher.FetchOutputs(pointsList);

            var targetHash = Cipher.HashFromAddress(targetAddress);
            var changeHash = Cipher.HashFromAddress(changeAddress);
            var totalInputAmount = previousOutputs.Sum(o => o.Amount);

            var transacion = new Transaction(1, false,
                pointsList.Select(p => new TransactionInput(p.TransactionId, p.Index, new Script(), new Script(), 0)),
                new[]
                {
                    new TransactionOutput(amount, StandardScripts.PayToPubKeyHash(targetHash)),
                    new TransactionOutput(totalInputAmount - amount - fee, StandardScripts.PayToPubKeyHash(changeHash)),
                },
                0, true);

            transacion = await signer.SignTransaction(wallet, transacion, SigHashType.All);
            return transacion;
        }
    }
}
