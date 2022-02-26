namespace BitcoinBook;

public class VerAckMessage : EmptyMessageBase
{
    public override string Command => "verack";
    public static VerAckMessage Parse(byte[] bytes) => Parse<VerAckMessage>(bytes);
}