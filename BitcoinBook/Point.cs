using System;
using System.Numerics;

namespace BitcoinBook
{
    public struct Point : IEquatable<Point>
    {
        readonly BigInteger x;
        readonly BigInteger y;

        public BigInteger X => !IsInfinity ? x : throw new ArithmeticException("X not valid for identity point");
        public BigInteger Y => !IsInfinity ? y : throw new ArithmeticException("Y not valid for identity point");
        public BigInteger A { get; }
        public BigInteger B { get; }
        public bool IsInfinity { get; }

        public Point(BigInteger x, BigInteger y, BigInteger a, BigInteger b)
        {
            if (BigInteger.Pow(y, 2) != BigInteger.Pow(x, 3) + a * x + b)
            {
                throw new ArithmeticException($"{x},{y} is not on the curve");
            }

            this.x = x;
            this.y = y;
            A = a;
            B = b;
            IsInfinity = false;
        }

        Point(BigInteger a, BigInteger b)
        {
            A = a;
            B = b;
            IsInfinity = true;
        }

        public static Point Infinity(BigInteger a, BigInteger b)
        {
            return new Point(a, b);
        }

        public bool Equals(Point p)
        {
            if (A != p.A || B != p.B) return false;
            if (IsInfinity != p.IsInfinity) return false;
            if (IsInfinity && p.IsInfinity) return true;
            return X == p.X && Y == p.Y;
        }

        public override bool Equals(object obj)
        { 
            if (obj == null || obj.GetType() != GetType()) return false;
            return Equals((Point) obj);
        }

        public Point Add(Point p)
        {
            CheckCurve(p);
            if (IsInfinity) return p;
            if (p.IsInfinity) return this;
            if (X == p.x && Y != p.Y) return Infinity(A, B);
            throw new NotImplementedException();
        }

        public static Point operator +(Point p1, Point p2) => p1.Add(p2);

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        void CheckCurve(Point p)
        {
            if (p.A != A || p.B != B) throw new InvalidOperationException("Points must beon the same curve");
        }

    }
}
