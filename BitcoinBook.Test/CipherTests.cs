﻿using System;
using Xunit;

namespace BitcoinBook.Test
{
    public class CipherTests
    {
        [Theory]
        [InlineData("9MA8fRQrT4u8Zj8ZRd6MAiiyaxb2Y1CMpvVkHQu5hVM6", "7c076ff316692a3d7eb3c3bb0f8b1488cf72e1afcd929e29307032997a838a3d")]
        [InlineData("4fE3H2E6XMp4SsxtwinF7w9a34ooUrwWe4WsW1458Pd", "eff69ef2b1bd93a66ed5219add4fb51e11a840f404876325a1e8ffe0529a2c")]
        [InlineData("EQJsjkd6JaGwxrjEhfeqPenqHwrBmPQZjJGNSCHBkcF7", "c7207fee197d27c618aea621406f6bf5ef6fca38681d82b2f06fddbdce6feab6")]
        public void ToBase58Test(string expected, string input)
        {
            Assert.Equal(expected, Cipher.ToBase58(HexToByteArray(input)));
        }

        [Theory]
        [InlineData("7c076ff316692a3d7eb3c3bb0f8b1488cf72e1afcd929e29307032997a838a3d", "9MA8fRQrT4u8Zj8ZRd6MAiiyaxb2Y1CMpvVkHQu5hVM6")]
        [InlineData("eff69ef2b1bd93a66ed5219add4fb51e11a840f404876325a1e8ffe0529a2c", "4fE3H2E6XMp4SsxtwinF7w9a34ooUrwWe4WsW1458Pd")]
        [InlineData("c7207fee197d27c618aea621406f6bf5ef6fca38681d82b2f06fddbdce6feab6", "EQJsjkd6JaGwxrjEhfeqPenqHwrBmPQZjJGNSCHBkcF7")]
        public void FromBase58Test(string expected, string input)
        {
            Assert.Equal(HexToByteArray(expected), Cipher.FromBase58(input));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("9MA8fRQrT4u8Zj8ZRd6MAiiyaxb2Y1CMpvVkHQu5hVM60")]
        [InlineData("kjsdl")]
        public void FormatExceptionTest(string input)
        {
            Assert.Throws<FormatException>(() => Cipher.FromBase58(input));
        }

        public static byte[] HexToByteArray(string hex)
        {
            var length = hex.Length;
            var bytes = new byte[length / 2];
            for (var i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}
