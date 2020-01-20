using System;

namespace BitcoinBook
{
    public struct DoublePoint : IEquatable<DoublePoint>
    {
        readonly double x;
        readonly double y;

        public double X => !IsInfinity ? x : throw new ArithmeticException("X not valid for infinity point");
        public double Y => !IsInfinity ? y : throw new ArithmeticException("Y not valid for infinity point");
        public double A { get; }
        public double B { get; }
        public bool IsInfinity { get; }

        public DoublePoint(double x, double y, double a, double b)
        {
            if (Math.Pow(y, 2) != Math.Pow(x, 3) + a * x + b)
            {
                throw new ArithmeticException($"{x},{y} is not on the curve");
            }

            this.x = x;
            this.y = y;
            A = a;
            B = b;
            IsInfinity = false;
        }

        DoublePoint(double a, double b)
        {
            A = a;
            B = b;
            x = double.MinValue;
            y = double.MinValue;
            IsInfinity = true;
        }

        public static DoublePoint Infinity(double a, double b)
        {
            return new DoublePoint(a, b);
        }

        public bool Equals(DoublePoint p)
        {
            if (A != p.A || B != p.B) return false;
            if (IsInfinity != p.IsInfinity) return false;
            if (IsInfinity && p.IsInfinity) return true;
            return X == p.X && Y == p.Y;
        }

        public override bool Equals(object obj)
        { 
            if (obj == null || obj.GetType() != GetType()) return false;
            return Equals((DoublePoint) obj);
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

        public DoublePoint Add(DoublePoint p)
        {
            CheckCurve(p);
            if (IsInfinity) return p;
            if (p.IsInfinity) return this;
            if (X == p.x && Y != p.Y) return Infinity(A, B);
            if (Equals(p)) return AddToSelf(this);
            return AddGeneral(this, p);
        }

        public static DoublePoint operator +(DoublePoint p1, DoublePoint p2) => p1.Add(p2);

        public override string ToString()
        {
            return IsInfinity ? "Inf" : $"({x},{y})_{A}_{B}";
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        void CheckCurve(DoublePoint p)
        {
            if (p.A != A || p.B != B) throw new InvalidOperationException("Points must be on the same curve");
        }

        static DoublePoint AddToSelf(DoublePoint p)
        {
            var slope = SlopeOfTangent(p);
            var rx = Math.Pow(slope, 2) - 2 * p.X;
            var ry = slope * (p.X - rx) - p.Y;
            return new DoublePoint(rx, ry, p.A, p.B);
        }

        static DoublePoint AddGeneral(DoublePoint p1, DoublePoint p2)
        {
            var slope = Slope(p1, p2);
            var rx = Math.Pow(slope, 2) - p1.X - p2.X;
            var ry = slope * (p1.X - rx) - p1.Y;
            return new DoublePoint(rx, ry, p1.A, p1.B);
        }

        static double Slope(DoublePoint p1, DoublePoint p2)
        {
            return (p2.Y - p1.Y) / (p2.X - p1.X);
        }

        static double SlopeOfTangent(DoublePoint p)
        {
            return (3 * Math.Pow(p.X, 2) + p.A) / (2 * p.Y);
        }
    }
}
