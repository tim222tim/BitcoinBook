using System;
using System.Collections.Generic;
using System.Linq;

namespace BitcoinBook
{
    public class MerkleTree
    {
        public MerkleNode Root { get; }
        public int Depth => GetDepth(Root);
        public int LeafCount => GetLeafCount(Root);

        public MerkleTree(int leafCount)
        {
            if (leafCount < 1) throw new ArgumentException("leafCount must be > 0", nameof(leafCount));

            Root = CreateTree(leafCount);
        }

        public MerkleTree(IEnumerable<byte[]> hashes)
        {
            var hashList = hashes?.ToList() ?? throw new ArgumentNullException(nameof(hashes));
            CheckHashes(hashList, false);

            Root = CreateTree(hashList.Select(v => new MerkleNode(v)));
        }

        public MerkleTree(int leafCount, MerkleProof proof)
        {
            if (leafCount < 1) throw new ArgumentException("leafCount must be > 0", nameof(leafCount));

            Root = CreateTree(leafCount);
            Populate(Root,  proof);

            // TODO make sure queues are empty
        }

        void Populate(MerkleNode node, MerkleProof proof)
        {
            if (node != null)
            {
                node.Hash = proof.Flags.Dequeue() ? proof.ProofHashes.Dequeue() :
                    node.IsLeaf ?  proof.IncludedHashes.Dequeue() :
                    ComputeHashFromChildren(node, proof);
            }
        }

        byte[] ComputeHashFromChildren(MerkleNode node, MerkleProof proof)
        {
            Populate(node.Left, proof);
            Populate(node.Right, proof);
            return GetParentHash(node.Left, node.Right);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        void CheckHashes(List<byte[]> hashes, bool isEmptyAllowed)
        {
            if (!(isEmptyAllowed || hashes.Any())) throw new ArgumentException("Must contain at least one hash", nameof(hashes));
            if (hashes.Any(h => h == null || h.Length != 32)) throw new ArgumentException("All hashes must be 32 bytes", nameof(hashes));
            if (!Unique(hashes)) throw new ArgumentException("All hashes must be unique", nameof(hashes));
        }

        MerkleNode CreateTree(int leafCount)
        {
            var nodes = new List<MerkleNode>();
            while (leafCount-- > 0)
            {
                nodes.Add(new MerkleNode());
            }
            return CreateTree(nodes);
        }

        MerkleNode CreateTree(IEnumerable<MerkleNode> leafNodes)
        {
            var nodes = leafNodes.ToList();

            while (nodes.Count > 1)
            {
                if (nodes.Count > 1 && nodes.Count % 2 == 1)
                {
                    nodes.Add(null);
                }
                var parents = new List<MerkleNode>();
                for (var i = 0; i < nodes.Count; i += 2)
                {
                    parents.Add(new MerkleNode(GetParentHash(nodes[i], nodes[i + 1]), nodes[i], nodes[i + 1]));
                }

                nodes = parents;
            }

            return nodes[0];
        }

        byte[] GetParentHash(MerkleNode left, MerkleNode right)
        {
            return left.Hash == null ? null : Merkler.ComputeParent(left.Hash, right?.Hash ?? left.Hash);
        }

        int GetDepth(MerkleNode node)
        {
            return node == null ? 0 : 1 + GetDepth(node.Left);
        }

        int GetLeafCount(MerkleNode node)
        {
            if (node == null)
            {
                return 0;
            }
            var childCount = GetLeafCount(node.Left) + GetLeafCount(node.Right);
            return childCount == 0 ? 1 : childCount;
        }

        bool Unique(IEnumerable<byte[]> valueList)
        {
            var hashSet = new HashSet<string>();
            return valueList.Select(b => b.ToHex()).All(hashSet.Add);
        }
    }
}
