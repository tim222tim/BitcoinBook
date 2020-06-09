using System.Text;
using Xunit;

namespace BitcoinBook.Test
{
    public class BloomFilterTests
    {
        [Fact]
        public void BloomResultTest()
        {
            var filter = new BloomFilter(10, 5, 99);
            foreach (var value in new[] {"Hello World",  "Goodbye!"})
            {
                filter.Add(Encoding.ASCII.GetBytes(value));
            }
            Assert.Equal("4000600a080000010940", filter.Result.ToHex());
        }
    }
}
