using System;
using System.Data;
using System.Globalization;
using System.Numerics;

namespace BitcoinBook
{
    public class Signature
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
    }
}
