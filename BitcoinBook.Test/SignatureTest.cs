using Xunit;

namespace BitcoinBook.Test
{
    public class SignatureTest
    {
        [Fact]
        public void DerFormatTest()
        {
            Assert.Equal("3045022037206a0610995c58074999cb9767b87af4c4978db68c06e8e6e81d282047a7c60221008ca63759c1157ebeaec0d03cecca119fc9a75bf8e6d0fa65c841c8e2738cdaec",
                new Signature(
                    "37206a0610995c58074999cb9767b87af4c4978db68c06e8e6e81d282047a7c6", 
                    "08ca63759c1157ebeaec0d03cecca119fc9a75bf8e6d0fa65c841c8e2738cdaec").ToDerFormat());
        }
    }
}
