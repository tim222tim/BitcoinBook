using System;
using Xunit;

namespace BitcoinBook.Test
{
    public class SignatureTest
    {
        const string derString = "3045022037206a0610995c58074999cb9767b87af4c4978db68c06e8e6e81d282047a7c60221008ca63759c1157ebeaec0d03cecca119fc9a75bf8e6d0fa65c841c8e2738cdaec";
        const string r = "37206a0610995c58074999cb9767b87af4c4978db68c06e8e6e81d282047a7c6";
        const string s = "08ca63759c1157ebeaec0d03cecca119fc9a75bf8e6d0fa65c841c8e2738cdaec";

        [Fact]
        public void DerTest()
        {
            Assert.Equal(derString, new Signature(r, s).ToDer().ToHex());
        }

        [Fact]
        public void DerFormatTest()
        {
            Assert.Equal(derString, new Signature(r, s).ToDerString());
        }

        [Theory]
        [InlineData("3000")]
        [InlineData("4245022037206a0610995c58074999cb9767b87af4c4978db68c06e8e6e81d282047a7c60221008ca63759c1157ebeaec0d03cecca119fc9a75bf8e6d0fa65c841c8e2738cdaec")]
        [InlineData("30FF022037206a0610995c58074999cb9767b87af4c4978db68c06e8e6e81d282047a7c60221008ca63759c1157ebeaec0d03cecca119fc9a75bf8e6d0fa65c841c8e2738cdaec")]
        [InlineData("3045FF2037206a0610995c58074999cb9767b87af4c4978db68c06e8e6e81d282047a7c60221008ca63759c1157ebeaec0d03cecca119fc9a75bf8e6d0fa65c841c8e2738cdaec")]
        [InlineData("304502FF37206a0610995c58074999cb9767b87af4c4978db68c06e8e6e81d282047a7c60221008ca63759c1157ebeaec0d03cecca119fc9a75bf8e6d0fa65c841c8e2738cdaec")]
        public void ThrowsOnBadFormat(string input)
        {
            Assert.Throws<FormatException>(() => Signature.FromDer(Cipher.ToBytes(input)));
        }

        [Fact]
        public void FromDerTest()
        {
            Assert.Equal(new Signature(r, s), Signature.FromDer(Cipher.ToBytes(derString)));
        }

        [Fact]
        public void FromDerStringTest()
        {
            Assert.Equal(new Signature(r, s), Signature.FromDer(derString));
        }
    }
}
