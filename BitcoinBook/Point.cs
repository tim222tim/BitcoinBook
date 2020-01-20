using System;
using System.Numerics;

namespace BitcoinBook
{
    public class Point : IEquatable<Point>
    {
        public BigInteger X { get; }
        public BigInteger Y { get; }
        public BigInteger A { get; }
        public BigInteger B { get; }

        public Point(BigInteger x, BigInteger y, BigInteger a, BigInteger b)
        {
            if (BigInteger.Pow(y, 2) != BigInteger.Pow(x, 3) + a * x + b)
            {
                throw new ArithmeticException($"{x},{y} is not on the curve");
            }

            X = x;
            Y = y;
            A = a;
            B = b;
        }

        public bool Equals(Point other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X.Equals(other.X) && Y.Equals(other.Y) && A.Equals(other.A) && B.Equals(other.B);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ A.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                return hashCode;
            }
        }
    }
}
