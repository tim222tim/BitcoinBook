﻿using System;
using System.Numerics;

namespace BitcoinBook
{
    public class Curve
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
            Infinity = BitcoinBook.Point.Infinity(A, B);
        }

        public Curve(Field field, BigInteger a, BigInteger b) : this(field.Element(a), field.Element(b))
        {
        }

        public Point Point(FieldElement x, FieldElement y)
        {
            if (x.Field != y.Field) throw new InvalidOperationException("Numbers must be in the same field");

            return new Point(x, y, A, B);
        }

        public Point Point(BigInteger x, BigInteger y)
        {
            return Point(field.Element(x), field.Element(y));
        }

        public Point Infinity { get; }
    }
}
