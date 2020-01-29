namespace BitcoinBook
{
    public class ScriptPubKey
    {
        public byte[] Bytes { get; }

        public ScriptPubKey(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}