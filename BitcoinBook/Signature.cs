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

        public string ToDerFormat()
        {
            var rs = ToPrefixedHex(R) + ToPrefixedHex(S);
            return $"30{rs.Length/2:X2}{rs}";
        }

        string ToPrefixedHex(BigInteger i)
        {
            var hex = ToHex(i);
            return $"02{hex.Length/2:X2}{hex}";
        }

        string ToHex(BigInteger i)
        {
            var hex = i.ToString("X64");
            if (hex.Length == 65 && hex.StartsWith("0"))
            {
                hex = '0' + hex;
            }
            return hex.ToLower();
        }
    }
}
