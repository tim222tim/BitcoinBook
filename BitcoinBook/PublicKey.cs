using System;
using System.Globalization;
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

        public PublicKey(string x, string y, NumberStyles numberStyles = NumberStyles.HexNumber) :
            this(S256Curve.Point(x, y, numberStyles))
        {
        }

        public PublicKey(PrivateKey privateKey) : this(S256Curve.Generator * privateKey.Key)
        {
        }

        public bool Verify(BigInteger hash, Signature signature)
        {
            var sinv = BigInteger.ModPow(signature.S, S256Curve.Order - 2, S256Curve.Order);
            var u = BigInteger.Remainder(hash * sinv, S256Curve.Order);
            var v = BigInteger.Remainder(signature.R * sinv, S256Curve.Order);
            return (S256Curve.Generator * u + Key * v).X.Number == signature.R;
        }

        public bool Verify(string data, Signature signature, NumberStyles numberStyles = NumberStyles.HexNumber)
        {
            return Verify(S256Curve.ComputeHash(data), signature);
        }

        public string ToSecFormat()
        {
            return $"04{Key.X.Number}{Key.Y.Number}";
        }

        string ToHex(BigInteger i)
        {
            return $"{i:X64}";
        }

        public override string ToString()
        {
            return Key.ToString();
        }
    }
}
