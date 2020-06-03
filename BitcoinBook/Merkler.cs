using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BitcoinBook
{
    public class Merkler
    {
        public byte[] ComputeParent(byte[] hash1, byte[] hash2)
        {
            return Cipher.Hash256(hash1.Concat(hash2));
        }

        public List<byte[]> ComputeLevelParent(IEnumerable<byte[]> hashes)
        {
            var hashList = hashes.ToList();
            if (hashList.Count % 2 == 1)
            {
                hashList.Add(hashList[^1]);
            }
            var parentHashes = new List<byte[]>();
            for (var i = 0; i < hashList.Count; i += 2)
            {
                parentHashes.Add(ComputeParent(hashList[i], hashList[i+1]));
            }

            return parentHashes;
        }

        public byte[] ComputeMerkleRoot(IEnumerable<byte[]> hashes)
        {
            if (hashes == null) throw new ArgumentNullException(nameof(hashes));
            var hashList = hashes.ToList();
            if (!hashList.Any()) throw new ArgumentException("Must contain at least one hash", nameof(hashes));
            if (hashList.Any(h => h == null || h.Length != 32)) throw new ArgumentException("All hashes must be 32 bytes", nameof(hashes));
            if (!Unique(hashList)) throw new ArgumentException("All hashes must be unique", nameof(hashes));

            while (hashList.Count > 1)
            {
                hashList = ComputeLevelParent(hashList);    
            }

            return hashList[0];
        }

        bool Unique(IEnumerable<byte[]> hashList)
        {
            var hashSet = new HashSet<string>();
            return hashList.Select(b => b.ToHex()).All(hashSet.Add);
        }
    }
}
