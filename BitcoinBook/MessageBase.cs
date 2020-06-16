﻿using System;
using System.IO;

namespace BitcoinBook
{
    public abstract class MessageBase : IMessage
    {
        public abstract string Command { get; }
        public abstract byte[] ToBytes();

        public static T Parse<T>(byte[] bytes, Func<ByteReader, T> parse)
        {
            var reader = new ByteReader(bytes);
            try
            {
                return parse(reader);
            }
            catch (EndOfStreamException ex)
            {
                throw new FormatException("Read past end of data", ex);
            }
        }
    }
}