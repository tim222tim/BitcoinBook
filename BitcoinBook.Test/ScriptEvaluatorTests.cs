using System;
using System.Collections.Generic;
using Xunit;

namespace BitcoinBook.Test
{
    public class ScriptEvaluatorTests
    {
        readonly ScriptEvaluator evaluator = new ScriptEvaluator();
        readonly byte[] emptyHash = new byte[0];

        readonly PublicKey publicKey = new PublicKey(
            "0887387e452b8eacc4acfde10d9aaf7f6d9a0f975aabb10d006e4da568744d06c",
            "061de6d95231cd89026e286df3b6ae4a894a3378e393e93a0f45b666329a0ae34");

        readonly Signature signature = new Signature(
            "0ac8d1c87e51d0d441be8b3dd5b05c8795b48875dffe00b7ffcfac23010d3a395",
            "068342ceff8935ededd102dd876ffd6ba72d6a427a3edb13d26eb0781cb423c4");

        [Fact]
        public void NullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => evaluator.Evaluate(null, emptyHash));
        }

        [Fact]
        public void WrongObjectTypeThrows()
        {
            Assert.Throws<InvalidOperationException>(() => evaluator.Evaluate(new object[] {7}, emptyHash));
        }

        [Fact]
        public void NullOnTopThrows()
        {
            Assert.Throws<InvalidOperationException>(() => evaluator.Evaluate(new object[] { null }, emptyHash));
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
        [InlineData(true, "ec208baa0fc1c19f708a9ca96fdeff3ac3f230bb4a7ba4aede4942ad003c0f60")]
        [InlineData(false, "ff208baa0fc1c19f708a9ca96fdeff3ac3f230bb4a7ba4aede4942ad003c0f60")]
        public void P2PK_Test(bool expected, string hashString)
        {
            var commands = new object[]
            {
                signature.ToDer(), 
                publicKey.ToSec(), 
                OpCode.OP_CHECKSIG
            };
            Assert.Equal(expected, new ScriptEvaluator().Evaluate(commands, Cipher.ToBytes(hashString)));
        }

        [Theory]
        [InlineData(true, "ec208baa0fc1c19f708a9ca96fdeff3ac3f230bb4a7ba4aede4942ad003c0f60", "c0acbcf383132d76998d67ac0d2d81d85c07b0a2")]
        [InlineData(false, "ff208baa0fc1c19f708a9ca96fdeff3ac3f230bb4a7ba4aede4942ad003c0f60", "c0acbcf383132d76998d67ac0d2d81d85c07b0a2")]
        [InlineData(false, "ec208baa0fc1c19f708a9ca96fdeff3ac3f230bb4a7ba4aede4942ad003c0f60", "ffacbcf383132d76998d67ac0d2d81d85c07b0a2")]
        public void P2PKH_Test(bool expected, string sigHash, string keyHash)
        {
            var commands = new object[] 
            { 
                signature.ToDer(), 
                publicKey.ToSec(), 
                OpCode.OP_DUP, 
                OpCode.OP_HASH160, 
                Cipher.ToBytes(keyHash), 
                OpCode.OP_EQUALVERIFY, 
                OpCode.OP_CHECKSIG
            };
            Assert.Equal(expected, new ScriptEvaluator().Evaluate(commands, Cipher.ToBytes(sigHash)));
        }
    }
}
