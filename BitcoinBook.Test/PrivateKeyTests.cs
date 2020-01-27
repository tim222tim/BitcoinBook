﻿using System.Numerics;
using Xunit;

namespace BitcoinBook.Test
{
    public class PrivateKeyTests
    {
        [Fact]
        public void SignHashTest()
        {
            var privateKey = new PrivateKey();
            var hash = 2384234;
            var signature = privateKey.Sign(hash);
            Assert.True(privateKey.PublicKey.Verify(hash, signature));
        }

        [Fact]
        public void SignDataTest()
        {
            var privateKey = new PrivateKey(12345);
            var data = "Programming Bitcoin!";
            var hash = Cipher.ComputeHash256Int(data);
            var hashStr = $"{hash:X64}";
            var hash160 = Cipher.ComputeHash160String(data);
            var signature = privateKey.Sign(data);
            Assert.True(privateKey.PublicKey.Verify(data, signature));
        }
    }
}
