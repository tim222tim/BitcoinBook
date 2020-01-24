﻿using System.Globalization;
using System.Numerics;

namespace BitcoinBook
{
    public class PublicKey
    {
        public Point Key { get; }

        public PublicKey(Point key)
        {
            Key = key;
        }

        public PublicKey(string x, string y, NumberStyles numberStyles = NumberStyles.HexNumber)
        {
            Key = S256Curve.Point(x, y, numberStyles);
        }

        public bool Verify(BigInteger hash, Signature signature)
        {
            var sinv = BigInteger.ModPow(signature.S, S256Curve.Order - 2, S256Curve.Order);
            var u = BigInteger.Remainder(hash * sinv, S256Curve.Order);
            var v = BigInteger.Remainder(signature.R * sinv, S256Curve.Order);
            return (S256Curve.Generator * u + Key * v).X.Number == signature.R;
        }

        public bool Verify(string hash, Signature signature, NumberStyles numberStyles = NumberStyles.HexNumber)
        {
            return Verify(BigInteger.Parse(hash, numberStyles), signature);
        }

        public override string ToString()
        {
            return Key.ToString();
        }
    }
}
