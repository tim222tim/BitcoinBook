using System.Collections.Generic;
using System.Linq;

namespace BitcoinBook
{
    public class Merkle
    {
        public static byte[] ComputeParent(byte[] hash1, byte[] hash2)
        {
            return Cipher.Hash256(hash1.Concat(hash2));
        }

        public static IList<byte[]> ComputeLevelParent(IEnumerable<byte[]> hashes)
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
    }
}
