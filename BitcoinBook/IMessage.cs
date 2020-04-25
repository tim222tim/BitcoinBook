namespace BitcoinBook
{
    public interface IMessage
    {
        string Command { get; }
        byte[] ToBytes();
    }
}