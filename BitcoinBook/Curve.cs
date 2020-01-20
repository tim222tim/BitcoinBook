namespace BitcoinBook
{
    public class Curve
    {
        public FieldElement A { get; }
        public FieldElement B { get; }

        public Curve(FieldElement a, FieldElement b)
        {
            A = a;
            B = b;
            Infinity = BitcoinBook.Point.Infinity(A, B);
        }

        public Point Point(FieldElement x, FieldElement y)
        {
            return new Point(x, y, A, B);
        }

        public Point Infinity { get; }
    }
}
