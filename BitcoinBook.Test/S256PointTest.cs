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
            Assert.Equal(S256Curve.Curve.Infinity, S256Curve.Generator * S256Curve.Order);
        }

        [Fact]
        public void ToStringTest()
        {
            Assert.Equal("(0x79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798,0x483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8)_S256)",
                S256Curve.Generator.ToString());
        }
    }
}
