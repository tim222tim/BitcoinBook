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
            var p = BigInteger.Pow(2, 256) - BigInteger.Pow(2, 32) - 977;

            // ReSharper disable once ObjectCreationAsStatement
            new S256Point(gx, gy);
        }

    }
}
