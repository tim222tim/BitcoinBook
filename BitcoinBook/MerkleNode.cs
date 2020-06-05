namespace BitcoinBook
{
    public class MerkleNode
    {
        public byte[] Value { get; set; }
        public MerkleNode Left { get; }
        public MerkleNode Right { get; }
        public bool IsLeaf => Left == null && Right == null;

        public MerkleNode(byte[] value = null) : this(value, null, null)
        {
        }

        public MerkleNode(MerkleNode left, MerkleNode right) : this(null, left, right)
        {
        }

        public MerkleNode(byte[] value, MerkleNode left, MerkleNode right)
        {
            Value = value;
            Left = left;
            Right = right;
        }
    }
}
