using System.Text;
using Xunit;

namespace BitcoinBook.Test;

public class BloomFilterTests
{
    [Theory]
    [InlineData("4000600a080000010940", new[] {"Hello World", "Goodbye!"}, 10, 5, 99)]
    [InlineData("6006", new[] {"hello world", "goodbye"}, 2, 2, 42)]
    public void BloomResultTest(string expectedResult, string[] inputs, int size, int hashCount, uint tweak)
    {
        var filter = new BloomFilter(size, hashCount, tweak);
        foreach (var value in inputs)
        {
            filter.Add(Encoding.ASCII.GetBytes(value));
        }
        Assert.Equal(expectedResult, filter.Result.ToHex());
    }
}