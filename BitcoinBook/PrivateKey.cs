using System;
using System.Globalization;
using System.Numerics;

namespace BitcoinBook
{
    public class PrivateKey : IEquatable<PrivateKey>
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

        public Signature Sign(byte[] hash)
        {
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            return Sign(hash.ToBigInteger());
        }
        
        Signature Sign(BigInteger hash)
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

        public Signature SignPreimage(byte[] data)
        {
            return Sign(Cipher.Hash256(data));
        }

        public Signature SignPreimage(string data)
        {
            return Sign(Cipher.Hash256(data));
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

        public bool Equals(PrivateKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Key.Equals(other.Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PrivateKey) obj);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public static bool operator ==(PrivateKey a, PrivateKey b) => a?.Equals(b) ?? ReferenceEquals(null, b);
        public static bool operator !=(PrivateKey a, PrivateKey b) => !a?.Equals(b) ?? !ReferenceEquals(null, b);
    }
}
