using System;

namespace BitcoinBook
{
    public struct Point : IEquatable<Point>
    {
        readonly FieldElement x;
        readonly FieldElement y;

        public FieldElement X => !IsInfinity ? x : throw new ArithmeticException("X not valid for infinity point");
        public FieldElement Y => !IsInfinity ? y : throw new ArithmeticException("Y not valid for infinity point");
        public FieldElement A { get; }
        public FieldElement B { get; }
        public bool IsInfinity { get; }

        public Point(FieldElement x, FieldElement y, FieldElement a, FieldElement b)
        {
            if ((y ^ 2) != (x ^ 3) + a * x + b)
            {
                throw new ArithmeticException($"{x},{y} is not on the curve");
            }

            this.x = x;
            this.y = y;
            A = a;
            B = b;
            IsInfinity = false;
        }

        Point(FieldElement a, FieldElement b)
        {
            A = a;
            B = b;
            IsInfinity = true;
        }

        public static Point Infinity(FieldElement a, FieldElement b)
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
            return Equals((DoublePoint)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ A.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                hashCode = (hashCode * 397) ^ IsInfinity.GetHashCode();
                return hashCode;
            }
        }

        public Point Add(Point p)
        {
            CheckCurve(p);
            if (IsInfinity) return p;
            if (p.IsInfinity) return this;
            if (X == p.x && Y != p.Y) return Infinity(A, B);
            if (Equals(p)) return AddToSelf(this);
            return AddGeneral(this, p);
        }

        public static Point operator +(Point p1, Point p2) => p1.Add(p2);

        public override string ToString()
        {
            return IsInfinity ? "Inf" : $"({x.Number},{y.Number})_{A.Number}_{B.Number} Field({x.Prime})";
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        void CheckCurve(Point p)
        {
            if (p.A != A || p.B != B) throw new InvalidOperationException("Points must be on the same curve");
        }

        static Point AddToSelf(Point p)
        {
            var slope = SlopeOfTangent(p);
            var rx = (slope ^ 2) - new FieldElement(2, p.x.Prime) * p.X;
            var ry = slope * (p.X - rx) - p.Y;
            return new Point(rx, ry, p.A, p.B);
        }

        static Point AddGeneral(Point p1, Point p2)
        {
            var slope = Slope(p1, p2);
            var rx = (slope ^ 2) - p1.X - p2.X;
            var ry = slope * (p1.X - rx) - p1.Y;
            return new Point(rx, ry, p1.A, p1.B);
        }

        static FieldElement Slope(Point p1, Point p2)
        {
            return (p2.Y - p1.Y) / (p2.X - p1.X);
        }

        static FieldElement SlopeOfTangent(Point p)
        {
            return (new FieldElement(3, p.x.Prime) * (p.X ^ 2) + p.A) / (new FieldElement(2, p.x.Prime) * p.Y);
        }
    }
}
