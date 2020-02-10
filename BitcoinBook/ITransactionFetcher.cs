using System.Threading.Tasks;

namespace BitcoinBook
{
    public interface ITransactionFetcher
    {
        Task<Transaction> Fetch(string transactionId, bool fresh = false);
        Task<Transaction> Fetch(byte[] transactionId, bool fresh = false);
    }
}