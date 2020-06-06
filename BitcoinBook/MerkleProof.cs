using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BitcoinBook
{
    public class MerkleProof
    {
        public int LeafCount { get; }
        public Queue<byte[]> IncludedHashes { get; }
        public Queue<byte[]> ProofHashes { get; }
        public Queue<bool> Flags { get; }

        public MerkleProof(int leafCount)
        {
            if (leafCount < 1) throw new ArgumentException("leafCount must be > 0", nameof(leafCount));

            LeafCount = leafCount;
            IncludedHashes = new Queue<byte[]>();
            ProofHashes = new Queue<byte[]>();
            Flags = new Queue<bool>();
        }

        public MerkleProof(int leafCount, IEnumerable<byte[]> includedHashes, IEnumerable<byte[]> proofHashes, IEnumerable<bool> flags)
        {
            if (leafCount < 1) throw new ArgumentException("leafCount must be > 0", nameof(leafCount));
            var includedHashList = includedHashes?.ToList() ?? throw new ArgumentNullException(nameof(includedHashes));
            CheckHashes(includedHashList, false);
            var proofHashList = proofHashes?.ToList() ?? throw new ArgumentNullException(nameof(proofHashes));
            CheckHashes(proofHashList, true);

            LeafCount = leafCount;
            IncludedHashes = new Queue<byte[]>(includedHashList);
            ProofHashes = new Queue<byte[]>(proofHashList);
            Flags = new Queue<bool>(flags ?? throw new ArgumentNullException(nameof(flags)));
        }

        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        void CheckHashes(List<byte[]> hashes, bool isEmptyAllowed)
        {
            if (!(isEmptyAllowed || hashes.Any())) throw new ArgumentException("Must contain at least one hash", nameof(hashes));
            if (hashes.Any(h => h == null || h.Length != 32)) throw new ArgumentException("All hashes must be 32 bytes", nameof(hashes));
        }
    }
}
