namespace BitcoinBook;

public class SendHeadersMessage : EmptyMessageBase
{
    public override string Command => "sendheaders";
    public static SendHeadersMessage Parse(byte[] bytes) => Parse<SendHeadersMessage>(bytes);
}