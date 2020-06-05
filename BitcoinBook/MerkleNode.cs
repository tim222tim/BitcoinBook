namespace BitcoinBook
{
    public class MerkleNode
    {
        public byte[] Hash { get; set; }
        public MerkleNode Left { get; }
        public MerkleNode Right { get; }
        public bool IsLeaf => Left == null && Right == null;

        public MerkleNode(byte[] hash = null) : this(hash, null, null)
        {
        }

        public MerkleNode(MerkleNode left, MerkleNode right) : this(null, left, right)
        {
        }

        public MerkleNode(byte[] hash, MerkleNode left, MerkleNode right)
        {
            Hash = hash;
            Left = left;
            Right = right;
        }
    }
}
