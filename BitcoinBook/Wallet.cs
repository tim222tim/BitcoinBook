using System;
using System.Collections.Generic;
using System.Linq;

namespace BitcoinBook
{
    public class Wallet
    {
        readonly IList<PrivateKey> privateKeys;

        public Wallet() : this(new PrivateKey[0])
        {
        }

        public Wallet(IEnumerable<PrivateKey> privateKeys)
        {
            this.privateKeys = new List<PrivateKey>(privateKeys);
        }

        public PrivateKey? FindBy(PublicKey publicKey)
        {
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));
            return privateKeys.FirstOrDefault(k => k.PublicKey.Equals(publicKey));
        }

        public PrivateKey? FindByHash(byte[] hash)
        {
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            return privateKeys.FirstOrDefault(k => k.PublicKey.ToHash160().SequenceEqual(hash));
        }
    }
}
