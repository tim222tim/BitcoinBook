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
            ThrowIfNotSameField(this, b);
            return new FieldElement((Number + b.Number) % Prime, Prime);
        }

        public FieldElement Subtract(FieldElement b)
        {
            ThrowIfNotSameField(this, b);
            var result = Number - b.Number;
            if (result < 0)
            {
                result += Prime;
            }
            return new FieldElement(result % Prime, Prime);
        }

        public FieldElement Multiply(FieldElement b)
        {
            ThrowIfNotSameField(this, b);
            return new FieldElement((Number * b.Number) % Prime, Prime);
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
            return new FieldElement(BigInteger.ModPow(Number, exponent, Prime), Prime);
        }

        public static bool operator ==(FieldElement a, FieldElement b) => a.Equals(b);
        public static bool operator !=(FieldElement a, FieldElement b) => !a.Equals(b);
        public static FieldElement operator +(FieldElement a, FieldElement b) => a.Add(b);
        public static FieldElement operator -(FieldElement a, FieldElement b) => a.Subtract(b);
        public static FieldElement operator *(FieldElement a, FieldElement b) => a.Multiply(b);
        public static FieldElement operator /(FieldElement a, FieldElement b) => a.Divide(b);
        public static FieldElement operator ^(FieldElement a, BigInteger exponent) => a.Power(exponent);

        public bool Equals(FieldElement other)
        {
            return Number.Equals(other.Number) && Prime.Equals(other.Prime);
        }

        public override bool Equals(object obj)
        {
            return obj is FieldElement other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Number.GetHashCode() * 397) ^ Prime.GetHashCode();
            }
        }

        public static bool SameField(params FieldElement[] elements)
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
