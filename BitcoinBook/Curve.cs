using System.Numerics;

namespace BitcoinBook
{
    public class Curve
    {
        public double A { get; }
        public double B { get; }

        public Curve(double a, double b)
        {
            A = a;
            B = b;
            Infinity = BitcoinBook.Point.Infinity(A, B);
        }

        public Point Point(double x, double y)
        {
            return new Point(x, y, A, B);
        }

        public Point Infinity { get; }
    }
}
