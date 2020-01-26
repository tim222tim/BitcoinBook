using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Numerics;

namespace BitcoinBook
{
    public class PublicKey : IEquatable<PublicKey>
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

        public static PublicKey ParseSecFormat(string sec)
        {
            if (sec == null || sec.Length < 2)
            {
                sec = "  ";
            }
            var prefix = sec[0] == '0' ? sec[1] : ' ';
            if (sec.Length != GetValidLength(prefix))
            {
                throw new FormatException("Invalid SEC format");
            }

            if (prefix == '4')
            {
                return new PublicKey("0" + sec.Substring(2, 64), "0" + sec.Substring(66));
            }

            var isEven = prefix == '2';
            var x = S256Curve.Field.Element(BigInteger.Parse("0" + sec.Substring(2), NumberStyles.HexNumber));
            var alpha = (x ^ 3) + S256Curve.Curve.B;
            var beta = alpha.SquareRoot();
            var evenBeta = beta.Number % 2 == 0 ? beta : S256Curve.Field.Element(S256Curve.Field.Prime - beta.Number);
            var oddBeta = beta.Number % 2 == 0 ? S256Curve.Field.Element(S256Curve.Field.Prime - beta.Number) : beta;

            return new PublicKey(S256Curve.Point(x, isEven ? evenBeta : oddBeta));
        }

        static int GetValidLength(char prefix)
        {
            switch (prefix)
            {
                case '2':
                case '3':
                    return 66;
                case '4':
                    return 130;
                default:
                    return -1;
            }
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
            return Verify(Cipher.ComputeHash256Int(data), signature);
        }

        public string ToSecUncompressedFormat()
        {
            return $"04{ToHex(Key.X.Number)}{ToHex(Key.Y.Number)}";
        }

        public string ToSecCompressedFormat()
        {
            return $"{(Key.Y.Number.IsEven ? "02" : "03")}{ToHex(Key.X.Number)}";
        }

        string ToHex(BigInteger i)
        {
            var hex = i.ToString("X64");
            if (hex.Length == 65 && hex.StartsWith("0"))
            {
                hex = hex.Substring(1);
            }
            return hex.ToLower();
        }

        public override string ToString()
        {
            return Key.ToString();
        }

        public bool Equals(PublicKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Key, other.Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PublicKey) obj);
        }

        public override int GetHashCode()
        {
            return (Key != null ? Key.GetHashCode() : 0);
        }
    }
}
