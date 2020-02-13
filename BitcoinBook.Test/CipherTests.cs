using System;
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
            Assert.Equal(expected, Cipher.ToBase58(Cipher.ToBytes(input)));
        }

        [Theory]
        [InlineData("7c076ff316692a3d7eb3c3bb0f8b1488cf72e1afcd929e29307032997a838a3d", "9MA8fRQrT4u8Zj8ZRd6MAiiyaxb2Y1CMpvVkHQu5hVM6")]
        [InlineData("eff69ef2b1bd93a66ed5219add4fb51e11a840f404876325a1e8ffe0529a2c", "4fE3H2E6XMp4SsxtwinF7w9a34ooUrwWe4WsW1458Pd")]
        [InlineData("c7207fee197d27c618aea621406f6bf5ef6fca38681d82b2f06fddbdce6feab6", "EQJsjkd6JaGwxrjEhfeqPenqHwrBmPQZjJGNSCHBkcF7")]
        public void FromBase58Test(string expected, string input)
        {
            Assert.Equal(Cipher.ToBytes(expected), Cipher.FromBase58(input));
        }

        [Theory]
        [InlineData("wdA2ffYs5cudrdkhFm5Ym94AuLvavacapuDBL2CAcvqYPkcvi", "7c076ff316692a3d7eb3c3bb0f8b1488cf72e1afcd929e29307032997a838a3d")]
        [InlineData("Qwj1mwXNifQmo5VV2s587usAy4QRUviQsBxoe4EJXyWz4GBs", "eff69ef2b1bd93a66ed5219add4fb51e11a840f404876325a1e8ffe0529a2c")]
        [InlineData("2WhRyzK3iKFveq4hvQ3VR9uau26t6qZCMhADPAVMeMR6VraBbX", "c7207fee197d27c618aea621406f6bf5ef6fca38681d82b2f06fddbdce6feab6")]
        public void ToBase58CheckTest(string expected, string input)
        {
            Assert.Equal(expected, Cipher.ToBase58Check(Cipher.ToBytes(input)));
        }

        [Theory]
        [InlineData("7c076ff316692a3d7eb3c3bb0f8b1488cf72e1afcd929e29307032997a838a3d", "wdA2ffYs5cudrdkhFm5Ym94AuLvavacapuDBL2CAcvqYPkcvi")]
        [InlineData("eff69ef2b1bd93a66ed5219add4fb51e11a840f404876325a1e8ffe0529a2c", "Qwj1mwXNifQmo5VV2s587usAy4QRUviQsBxoe4EJXyWz4GBs")]
        [InlineData("c7207fee197d27c618aea621406f6bf5ef6fca38681d82b2f06fddbdce6feab6", "2WhRyzK3iKFveq4hvQ3VR9uau26t6qZCMhADPAVMeMR6VraBbX")]
        public void FromBase58CheckTest(string expected, string input)
        {
            Assert.Equal(expected, Cipher.FromBase58Check(input).ToHex());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("9MA8fRQrT4u8Zj8ZRd6MAiiyaxb2Y1CMpvVkHQu5hVM60")]
        [InlineData("kjsdl")]
        public void FromBase58FormatExceptionTest(string input)
        {
            Assert.Throws<FormatException>(() => Cipher.FromBase58(input));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("9MA8fRQrT4u8Zj8ZRd6MAiiyaxb2Y1CMpvVkHQu5hVM60")]
        [InlineData("kjsdl")]
        [InlineData("X")]
        [InlineData("EQJsjkd6JaGwxrjEhfeqPenqHwrBmPQZjJGNSCHBkcF7")]
        public void FromBase58CheckFormatExceptionTest(string input)
        {
            Assert.Throws<FormatException>(() => Cipher.FromBase58Check(input));
        }
    }
}
