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

            var xBytes = new byte[32];
            Array.Copy(sec, 1, xBytes, 0, 32);
            var xInt = Cipher.ToBigInteger(xBytes);
            if (prefix == 0x04)
            {
                var yBytes = new byte[32];
                Array.Copy(sec, 33, yBytes, 0, 32);
                return new PublicKey(xInt, Cipher.ToBigInteger(yBytes));
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
