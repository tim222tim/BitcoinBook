using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace BitcoinBook.Test;

public class PrivateKeyTests
{
    [Fact]
    public void SignHashTest()
    {
        var privateKey = new PrivateKey();
        var hash = new BigInteger(2384234).ToBigBytes32();
        var signature = privateKey.Sign(hash);
        Assert.True(privateKey.PublicKey.Verify(hash, signature));
    }

    [Fact]
    public void SignDataTest()
    {
        var privateKey = new PrivateKey(12345);
        var data = "Programming Bitcoin!";
        var signature = privateKey.SignPreimage(data);
        Assert.True(privateKey.PublicKey.Verify(data, signature));
    }

    public static IEnumerable<object[]> WifTestData => new[]
    {
        new object[] { "cMahea7zqjxrtgAbB7LSGbcQUr1uX1ojuat9jZodMN8rFTv2sfUK", new PrivateKey(5003), true, true },
        new object[] { "91avARGdfge8E4tZfYLoxeJ5sGBdNJQH4kvjpWAxgzczjbCwxic", new PrivateKey(BigInteger.Pow(2021, 5)), false, true},
        new object[] { "KwDiBf89QgGbjEhKnhXJuH7LrciVrZi3qYjgiuQJv1h8Ytr2S53a", new PrivateKey("54321deadbeef"), true, false },
    };

    [Theory]
    [MemberData(nameof(WifTestData))]
    public void WifTest(string expected, PrivateKey privateKey, bool compressed, bool testnet)
    {
        Assert.Equal(expected, privateKey.Wif(compressed, testnet));
    }
}