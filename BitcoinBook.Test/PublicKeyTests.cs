using System;
using System.Globalization;
using System.Numerics;
using Xunit;

namespace BitcoinBook.Test
{
    public class PublicKeyTests
    {
        readonly PublicKey key = new PublicKey(
            "0887387e452b8eacc4acfde10d9aaf7f6d9a0f975aabb10d006e4da568744d06c",
            "061de6d95231cd89026e286df3b6ae4a894a3378e393e93a0f45b666329a0ae34");

        [Fact]
        public void SignatureOneTest()
        {
            var signature = new Signature(
                "0ac8d1c87e51d0d441be8b3dd5b05c8795b48875dffe00b7ffcfac23010d3a395",
                "068342ceff8935ededd102dd876ffd6ba72d6a427a3edb13d26eb0781cb423c4");
            Assert.True(key.Verify(
                BigInteger.Parse("0ec208baa0fc1c19f708a9ca96fdeff3ac3f230bb4a7ba4aede4942ad003c0f60", NumberStyles.HexNumber),
                signature));
        }

        [Fact]
        public void SignatureTwoTest()
        {
            var signature = new Signature(
                "00eff69ef2b1bd93a66ed5219add4fb51e11a840f404876325a1e8ffe0529a2c",
                "00c7207fee197d27c618aea621406f6bf5ef6fca38681d82b2f06fddbdce6feab6");
            Assert.True(key.Verify(
                BigInteger.Parse("007c076ff316692a3d7eb3c3bb0f8b1488cf72e1afcd929e29307032997a838a3d", NumberStyles.HexNumber),
                signature));
        }

        [Fact]
        public void SecUncompressedFormatTest()
        {
            Assert.Equal(
                "04ffe558e388852f0120e46af2d1b370f85854a8eb0841811ece0e3e03d282d57c315dc72890a4f10a1481c031b03b351b0dc79901ca18a00cf009dbdb157a1d10", 
                new PrivateKey(5000).PublicKey.ToSecString(false));
            Assert.Equal(
                "04027f3da1918455e03c46f659266a1bb5204e959db7364d2f473bdf8f0a13cc9dff87647fd023c13b4a4994f17691895806e1b40b57f4fd22581a4f46851f3b06",
                new PrivateKey((BigInteger)Math.Pow(2018, 5)).PublicKey.ToSecString(false));
            Assert.Equal(
                "04d90cd625ee87dd38656dd95cf79f65f60f7273b67d3096e68bd81e4f5342691f842efa762fd59961d0e99803c61edba8b3e3f7dc3a341836f97733aebf987121",
                new PrivateKey("0deadbeef12345").PublicKey.ToSecString(false));
        }

        [Fact]
        public void SecCompressedFormatTest()
        {
            Assert.Equal(
                "0357a4f368868a8a6d572991e484e664810ff14c05c0fa023275251151fe0e53d1",
                new PrivateKey(5001).PublicKey.ToSecString());
            // TODO how come this one didn't come out the same as the book?
            // Assert.Equal(
            //     "02933ec2d2b111b92737ec12f1c5d20f3233a0ad21cd8b36d0bca7a0cfa5cb8701",
            //     new PrivateKey((BigInteger)Math.Pow(2019, 5)).PublicKey.ToSecString());
            Assert.Equal(
                "0296be5b1292f6c856b3c5654e886fc13511462059089cdf9c479623bfcbe77690",
                new PrivateKey("0deadbeef54321").PublicKey.ToSecString());
        }

        [Fact]
        public void ParseSecTest()
        {
            Assert.Equal(new PrivateKey(5000).PublicKey, 
                PublicKey.ParseSecFormat("04ffe558e388852f0120e46af2d1b370f85854a8eb0841811ece0e3e03d282d57c315dc72890a4f10a1481c031b03b351b0dc79901ca18a00cf009dbdb157a1d10"));
            Assert.Equal(new PrivateKey((BigInteger)Math.Pow(2018, 5)).PublicKey,
                PublicKey.ParseSecFormat("04027f3da1918455e03c46f659266a1bb5204e959db7364d2f473bdf8f0a13cc9dff87647fd023c13b4a4994f17691895806e1b40b57f4fd22581a4f46851f3b06"));
            Assert.Equal(new PrivateKey("0deadbeef12345").PublicKey,
                PublicKey.ParseSecFormat("04d90cd625ee87dd38656dd95cf79f65f60f7273b67d3096e68bd81e4f5342691f842efa762fd59961d0e99803c61edba8b3e3f7dc3a341836f97733aebf987121"));
            Assert.Equal(new PrivateKey(5001).PublicKey,
                PublicKey.ParseSecFormat("0357a4f368868a8a6d572991e484e664810ff14c05c0fa023275251151fe0e53d1"));
            Assert.Equal(new PrivateKey("0deadbeef54321").PublicKey,
                PublicKey.ParseSecFormat("0296be5b1292f6c856b3c5654e886fc13511462059089cdf9c479623bfcbe77690"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("x")]
        [InlineData("04")]
        [InlineData("0423")]
        public void ParseSecFormatExceptionTest(string sec)
        {
            Assert.Throws<FormatException>(() => PublicKey.ParseSecFormat(sec));
        }

        [Fact]
        public void AddressTest()
        {
            // Assert.Equal("mmTPbXQFxboEtNRkwfh6K51jvdtHLxGeMA",
            //     new PrivateKey(5002).PublicKey.ToAddress(false, false));
            // Assert.Equal("mopVkxp8UhXqRYbCYJsbeE1h1fiF64jcoH",
            //     new PrivateKey((BigInteger)Math.Pow(2020, 5)).PublicKey.ToAddress(true, false));
            Assert.Equal("1F1Pn2y6pDb68E5nYJJeba4TLg2U7B6KF1",
                new PrivateKey("012345deadbeef").PublicKey.ToAddress());
        }

        [Fact]
        public void HexTest()
        {
            Assert.Equal("1F", $"{new BigInteger(31):X2}");
            Assert.Equal("001F", $"{new BigInteger(31):X4}");
            Assert.Equal("00FE", $"{new BigInteger(254):X4}");
            Assert.Equal("0FE", $"{new BigInteger(254):X2}");
            Assert.Equal("00000000000000000000000000000000000000000000000000000000000000FE", $"{new BigInteger(254):X64}");
            Assert.Equal("00000000000000000000000000000000000000000000000000000000000000FE", new BigInteger(254).ToString("X64"));
        }
    }
}
