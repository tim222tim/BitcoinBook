using System.Numerics;
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
            var hash = Cipher.ComputeHash256(data);
            var hashStr = $"{hash:X64}";
            var signature = privateKey.Sign(data);
            Assert.True(privateKey.PublicKey.Verify(data, signature));
        }

        [Fact]
        public void HashTest()
        {
            var i = new BigInteger(260);
            var b = i.ToByteArray();
            b = SuffixZeros(b, 2);
            Assert.Equal(260, new BigInteger(b));
        }

        static byte[] PrefixZeros(byte[] bytes, int count)
        {
            var newBytes = new byte[bytes.Length + count];
            for (var i = 0; i < count; i++)
            {
                newBytes[i] = 0;
            }
            bytes.CopyTo(newBytes, count);
            return newBytes;
        }

        static byte[] SuffixZeros(byte[] bytes, int count)
        {
            var newBytes = new byte[bytes.Length + count];
            bytes.CopyTo(newBytes, 0);
            for (int i = 0; i < count; i++)
            {
                newBytes[bytes.Length + i] = 0;
            }
            return newBytes;
        }

    }
}
