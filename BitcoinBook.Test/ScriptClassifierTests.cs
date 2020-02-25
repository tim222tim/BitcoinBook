using Xunit;

namespace BitcoinBook.Test
{
    public class ScriptClassifierTests
    {
        readonly ScriptClassifier classifier = new ScriptClassifier();

        [Fact]
        public void EmptyTest()
        {
            Assert.Equal(ScriptType.Empty, classifier.GetScriptType(new Script()));
        }

        [Theory]
        [InlineData(OpCode.OP_1ADD, OpCode.OP_1SUB)]
        [InlineData(OpCode.OP_SWAP, OpCode.OP_1SUB)]
        [InlineData(new byte[] {2, 3, 4}, new byte[] { 4, 5, 6 })]
        [InlineData(OpCode.OP_DUP, OpCode.OP_HASH160, new byte[] {1, 2, 3, 4}, OpCode.OP_EQUALVERIFY, OpCode.OP_CHECKSIG)]
        public void UnknownTest(params object[] commands)
        {
            Assert.Equal(ScriptType.Unknown, classifier.GetScriptType(new Script(commands)));
        }

        [Fact]
        public void PayToPublicKeyHashTest()
        {
            Assert.Equal(ScriptType.PayToPublicKeyHash, classifier.GetScriptType(
                StandardScripts.PayToPublicKeyHash(new PrivateKey(12345).PublicKey)));
        }
    }
}
