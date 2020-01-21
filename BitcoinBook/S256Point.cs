using System.Globalization;
using System.Numerics;

namespace BitcoinBook
{
    public class S256Point : Point
    {
        public static Curve S256Curve { get; } = new Curve(S256FieldElement.S256Field, 0, 7);
        public static S256Point S256Generator = new S256Point(
            BigInteger.Parse("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", NumberStyles.HexNumber),
            BigInteger.Parse("483ada7726a3c4655da4fbfc0e1108a8fd17b448a68554199c47d08ffb10d4b8", NumberStyles.HexNumber)
        );
        public static BigInteger S256Order { get; } = BigInteger.Parse("00FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", NumberStyles.HexNumber);

        public S256Point(S256FieldElement x, S256FieldElement y) : base(x, y, S256Curve)
        {
        }

        public S256Point(BigInteger x, BigInteger y) : this(new S256FieldElement(x), new S256FieldElement(y))
        {
        }

        S256Point() : base(S256Curve)
        {
        }

        public static S256Point Infinity()
        {
            return new S256Point();
        }

        public override Point MultiplyBy(BigInteger coefficient)
        {
            coefficient = BigInteger.Remainder(coefficient, S256Order);
            return base.MultiplyBy(coefficient);
        }
    }
}
