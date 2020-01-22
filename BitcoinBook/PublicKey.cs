using System.Globalization;
using System.Numerics;

namespace BitcoinBook
{
    public class PublicKey
    {
        public S256Point Key { get; }

        public PublicKey(S256Point key)
        {
            Key = key;
        }

        public PublicKey(string x, string y, NumberStyles numberStyles = NumberStyles.HexNumber)
        {
            Key = new S256Point(x, y, numberStyles);
        }

        public bool VerifySignature(BigInteger hash, BigInteger r, BigInteger s)
        {
            var sinv = BigInteger.ModPow(s, S256Curve.Order - 2, S256Curve.Order);
            var u = BigInteger.Remainder(hash * sinv, S256Curve.Order);
            var v = BigInteger.Remainder(r * sinv, S256Curve.Order);
            return (S256Curve.Generator * u + Key * v).X.Number == r;
        }

        public bool VerifySignature(string hash, string r, string s, NumberStyles numberStyles = NumberStyles.HexNumber)
        {
            return VerifySignature(
                BigInteger.Parse(hash, numberStyles),
                BigInteger.Parse(r, numberStyles),
                BigInteger.Parse(s, numberStyles)
            );
        }
    }
}
