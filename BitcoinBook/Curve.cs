using System;
using System.Globalization;
using System.Numerics;

namespace BitcoinBook
{
    public record Curve
    {
        public FieldElement A { get; }
        public FieldElement B { get; }

        public Point GetInfinity() => BitcoinBook.Point.Infinity(this);

        public Curve(FieldElement a, FieldElement b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));
            if (!a.Field.Equals(b.Field)) throw new InvalidOperationException("Numbers must be in the same field");

            A = a;
            B = b;
        }

        public Curve(Field field, BigInteger a, BigInteger b) : this(field.Element(a), field.Element(b))
        {
        }

        public Point Point(FieldElement x, FieldElement y)
        {
            if (x.Field != y.Field) throw new InvalidOperationException("Numbers must be in the same field");

            return new Point(x, y, this);
        }

        public Point Point(BigInteger x, BigInteger y)
        {
            x = BigInteger.Remainder(x, A.Field.Prime);
            y = BigInteger.Remainder(y, A.Field.Prime);
            return Point(A.Field.Element(x), A.Field.Element(y));
        }

        public Point Point(string x, string y, NumberStyles numberStyles = NumberStyles.HexNumber)
        {
            return Point(BigInteger.Parse(x, numberStyles), BigInteger.Parse(y, numberStyles));
        }
    }
}
