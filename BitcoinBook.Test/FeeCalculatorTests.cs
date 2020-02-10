using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BitcoinBook.Test
{
    public class FeeCalculatorTests
    {
        readonly TransactionFetcher fetcher = new TransactionFetcher(new HttpClient { BaseAddress = new Uri("http://mainnet.programmingbitcoin.com") });
        readonly FeeCalculator calculator;

        public FeeCalculatorTests()
        {
            calculator = new FeeCalculator(fetcher);
        }

        [Theory]
        [InlineData(50000, "1bdbd226611278e59f84206e1393666858387f9c17adb6608493250991341d1b")]
        [InlineData(0, "01cbd3888987dd2b590609dd31c89116852e1ffbf4ef245f851056057b6b17c6")]
        public async Task ShouldCaculateFees(long expected, string transactionId)
        {
            var transaction = await fetcher.Fetch(transactionId);
            Assert.Equal(expected, await calculator.CalculateFeesAsync(transaction));
        }
    }
}
