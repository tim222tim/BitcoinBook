using System;
using System.Numerics;

namespace BitcoinBook
{
    public class Field : IEquatable<Field>
    {
        public BigInteger Prime { get; }

        public Field(BigInteger prime)
        {
            Prime = prime;
        }

        public FieldElement Element(BigInteger number)
        {
            return new FieldElement(number, this);
        }

        public bool Equals(Field? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Prime.Equals(other.Prime);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Field) obj);
        }

        public override int GetHashCode()
        {
            return Prime.GetHashCode();
        }

        public static bool operator ==(Field a, Field b) => a?.Equals(b) ?? ReferenceEquals(null, b);
        public static bool operator !=(Field a, Field b) => !a?.Equals(b) ?? !ReferenceEquals(null, b);
    }
}