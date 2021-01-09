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
            var g = new Point(
                new FieldElement(BigInteger.Parse("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", NumberStyles.HexNumber), S256Curve.Field),
                new FieldElement(BigInteger.Parse("483ada7726a3c4655da4fbfc0e1108a8fd17b448a68554199c47d08ffb10d4b8", NumberStyles.HexNumber), S256Curve.Field),
                new Curve(S256Curve.Field, 0, 7));
            Assert.Equal(S256Curve.Generator, g);
        }

        [Fact]
        public void OrderTest()
        {
            Assert.Equal(S256Curve.Curve.GetInfinity(), S256Curve.Generator * S256Curve.Order);
        }

        [Fact]
        public void ToStringTest()
        {
            Assert.Equal("(0x79be667ef9dcbbac55a06295ce870b07029bfcdb2dce28d959f2815b16f81798,0x483ada7726a3c4655da4fbfc0e1108a8fd17b448a68554199c47d08ffb10d4b8)_S256)",
                S256Curve.Generator.ToString());
        }
    }
}
