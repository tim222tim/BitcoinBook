namespace BitcoinBook
{
    public class MerkleNode
    {
        public byte[] Hash { get; }
        public MerkleNode? Left { get; }
        public MerkleNode? Right { get; }
        public bool IsLeaf => Left == null && Right == null;

        public MerkleNode(byte[] hash) : this(hash, null, null)
        {
        }

        public MerkleNode(byte[] hash, MerkleNode? left, MerkleNode? right)
        {
            Hash = hash;
            Left = left;
            Right = right;
        }
    }
}
