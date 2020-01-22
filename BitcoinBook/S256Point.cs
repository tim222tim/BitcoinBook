using System.Numerics;

namespace BitcoinBook
{
    public class S256Point : Point
    {
        public S256Point(S256FieldElement x, S256FieldElement y) : base(x, y, S256Curve.Curve)
        {
        }

        public S256Point(BigInteger x, BigInteger y) : this(new S256FieldElement(x), new S256FieldElement(y))
        {
        }

        S256Point() : base(S256Curve.Curve)
        {
        }

        public static S256Point Infinity()
        {
            return new S256Point();
        }

        public override Point MultiplyBy(BigInteger coefficient)
        {
            coefficient = BigInteger.Remainder(coefficient, S256Curve.Order);
            return base.MultiplyBy(coefficient);
        }

        public override string ToString()
        {
            return IsInfinity ? "Inf" : $"(0x{X.Number:X64},0x{Y.Number:X64})_S256)";
        }
    }
}
