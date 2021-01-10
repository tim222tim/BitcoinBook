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

        public MerkleTree(IEnumerable<byte[]> hashes)
        {
            if (hashes == null) throw new ArgumentNullException(nameof(hashes));
            var hashList = hashes.ToList();
            CheckHashes(hashList, false);

            Root = CreateTree(hashList.Select(h => new MerkleNode(h)));
        }

        public MerkleTree(MerkleProof proof)
        {
            if (proof == null) throw new ArgumentNullException(nameof(proof));

            Root = CreateTree(proof, DepthNeeded(proof.LeafCount));

            CheckForLeftovers(proof.ProofHashes, nameof(proof.ProofHashes));
            CheckForLeftovers(proof.IncludedHashes, nameof(proof.IncludedHashes));
            CheckForLeftovers(proof.Flags, nameof(proof.Flags));
        }

        public MerkleProof CreateProof(IEnumerable<byte[]> includedHashes)
        {
            if (includedHashes == null) throw new ArgumentNullException(nameof(includedHashes));
            var includedSet = new HashSet<string>(includedHashes.Select(b => b.ToHex()));
            if (!includedSet.Any()) throw new ArgumentException("Included hashes must not be empty", nameof(includedHashes));

            var proof = new MerkleProof(LeafCount);
            AddToProof(Root, proof, includedSet);

            if (proof.IncludedHashes.Count != includedSet.Count)
            {
                throw new InvalidOperationException("Not all included hashes were found in the tree");
            }

            return proof;
        }

        void AddToProof(MerkleNode? node, MerkleProof proof, HashSet<string> includedSet)
        {
            if (node != null)
            {
                if (ContainsAny(node, includedSet))
                {
                    proof.Flags.Enqueue(false);
                    if (node.IsLeaf)
                    {
                        proof.IncludedHashes.Enqueue(node.Hash);
                    }
                    AddToProof(node.Left, proof, includedSet);
                    AddToProof(node.Right, proof, includedSet);
                }
                else
                {
                    proof.Flags.Enqueue(true);
                    proof.ProofHashes.Enqueue(node.Hash);
                }
            }
        }

        bool ContainsAny(MerkleNode? node, HashSet<string> includedSet)
        {
            return node != null && (includedSet.Contains(node.Hash.ToHex()) || ContainsAny(node.Left, includedSet) || ContainsAny(node.Right, includedSet));
        }

        MerkleNode CreateTree(MerkleProof proof, int depth)
        {
            if (proof.Flags.Dequeue())
            {
                return new(proof.ProofHashes.Dequeue());
            }

            if (depth == 1)
            {
                return new(proof.IncludedHashes.Dequeue());
            }

            var left = CreateTree(proof, depth - 1);
            var right = proof.Flags.Any() ? CreateTree(proof, depth - 1) : null;
            return new(GetParentHash(left, right), left, right);
        }

        void CheckForLeftovers<T>(IEnumerable<T> items, string name)
        {
            if (items.Any()) throw new InvalidOperationException($"{name} has unused values");
        }

        void CheckHashes(List<byte[]> hashes, bool isEmptyAllowed)
        {
            if (!(isEmptyAllowed || hashes.Any())) throw new ArgumentException("Must contain at least one hash", nameof(hashes));
            if (hashes.Any(h => h == null || h.Length != 32)) throw new ArgumentException("All hashes must be 32 bytes", nameof(hashes));
            if (!Unique(hashes)) throw new ArgumentException("All hashes must be unique", nameof(hashes));
        }

        MerkleNode CreateTree(IEnumerable<MerkleNode> leafNodes)
        {
            var nodes = new List<MerkleNode>(leafNodes);

            while (nodes.Count > 1)
            {
                var parents = new List<MerkleNode>();
                for (var i = 0; i < nodes.Count; i += 2)
                {
                    var left = nodes[i];
                    var right = i + 1 < nodes.Count ? nodes[i + 1] : null;
                    parents.Add(new MerkleNode(GetParentHash(left, right), left, right));
                }

                nodes = parents;
            }

            return nodes[0];
        }

        byte[] GetParentHash(MerkleNode left, MerkleNode? right)
        {
            return Merkler.ComputeParent(left.Hash, right?.Hash ?? left.Hash);
        }

        int GetDepth(MerkleNode? node)
        {
            return node == null ? 0 : 1 + GetDepth(node.Left);
        }

        int GetLeafCount(MerkleNode? node)
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

        int DepthNeeded(int leafCount)
        {
            var leaves = 1;
            var depth = 1;
            while (leafCount > leaves)
            {
                leaves *= 2;
                depth += 1;
            }

            return depth;
        }
    }
}
