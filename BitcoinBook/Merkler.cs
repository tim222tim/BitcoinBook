using System;
using System.Collections.Generic;
using System.Linq;

namespace BitcoinBook
{
    public class Merkler
    {
        public byte[] ComputeParent(byte[] hash1, byte[] hash2)
        {
            return Cipher.Hash256(hash1.Concat(hash2));
        }

        public IList<byte[]> ComputeLevelParent(IEnumerable<byte[]> hashes)
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
            // TODO need to test inputs

            IList<byte[]> hashList = hashes.ToList();
            while (hashList.Count > 1)
            {
                hashList = ComputeLevelParent(hashList);
            }

            return hashList[0];
        }
    }
}
