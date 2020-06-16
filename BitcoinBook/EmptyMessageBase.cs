using System;

namespace BitcoinBook
{
    public abstract class EmptyMessageBase : MessageBase
    {
        public static T Parse<T>(byte[] bytes) where T : EmptyMessageBase, new()
        {
            var message = new T();
            if (bytes == null) throw  new ArgumentNullException(nameof(bytes));
            if (bytes.Length != 0) throw new FormatException($"{message.Command} payload should be zero bytes");
            return message;
        }

        public override byte[] ToBytes()
        {
            return new byte[0];
        }
    }
}