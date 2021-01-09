using System;
using System.Numerics;

namespace BitcoinBook
{
    public record FieldElement
    {
        public BigInteger Number { get; }
        public Field Field { get; }
        
        BigInteger Prime => Field.Prime;

        public FieldElement(BigInteger number, Field field)
        {
            if (number < 0 || number >= field.Prime)
            {
                throw new ArgumentOutOfRangeException($"Number {number} must be in range 0 to {field.Prime-1}");
            }
            Number = number;
            Field = field;
        }

        public FieldElement Add(FieldElement b)
        {
            ThrowIfNotSameField(this, b);
            return Field.Element((Number + b.Number) % Prime);
        }

        public FieldElement Subtract(FieldElement b)
        {
            ThrowIfNotSameField(this, b);
            var result = Number - b.Number;
            if (result < 0)
            {
                result += Prime;
            }
            return new FieldElement(result % Prime, Field);
        }

        public FieldElement Multiply(FieldElement b)
        {
            ThrowIfNotSameField(this, b);
            return new FieldElement((Number * b.Number) % Prime, Field);
        }

        public FieldElement Divide(FieldElement b)
        {
            ThrowIfNotSameField(this, b);
            return Multiply(b ^ (Prime - 2));
        }

        public FieldElement Power(BigInteger exponent)
        {
            while (exponent < 0) // TODO take out while by using mod
            {
                exponent += Prime - 1;
            }
            return Field.Element(BigInteger.ModPow(Number, exponent, Prime));
        }

        public FieldElement SquareRoot()
        {
            return new(BigInteger.ModPow(Number, (Field.Prime + 1) / 4, Field.Prime), Field);
        }

        public static FieldElement operator +(FieldElement a, FieldElement b) => a.Add(b);
        public static FieldElement operator -(FieldElement a, FieldElement b) => a.Subtract(b);
        public static FieldElement operator *(FieldElement a, FieldElement b) => a.Multiply(b);
        public static FieldElement operator /(FieldElement a, FieldElement b) => a.Divide(b);
        public static FieldElement operator ^(FieldElement a, BigInteger exponent) => a.Power(exponent);

        static bool SameField(params FieldElement[] elements)
        {
            foreach (var element in elements)
            {
                if (element.Prime != elements[0].Prime)
                {
                    return false;
                }
            }
            return true;
        }

        public static void ThrowIfNotSameField(params FieldElement[] elements)
        {
            if (!SameField(elements)) throw new InvalidOperationException("Elements must be in the same field");
        }
    }
}
