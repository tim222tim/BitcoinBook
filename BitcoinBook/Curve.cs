using System.Numerics;

namespace BitcoinBook
{
    public class Curve
    {
        public BigInteger A { get; }
        public BigInteger B { get; }

        public Curve(BigInteger a, BigInteger b)
        {
            A = a;
            B = b;
            Infinity = BitcoinBook.Point.Infinity(A, B);
        }

        public Point Point(BigInteger x, BigInteger y)
        {
            return new Point(x, y, A, B);
        }

        public Point Infinity { get; }
    }
}
