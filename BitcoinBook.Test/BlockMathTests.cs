using Xunit;

namespace BitcoinBook.Test
{
    public class BlockMathTests
    {
        [Fact]
        public void ComputeNewTargetTest()
        {
            var block473759 = BlockHeader.Parse(Cipher.ToBytes(
                "02000020f1472d9db4b563c35f97c428ac903f23b7fc055d1cfc26000000000000000000b3f449fcbe1bc4cfbcb8283a0d2c037f961a3fdf2b8bedc144973735eea707e1264258597e8b0118e5f00474"));
            Assert.Equal("000000000000000001389446206ebcd378c32cd00b4920a8a1ba7b540ca7d699",
                block473759.Id);
            var block471744 = BlockHeader.Parse(Cipher.ToBytes(
                "000000203471101bbda3fe307664b3283a9ef0e97d9a38a7eacd8800000000000000000010c8aba8479bbaa5e0848152fd3c2289ca50e1c3e58c9a4faaafbdf5803c5448ddb845597e8b0118e43a81d3"));
            Assert.Equal("0000000000000000012a85f9010f0e2cf696408300918f4b5df8ddd8809102a2",
                block471744.Id);
            Assert.Equal("x", BlockMath.ComputeNewTarget(block471744, block473759).ToHex32());
        }

        [Fact]
        public void TargetToBitsTest()
        {
            var header = new BlockHeader(
                0x20000000,
                "0000000000000000007962066dcd6675830883516bcf40047d42740a85eb2919",
                "31951c69428a95a46b517ffb0de12fec1bd0b2392aec07b64573e03ded31621f",
                1513622125,
                0x18013ce9,
                0x5cfc9955);
            Assert.Equal(header.Bits, BlockMath.TargetToBits(header.Target));
        }
    }
}
