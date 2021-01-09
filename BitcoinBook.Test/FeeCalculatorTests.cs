using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BitcoinBook.Test
{
    public class FeeCalculatorTests
    {
        readonly FeeCalculator calculator;

        public FeeCalculatorTests()
        {
            calculator = new FeeCalculator(IntegrationSetup.Mainnet.Fetcher);
        }

        [Theory]
        [InlineData(50000, "1bdbd226611278e59f84206e1393666858387f9c17adb6608493250991341d1b")]
        [InlineData(0, "01cbd3888987dd2b590609dd31c89116852e1ffbf4ef245f851056057b6b17c6")]
        public async Task ShouldCaculateFees(long expected, string transactionId)
        {
            var transaction = await IntegrationSetup.Mainnet.Fetcher.Fetch(transactionId);
            Assert.NotNull(transaction);
            Assert.Equal(expected, await calculator.CalculateFeesAsync(transaction!));
        }
    }
}
