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

            var nodes = Enumerable.Repeat(new MerkleNode(), leafCount).ToList();
            if (leafCount > 1 && leafCount % 2 == 1)
            {
                nodes.Add(null);
            }

            while (nodes.Count > 1)
            {
                var parents = new List<MerkleNode>();
                for (var i = 0; i < nodes.Count; i += 2)
                {
                    parents.Add(new MerkleNode(nodes[i], nodes[i+1]));
                }

                nodes = parents;
            }

            Root = nodes[0];
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
    }
}
