using System.Globalization;
using System.Numerics;

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
            var k = GetK();
            var r = (S256Curve.Generator * k).X.Number;
            var kinv = BigInteger.ModPow(k, S256Curve.Order - 2, S256Curve.Order);
            var s = BigInteger.Remainder((hash + r * Key) * kinv, S256Curve.Order);
            if (s > S256Curve.Order / 2)
            {
                s = S256Curve.Order - s;
            }
            return new Signature(r, s);
        }

        BigInteger GetK() // TODO should be changed to deterministic K
        {
            return random.NextBigInteger(S256Curve.Field.Prime);
        }

        public Signature Sign(byte[] data)
        {
            return Sign(Cipher.Hash256Int(data));
        }

        public Signature Sign(string data)
        {
            return Sign(Cipher.Hash256Int(data));
        }

        public string Wif(bool compressed = true, bool testnet = false)
        {
            var length = compressed ? 34 : 33;
            var bytes = new byte[length];
            bytes[0] = testnet ? (byte)'\xef' : (byte)'\x80';
            Key.ToBigBytes32().CopyTo(bytes, 1);
            if (compressed)
            {
                bytes[length - 1] = 1;
            }

            return Cipher.ToBase58Check(bytes);
        }
    }
}
