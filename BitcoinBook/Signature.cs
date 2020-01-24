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
    }
}
