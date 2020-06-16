namespace BitcoinBook
{
    public abstract class MessageBase : IMessage
    {
        public abstract string Command { get; }
        public abstract byte[] ToBytes();
    }
}