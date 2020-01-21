using System;
using System.Numerics;

namespace BitcoinBook
{
    public class Curve : IEquatable<Curve>
    {
        readonly Field field;

        public FieldElement A { get; }
        public FieldElement B { get; }

        public Curve(FieldElement a, FieldElement b)
        {
            if (a.Field!= b.Field) throw new InvalidOperationException("Numbers must be in the same field");

            A = a;
            B = b;
            field = A.Field;
            Infinity = BitcoinBook.Point.Infinity(this);
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
            return Point(field.Element(x), field.Element(y));
        }

        public Point Infinity { get; }

        public bool Equals(Curve other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(field, other.field) && A.Equals(other.A) && B.Equals(other.B) && Infinity.Equals(other.Infinity);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Curve) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (field != null ? field.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ A.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                hashCode = (hashCode * 397) ^ Infinity.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Curve a, Curve b) => a.Equals(b);
        public static bool operator !=(Curve a, Curve b) => !a.Equals(b);
    }
}
