using System;
using Xunit;

namespace BitcoinBook.Test
{
    public class ScriptEvaluatorTests
    {
        const string goodHash = "ec208baa0fc1c19f708a9ca96fdeff3ac3f230bb4a7ba4aede4942ad003c0f60";
        const string badHash = "ff208baa0fc1c19f708a9ca96fdeff3ac3f230bb4a7ba4aede4942ad003c0f60";
        readonly ScriptEvaluator evaluator = new ScriptEvaluator();
        readonly byte[] emptyHash = new byte[0];

        readonly PublicKey publicKey = new PublicKey(
            "0887387e452b8eacc4acfde10d9aaf7f6d9a0f975aabb10d006e4da568744d06c",
            "061de6d95231cd89026e286df3b6ae4a894a3378e393e93a0f45b666329a0ae34");

        readonly Signature signature = new Signature(
            "0ac8d1c87e51d0d441be8b3dd5b05c8795b48875dffe00b7ffcfac23010d3a395",
            "068342ceff8935ededd102dd876ffd6ba72d6a427a3edb13d26eb0781cb423c4");

        readonly PrivateKey privateKey2 = new PrivateKey(8989349843893);
        readonly Signature signature2;

        readonly PrivateKey privateKey3 = new PrivateKey(28974387478934);

        public ScriptEvaluatorTests()
        {
            signature2 = privateKey2.Sign(Cipher.ToBytes(goodHash));
        }

        [Fact]
        public void NullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => evaluator.Evaluate(null!, emptyHash));
        }

        [Fact]
        public void WrongObjectTypeThrows()
        {
            Assert.Throws<InvalidOperationException>(() => evaluator.Evaluate(new object[] {7}, emptyHash));
        }

        [Fact]
        public void NullOnTopThrows()
        {
            Assert.Throws<InvalidOperationException>(() => evaluator.Evaluate(new object[] { null! }, emptyHash));
        }

        [Fact]
        public void EmptyCommandsIsFalse()
        {
            Assert.False(evaluator.Evaluate(new object[0], emptyHash));
        }

        [Fact]
        public void EmptyOnTopIsFalse()
        {
            Assert.False(evaluator.Evaluate(new object[] { new byte[0] }, emptyHash));
        }

        [Fact]
        public void NonEmptyOnTopIsTrue()
        {
            Assert.True(evaluator.Evaluate(new object[] { new byte[] {1} }, emptyHash));
        }

        [Fact]
        public void Op0IsFalse()
        {
            Assert.False(evaluator.Evaluate(new object[] { OpCode.OP_0 }, emptyHash));
        }

        [Fact]
        public void OpNoOpIsFalse()
        {
            var nops = new[] { OpCode.OP_NOP, OpCode.OP_NOP1, OpCode.OP_NOP4, OpCode.OP_NOP5, OpCode.OP_NOP6, OpCode.OP_NOP7, OpCode.OP_NOP8, OpCode.OP_NOP9, OpCode.OP_NOP10 };
            foreach (var opCode in nops)
            {
                Assert.False(evaluator.Evaluate(new object[] { opCode }, emptyHash));
            }
        }

        [Fact]
        public void Op1NegateIsTrue()
        {
            Assert.True(evaluator.Evaluate(new object[] { OpCode.OP_1NEGATE }, emptyHash));
        }

        [Fact]
        public void Op1ToOp16AreTrue()
        {
            for (var op = OpCode.OP_1; op < OpCode.OP_16; op++)
            {
                Assert.True(evaluator.Evaluate(new object[] { op }, emptyHash));
            }
        }

        [Theory]
        [InlineData(true, goodHash)]
        [InlineData(false, badHash)]
        public void P2PK_Test(bool expected, string hashString)
        {
            var commands = new object[]
            {
                signature.ToDer().Concat(1), 
                publicKey.ToSec(), 
                OpCode.OP_CHECKSIG
            };
            Assert.Equal(expected, evaluator.Evaluate(commands, Cipher.ToBytes(hashString)));
        }

        [Theory]
        [InlineData(true, goodHash, "c0acbcf383132d76998d67ac0d2d81d85c07b0a2")]
        [InlineData(false, badHash, "c0acbcf383132d76998d67ac0d2d81d85c07b0a2")]
        [InlineData(false, goodHash, "ffacbcf383132d76998d67ac0d2d81d85c07b0a2")]
        public void P2PKH_Test(bool expected, string hashString, string keyHash)
        {
            var commands = new object[] 
            { 
                signature.ToDer().Concat(1), 
                publicKey.ToSec(), 
                OpCode.OP_DUP, 
                OpCode.OP_HASH160, 
                Cipher.ToBytes(keyHash), 
                OpCode.OP_EQUALVERIFY, 
                OpCode.OP_CHECKSIG
            };
            Assert.Equal(expected, evaluator.Evaluate(commands, Cipher.ToBytes(hashString)));
        }

        [Fact]
        public void OpAddEqualTest()
        {
            // 4 5 add 9 equal
            var script = new TransactionReader("055455935987").ReadScript();
            Assert.True(evaluator.Evaluate(script.Commands));
        }

        [Fact]
        public void OpMulTest()
        {
            // 2 dup dup mul add 6 equal
            var script = new TransactionReader("0752767695935687").ReadScript();
            Assert.True(evaluator.Evaluate(script.Commands));
        }

        [Theory]
        [InlineData(false, "010203", "010203")]
        [InlineData(false, "010203", "010204")]
        [InlineData(true, "255044462d312e330a25e2e3cfd30a0a0a312030206f626a0a3c3c2f57696474682032203020522f4865696768742033203020522f547970652034203020522f537562747970652035203020522f46696c7465722036203020522f436f6c6f7253706163652037203020522f4c656e6774682038203020522f42697473506572436f6d706f6e656e7420383e3e0a73747265616d0affd8fffe00245348412d3120697320646561642121212121852fec092339759c39b1a1c63c4c97e1fffe017f46dc93a6b67e013b029aaa1db2560b45ca67d688c7f84b8c4c791fe02b3df614f86db1690901c56b45c1530afedfb76038e972722fe7ad728f0e4904e046c230570fe9d41398abe12ef5bc942be33542a4802d98b5d70f2a332ec37fac3514e74ddc0f2cc1a874cd0c78305a21566461309789606bd0bf3f98cda8044629a1",
                          "255044462d312e330a25e2e3cfd30a0a0a312030206f626a0a3c3c2f57696474682032203020522f4865696768742033203020522f547970652034203020522f537562747970652035203020522f46696c7465722036203020522f436f6c6f7253706163652037203020522f4c656e6774682038203020522f42697473506572436f6d706f6e656e7420383e3e0a73747265616d0affd8fffe00245348412d3120697320646561642121212121852fec092339759c39b1a1c63c4c97e1fffe017346dc9166b67e118f029ab621b2560ff9ca67cca8c7f85ba84c79030c2b3de218f86db3a90901d5df45c14f26fedfb3dc38e96ac22fe7bd728f0e45bce046d23c570feb141398bb552ef5a0a82be331fea48037b8b5d71f0e332edf93ac3500eb4ddc0decc1a864790c782c76215660dd309791d06bd0af3f98cda4bc4629b1")]
        public void Sha1PinataTest(bool expected, string val1, string val2)
        {
            var commands = new object[]
            {
                Cipher.ToBytes(val1),
                Cipher.ToBytes(val2),
                OpCode.OP_2DUP,
                OpCode.OP_EQUAL,
                OpCode.OP_NOT,
                OpCode.OP_VERIFY,
                OpCode.OP_SHA1,
                OpCode.OP_SWAP,
                OpCode.OP_SHA1,
                OpCode.OP_EQUAL,
            };
            Assert.Equal(expected, evaluator.Evaluate(commands));
        }

        [Fact]
        public void BareMultisigOneOfTwoTest()
        {
            var commands = new object[]
            {
                OpCode.OP_0,
                signature.ToDer().Concat(1),
                OpCode.OP_1,
                privateKey2.PublicKey.ToSec(),
                publicKey.ToSec(),
                OpCode.OP_2,
                OpCode.OP_CHECKMULTISIG
            };
            Assert.True(evaluator.Evaluate(commands, Cipher.ToBytes(goodHash)));
        }

        [Fact]
        public void BareMultisigOneOfTwoReversedTest()
        {
            var commands = new object[]
            {
                OpCode.OP_0,
                signature.ToDer().Concat(1),
                OpCode.OP_1,
                publicKey.ToSec(),
                privateKey2.PublicKey.ToSec(),
                OpCode.OP_2,
                OpCode.OP_CHECKMULTISIG
            };
            Assert.True(evaluator.Evaluate(commands, Cipher.ToBytes(goodHash)));
        }

        [Fact]
        public void BareMultisigBadTest()
        {
            var commands = new object[]
            {
                OpCode.OP_0,
                signature.ToDer().Concat(1),
                OpCode.OP_1,
                publicKey.ToSec(),
                privateKey2.PublicKey.ToSec(),
                OpCode.OP_2,
                OpCode.OP_CHECKMULTISIG
            };
            Assert.False(evaluator.Evaluate(commands, Cipher.ToBytes(badHash)));
        }

        [Fact]
        public void BareShortStackTest()
        {
            var commands = new object[]
            {
                signature.ToDer().Concat(1),
                OpCode.OP_1,
                publicKey.ToSec(),
                privateKey2.PublicKey.ToSec(),
                OpCode.OP_2,
                OpCode.OP_CHECKMULTISIG
            };
            Assert.False(evaluator.Evaluate(commands, Cipher.ToBytes(goodHash)));
        }

        [Fact]
        public void BareMultisigTwoOfThreeTest()
        {
            var commands = new object[]
            {
                OpCode.OP_0,
                signature2.ToDer().Concat(1),
                signature.ToDer().Concat(1),
                OpCode.OP_2,
                privateKey3.PublicKey.ToSec(),
                privateKey2.PublicKey.ToSec(),
                publicKey.ToSec(),
                OpCode.OP_3,
                OpCode.OP_CHECKMULTISIG
            };
            Assert.True(evaluator.Evaluate(commands, Cipher.ToBytes(goodHash)));
        }

        [Fact]
        public void BareMultisigTwoOfThreeBadOrderTest()
        {
            var commands = new object[]
            {
                OpCode.OP_0,
                signature.ToDer().Concat(1),
                signature2.ToDer().Concat(1),
                OpCode.OP_2,
                privateKey3.PublicKey.ToSec(),
                privateKey2.PublicKey.ToSec(),
                publicKey.ToSec(),
                OpCode.OP_3,
                OpCode.OP_CHECKMULTISIG
            };
            Assert.False(evaluator.Evaluate(commands, Cipher.ToBytes(goodHash)));
        }

        [Fact]
        public void BareMultisigTwoOfThreeWithSameSigTwiceTest()
        {
            var commands = new object[]
            {
                OpCode.OP_0,
                signature2.ToDer().Concat(1),
                signature2.ToDer().Concat(1),
                OpCode.OP_2,
                privateKey3.PublicKey.ToSec(),
                privateKey2.PublicKey.ToSec(),
                publicKey.ToSec(),
                OpCode.OP_3,
                OpCode.OP_CHECKMULTISIG
            };
            Assert.False(evaluator.Evaluate(commands, Cipher.ToBytes(goodHash)));
        }

        [Fact]
        public void PayToScriptHashTrueTest()
        {
            var redeemBytes = new Script(OpCode.OP_ADD, OpCode.OP_7, OpCode.OP_EQUAL).ToBytes().Copy(1);

            var commands = new object[]
            {
                OpCode.OP_3,
                OpCode.OP_4,
                redeemBytes,
                OpCode.OP_HASH160,
                Cipher.Hash160(redeemBytes),
                OpCode.OP_EQUAL
            };
            Assert.True(evaluator.Evaluate(commands));
        }

        [Fact]
        public void PayToScriptHashRedeemKeyFalseTest()
        {
            var redeemBytes = new Script(OpCode.OP_ADD, OpCode.OP_7, OpCode.OP_EQUAL).ToBytes().Copy(1);

            var commands = new object[]
            {
                OpCode.OP_1,
                OpCode.OP_4,
                redeemBytes,
                OpCode.OP_HASH160,
                Cipher.Hash160(redeemBytes),
                OpCode.OP_EQUAL
            };
            Assert.False(evaluator.Evaluate(commands));
        }

        [Fact]
        public void PayToScriptHashBadHashTest()
        {
            var redeemBytes = new Script(OpCode.OP_ADD, OpCode.OP_7, OpCode.OP_EQUAL).ToBytes().Copy(1);

            var commands = new object[]
            {
                OpCode.OP_1,
                OpCode.OP_4,
                redeemBytes,
                OpCode.OP_HASH160,
                Cipher.Hash160(Cipher.ToBytes(badHash)),
                OpCode.OP_EQUAL
            };
            Assert.False(evaluator.Evaluate(commands));
        }
    }
}
