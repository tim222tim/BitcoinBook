using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public interface ITransactionFetcher
    {
        Task<Transaction?> Fetch(string transactionId, bool fresh = false);
        Task<Transaction?> Fetch(byte[] transactionId, bool fresh = false);
        Task<TransactionOutput[]> FetchPriorOutputs(IEnumerable<TransactionInput> inputs);
        Task<TransactionOutput> FetchPriorOutput(TransactionInput input);
        Task<TransactionOutput> FetchOutput(string outputPoint);
        Task<TransactionOutput> FetchOutput(OutputPoint outputPoint);
    }
}