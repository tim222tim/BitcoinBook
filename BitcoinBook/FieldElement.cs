﻿using System;
using System.Numerics;

namespace BitcoinBook
{
    public struct FieldElement
    {
        public BigInteger Number { get; }
        public BigInteger Prime { get; }

        public FieldElement(BigInteger number, BigInteger prime)
        {
            if (number < 0 || number >= prime)
            {
                throw new ArgumentOutOfRangeException($"Number {number} must be in range 0 to {prime-1}");
            }
            Number = number;
            Prime = prime;
        }

        public FieldElement Add(FieldElement b)
        {
            CheckField(b);
            return new FieldElement((Number + b.Number) % Prime, Prime);
        }

        public FieldElement Subtract(FieldElement b)
        {
            CheckField(b);
            var result = Number - b.Number;
            if (result < 0)
            {
                result += Prime;
            }
            return new FieldElement(result % Prime, Prime);
        }

        public FieldElement Multiply(FieldElement b)
        {
            CheckField(b);
            return new FieldElement((Number * b.Number) % Prime, Prime);
        }

        public FieldElement Divide(FieldElement b)
        {
            CheckField(b);
            return Multiply(b ^ (Prime - 2));
        }

        public FieldElement Power(BigInteger exponent)
        {
            if (exponent < 0)
            {
                return Power(Prime + exponent - 1);
            }
            return new FieldElement(BigInteger.ModPow(Number, exponent, Prime), Prime);
        }

        public static FieldElement operator +(FieldElement a, FieldElement b) => a.Add(b);
        public static FieldElement operator -(FieldElement a, FieldElement b) => a.Subtract(b);
        public static FieldElement operator *(FieldElement a, FieldElement b) => a.Multiply(b);
        public static FieldElement operator /(FieldElement a, FieldElement b) => a.Divide(b);
        public static FieldElement operator ^(FieldElement a, BigInteger exponent) => a.Power(exponent);

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        void CheckField(FieldElement element)
        {
            if (element.Prime != Prime) throw new InvalidOperationException("Numbers must bein the same field");
        }
    }
}
