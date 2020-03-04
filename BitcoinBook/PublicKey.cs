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

        public PublicKey(BigInteger x, BigInteger y) : this(S256Curve.Point(x, y))
        {
        }

        public PublicKey(PrivateKey privateKey) : this(S256Curve.Generator * privateKey.Key)
        {
        }

        public static PublicKey FromSec(byte[] sec)
        {
            if (sec == null || sec.Length == 0)
            {
                sec = new byte[] {0x00};
            }

            var prefix = sec[0];
            if (sec.Length != GetValidLength(prefix))
            {
                throw new FormatException("Invalid SEC format");
            }

            var xBytes = sec.Copy(1, 32);
            var xInt = xBytes.ToBigInteger();
            if (prefix == 0x04)
            {
                var yBytes = sec.Copy(33, 32);
                return new PublicKey(xInt, yBytes.ToBigInteger());
            }

            var isEven = prefix == 0x02;
            var xField = S256Curve.Field.Element(xInt);
            var alpha = (xField ^ 3) + S256Curve.Curve.B;
            var beta = alpha.SquareRoot();
            var evenBeta = beta.Number % 2 == 0 ? beta : S256Curve.Field.Element(S256Curve.Field.Prime - beta.Number);
            var oddBeta = beta.Number % 2 == 0 ? S256Curve.Field.Element(S256Curve.Field.Prime - beta.Number) : beta;

            return new PublicKey(S256Curve.Point(xField, isEven ? evenBeta : oddBeta));
        }

        public static PublicKey FromSec(string sec)
        {
            return FromSec(Cipher.ToBytes(sec ?? ""));
        }

        static int GetValidLength(byte prefix)
        {
            switch (prefix)
            {
                case 0x02:
                case 0x03:
                    return 33;
                case 0x04:
                    return 65;
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

        public bool Verify(byte[] hash, Signature signature)
        {
            return Verify(hash.ToBigInteger(), signature);
        }

        public bool Verify(string data, Signature signature)
        {
            return Verify(Cipher.Hash256Int(data), signature);
        }

        byte[] ToSecUncompressed()
        {
            var bytes = new byte[1 + 32 + 32];
            bytes[0] = 4;
            Key.X.Number.ToBigBytes32().CopyTo(bytes, 1);
            Key.Y.Number.ToBigBytes32().CopyTo(bytes, 1 + 32);
            return bytes;
        }

        byte[] ToSecCompressed()
        {
            var bytes = new byte[1 + 32];
            bytes[0] = Key.Y.Number.IsEven ? (byte)2 : (byte)3;
            Key.X.Number.ToBigBytes32().CopyTo(bytes, 1);
            return bytes;
        }

        public byte[] ToSec(bool compressed = true)
        {
            return compressed ? ToSecCompressed() : ToSecUncompressed();
        }

        public string ToSecString(bool compressed = true)
        {
            return ToSec(compressed).ToHex();
        }

        public string ToAddress(bool compressed = true, bool testnet = false)
        {
            return Cipher.ToAddress(testnet ? (byte) '\x6f' : (byte) 0, ToHash160(compressed));
        }

        public byte[] ToHash160(bool compressed = true)
        {
            return Cipher.Hash160(ToSec(compressed));
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

        public static bool operator ==(PublicKey a, PublicKey b) => a?.Equals(b) ?? ReferenceEquals(null, b);
        public static bool operator !=(PublicKey a, PublicKey b) => !a?.Equals(b) ?? !ReferenceEquals(null, b);
    }
}
