namespace BitcoinBook
{
    public class DoubleCurve
    {
        public double A { get; }
        public double B { get; }

        public DoubleCurve(double a, double b)
        {
            A = a;
            B = b;
            Infinity = BitcoinBook.DoublePoint.Infinity(A, B);
        }

        public DoublePoint Point(double x, double y)
        {
            return new DoublePoint(x, y, A, B);
        }

        public DoublePoint Infinity { get; }
    }
}
