using System;
using System.Globalization;
using System.Numerics;

namespace BitcoinBook
{
    public class Signature : IEquatable<Signature>
    {
        public BigInteger R { get; }
        public BigInteger S { get; }

        public Signature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }

        public Signature(string r, string s, NumberStyles numberStyles = NumberStyles.HexNumber) :
            this(BigInteger.Parse(r, numberStyles), BigInteger.Parse(s, numberStyles))
        {
        }

        public static Signature FromDer(string der)
        {
            return FromDer(Cipher.ToBytes(der));
        }

        public static Signature FromDer(byte[] der)
        {
            if (der == null) throw new ArgumentNullException(nameof(der));
            if (der.Length < 8) throw new FormatException("value not long enough");
            if (der[0] != 0x30) throw new FormatException("wrong DER prefix"); 
            if (der[1] != der.Length - 2) throw new FormatException("wrong length");
            var rBytes = GetPrefixedBytes(der, 2);
            var sBytes = GetPrefixedBytes(der, rBytes.Length + 4);
            return new Signature(Cipher.ToBigInteger(rBytes), Cipher.ToBigInteger(sBytes));
        }

        static byte[] GetPrefixedBytes(byte[] bytes, int index)
        {
            if (bytes[index] != 0x02) throw new FormatException("wrong number prefix");
            var length = bytes[index + 1];
            if (index + length > bytes.Length) throw new FormatException("value not long enough for number");
            var intBytes = new byte[length];
            Array.Copy(bytes, index + 2, intBytes, 0, length);
            return intBytes;
        }

        public override string ToString()
        {
            return $"Signature({R:X},{S:x})";
        }

        public string ToDerString()
        {
            return Cipher.ToHex(ToDer());
        }

        public byte[] ToDer()
        {
            var rBytes = ToPrefixedBytes(R);
            var sBytes = ToPrefixedBytes(S);
            var der = new byte[rBytes.Length + sBytes.Length + 2];
            der[0] = 0x30;
            der[1] = (byte) (rBytes.Length + sBytes.Length);
            rBytes.CopyTo(der, 2);
            sBytes.CopyTo(der, rBytes.Length + 2);
            return der;
        }

        byte[] ToPrefixedBytes(BigInteger i)
        {
            var bytes = Cipher.ToBytesSigned(i);
            return Cipher.Concat(new byte[] {0x02, (byte) bytes.Length}, bytes);
        }

        public bool Equals(Signature other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return R.Equals(other.R) && S.Equals(other.S);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Signature) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(R, S);
        }
    }
}
