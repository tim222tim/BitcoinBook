using System;
using System.Numerics;

namespace BitcoinBook
{
    public class Point : IEquatable<Point>
    {
        readonly FieldElement x;
        readonly FieldElement y;

        public FieldElement X => !IsInfinity ? x : throw new ArithmeticException("X not valid for infinity point");
        public FieldElement Y => !IsInfinity ? y : throw new ArithmeticException("Y not valid for infinity point");
        public Curve Curve { get; }
        public bool IsInfinity { get; }

        FieldElement A => Curve.A;
        FieldElement B => Curve.B;

        public Point(FieldElement x, FieldElement y, Curve curve)
        {
            FieldElement.ThrowIfNotSameField(x, y, curve.A);

            if ((y ^ 2) != (x ^ 3) + curve.A * x + curve.B)
            {
                throw new ArithmeticException($"{x},{y} is not on the curve");
            }

            this.x = x;
            this.y = y;
            Curve = curve;

            IsInfinity = false;
        }

        Point(Curve curve)
        {
            x = default;
            y = default;
            Curve = curve;
            IsInfinity = true;
        }

        public static Point Infinity(Curve curve)
        {
            return new Point(curve);
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
            return Equals((Point)obj);
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
            ThrowIfNotSameCurve(this, p);
            if (IsInfinity) return p;
            if (p.IsInfinity) return this;
            if (X == p.x && Y != p.Y) return Curve.Infinity;
            if (Equals(p)) return AddToSelf(this);
            return AddGeneral(this, p);
        }

        public virtual Point MultiplyBy(BigInteger coefficient)
        {
            if (coefficient < 0) throw new ArgumentException("Must be 0 or greater", nameof(coefficient));

            var current = this;
            var result = Curve.Infinity;
            while (coefficient > 0)
            {
                if (!coefficient.IsEven)
                {
                    result += current;
                }

                current += current;
                coefficient >>= 1;
            }

            return result;
        }

        public BigInteger GetOrder()
        {
            BigInteger count = 1;
            var product = this;
            while (!product.IsInfinity)
            {
                product += this;
                ++count;
            }

            return count;
        }

        public static bool operator ==(Point a, Point b) => a.Equals(b);
        public static bool operator !=(Point a, Point b) => !a.Equals(b);
        public static Point operator +(Point p1, Point p2) => p1.Add(p2);
        public static Point operator *(Point p1, BigInteger coefficient) => p1.MultiplyBy(coefficient);

        public static bool SameCurve(params Point[] points)
        {
            foreach (var point in points)
            {
                if (point.Curve != points[0].Curve)
                {
                    return false;
                }
            }
            return true;
        }

        public static void ThrowIfNotSameCurve(params Point[] points)
        {
            if (!SameCurve(points)) throw new InvalidOperationException("Points must be on the same curve");
        }

        public override string ToString()
        {
            return IsInfinity ? "Inf" : 
                x.Field.Prime < 1000 ?
                    $"({x.Number},{y.Number})_{A.Number}_{B.Number} Field({x.Field.Prime})" :
                    $"(0x{X.Number:X64},0x{Y.Number:X64})_S256)";
        }

        static Point AddToSelf(Point p)
        {
            var slope = SlopeOfTangent(p);
            var rx = (slope ^ 2) - p.X.Field.Element(2) * p.X;
            var ry = slope * (p.X - rx) - p.Y;
            return p.Curve.Point(rx, ry);
        }

        static Point AddGeneral(Point p1, Point p2)
        {
            var slope = Slope(p1, p2);
            var rx = (slope ^ 2) - p1.X - p2.X;
            var ry = slope * (p1.X - rx) - p1.Y;
            return p1.Curve.Point(rx, ry);
        }

        static FieldElement Slope(Point p1, Point p2)
        {
            return (p2.Y - p1.Y) / (p2.X - p1.X);
        }

        static FieldElement SlopeOfTangent(Point p)
        {
            return (p.X.Field.Element(3) * (p.X ^ 2) + p.A) / (p.X.Field.Element(2) * p.Y);
        }
    }
}
