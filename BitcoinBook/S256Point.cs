using System.Globalization;
using System.Numerics;

namespace BitcoinBook
{
    public class S256Point : Point
    {
        public S256Point(FieldElement x, FieldElement y) : base(x, y, S256Curve.Curve)
        {
        }

        public S256Point(BigInteger x, BigInteger y) : this(new FieldElement(x, S256Curve.Field), new FieldElement(y, S256Curve.Field))
        {
        }

        public S256Point(string x, string y, NumberStyles numberStyles = NumberStyles.HexNumber) 
            : this(BigInteger.Parse(x, numberStyles), BigInteger.Parse(y, numberStyles))
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
