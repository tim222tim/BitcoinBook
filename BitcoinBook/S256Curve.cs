using System.Globalization;
using System.Numerics;

namespace BitcoinBook
{
    public static class S256Curve
    {
        public static Field Field { get; } = new Field(BigInteger.Pow(2, 256) - BigInteger.Pow(2, 32) - 977);
        public static Curve Curve { get; } = new Curve(Field, 0, 7);
        public static S256Point Generator = new S256Point(
            BigInteger.Parse("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", NumberStyles.HexNumber),
            BigInteger.Parse("483ada7726a3c4655da4fbfc0e1108a8fd17b448a68554199c47d08ffb10d4b8", NumberStyles.HexNumber)
        );
        public static BigInteger Order { get; } = 
            BigInteger.Parse("00FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", NumberStyles.HexNumber);

        public static S256Point Point(BigInteger x, BigInteger y)
        {
            return new S256Point(x, y);
        }
    }
}
