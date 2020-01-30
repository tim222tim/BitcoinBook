using System;
using System.Globalization;
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

        public bool Verify(string data, Signature signature)
        {
            return Verify(Cipher.Hash256Int(data), signature);
        }

        byte[] ToSecUncompressed()
        {
            var bytes = new byte[1 + 32 + 32];
            bytes[0] = 4;
            Cipher.ToBytes32(Key.X.Number).CopyTo(bytes, 1);
            Cipher.ToBytes32(Key.Y.Number).CopyTo(bytes, 1 + 32);
            return bytes;
        }

        byte[] ToSecCompressed()
        {
            var bytes = new byte[1 + 32];
            bytes[0] = Key.Y.Number.IsEven ? (byte)2 : (byte)3;
            Cipher.ToBytes32(Key.X.Number).CopyTo(bytes, 1);
            return bytes;
        }

        public byte[] ToSec(bool compressed = true)
        {
            return compressed ? ToSecCompressed() : ToSecUncompressed();
        }

        public string ToSecString(bool compressed = true)
        {
            return Cipher.ToHex(ToSec(compressed));
        }

        public string ToAddress(bool compressed = true, bool testnet = false)
        {
            var hash = Cipher.Hash160(ToSec(compressed));
            var address = new byte[hash.Length + 1];
            address[0] = testnet ? (byte) '\x6f' : (byte)0;
            hash.CopyTo(address, 1);
            return Cipher.ToBase58Check(address);
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
