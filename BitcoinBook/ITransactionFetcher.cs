using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public interface ITransactionFetcher
    {
        Task<Transaction> Fetch(string transactionId, bool fresh = false);
        Task<Transaction> Fetch(byte[] transactionId, bool fresh = false);
        Task<TransactionOutput[]> GetPriorOutputs(IEnumerable<TransactionInput> inputs);
        Task<TransactionOutput> GetPriorOutput(TransactionInput input);
        Task<TransactionOutput> GetOutput(byte[] transactionId, int index);
    }
}