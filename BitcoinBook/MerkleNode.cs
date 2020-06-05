namespace BitcoinBook
{
    public class MerkleNode
    {
        public string Value { get; set; }
        public MerkleNode Left { get; }
        public MerkleNode Right { get; }

        public MerkleNode() : this(null, null)
        {
        }

        public MerkleNode(MerkleNode left, MerkleNode right)
        {
            Left = left;
            Right = right;
        }
    }
}
