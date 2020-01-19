using System;

namespace BitcoinBook
{
    public struct FieldElement
    {
        public long Number { get; }
        public long Prime { get; }

        public FieldElement(long number, long prime)
        {
            if (number < 0 || number >= prime)
            {
                throw new ArgumentOutOfRangeException($"Number {number} must be in range 0 to {prime-1}");
            }
            Number = number;
            Prime = prime;
        }

        public FieldElement Add(FieldElement element)
        {
            if (element.Prime != Prime) throw new InvalidOperationException("Cannot add two number in different fields");

            return new FieldElement((Number + element.Number) % Prime, Prime);
        }

        public FieldElement Subtract(FieldElement element)
        {
            if (element.Prime != Prime) throw new InvalidOperationException("Cannot add two number in different fields");

            var result = Number - element.Number;
            if (result < 0)
            {
                result += Prime;
            }
            return new FieldElement(result % Prime, Prime);
        }

        public static FieldElement operator +(FieldElement a, FieldElement b) => a.Add(b);
        public static FieldElement operator -(FieldElement a, FieldElement b) => a.Subtract(b);
    }
}
