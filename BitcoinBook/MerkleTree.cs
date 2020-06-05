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

        public MerkleTree(IEnumerable<byte[]> values)
        {
            var valueList = values?.ToList() ?? throw new ArgumentNullException(nameof(values));
            if (!valueList.Any()) throw new ArgumentException("Must contain at least one hash", nameof(values));
            if (valueList.Any(h => h == null || h.Length != 32)) throw new ArgumentException("All hashes must be 32 bytes", nameof(values));
            if (!Unique(valueList)) throw new ArgumentException("All hashes must be unique", nameof(values));

            Root = CreateTree(valueList.Select(v => new MerkleNode(v)));
        }

        MerkleNode CreateTree(int leafCount)
        {
            return CreateTree(Enumerable.Repeat(new MerkleNode(), leafCount));
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
                    parents.Add(new MerkleNode(GetParentValue(nodes[i], nodes[i + 1]), nodes[i], nodes[i + 1]));
                }

                nodes = parents;
            }

            return nodes[0];
        }

        byte[] GetParentValue(MerkleNode left, MerkleNode right)
        {
            return left.Value == null ? null : Merkler.ComputeParent(left.Value, right?.Value ?? left.Value);
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
