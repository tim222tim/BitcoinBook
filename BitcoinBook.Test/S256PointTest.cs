using System.Globalization;
using System.Numerics;
using Xunit;

namespace BitcoinBook.Test
{
    public class S256PointTest
    {
        [Fact]
        public void GeneratorPointTest()
        {
            var gx = BigInteger.Parse("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", NumberStyles.HexNumber);
            var gy = BigInteger.Parse("483ada7726a3c4655da4fbfc0e1108a8fd17b448a68554199c47d08ffb10d4b8", NumberStyles.HexNumber);

            var g = new S256Point(gx, gy);
            Assert.Equal(S256Point.S256Generator, g);
        }

        [Fact]
        public void OrderTest()
        {
            Assert.Equal(S256Point.Infinity(), S256Point.S256Generator * S256Point.S256Order);
        }
    }
}
