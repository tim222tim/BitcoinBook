namespace BitcoinBook
{
    public class ScriptSig
    {
        public byte[] Bytes { get; }

        public ScriptSig() : this(new byte[0])
        {
        }

        public ScriptSig(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}