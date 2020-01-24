using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace BitcoinBook
{
    public class PrivateKey
    {
        static readonly RandomBigInteger random = new RandomBigInteger();

        public BigInteger Key { get; }
        public PublicKey PublicKey { get; }

        public PrivateKey(BigInteger key)
        {
            Key = key;
            PublicKey = new PublicKey(this);
        }

        public PrivateKey(string key, NumberStyles numberStyles = NumberStyles.HexNumber) :
            this(BigInteger.Parse(key, numberStyles))
        {
        }

        public PrivateKey() : this(random.NextBigInteger(S256Curve.Field.Prime))
        {
        }

        public Signature Sign(BigInteger hash)
        {
            var k = random.NextBigInteger(S256Curve.Field.Prime);
            var r = (S256Curve.Generator * k).X.Number;
            var kinv = BigInteger.ModPow(k, S256Curve.Order - 2, S256Curve.Order);
            var s = BigInteger.Remainder((hash + r * Key) * kinv, S256Curve.Order);
            return new Signature(r, s);
        }

        public Signature Sign(byte[] data)
        {
            return Sign(S256Curve.ComputeHash(data));
        }

        public Signature Sign(string data)
        {
            return Sign(S256Curve.ComputeHash(data));
        }
    }
}
