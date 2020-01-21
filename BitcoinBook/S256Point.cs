using System.Numerics;

namespace BitcoinBook
{
    public class S256Point : Point
    {
        public static Curve S256Curve { get; } = new Curve(S256FieldElement.S256Field, 0, 7);

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
    }
}
