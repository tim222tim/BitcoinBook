using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Xunit;

namespace BitcoinBook.Test
{
    public class ScriptEvaluatorTests
    {
        readonly ScriptEvaluator evaluator = new ScriptEvaluator();
        readonly byte[] signHash = new byte[0];

        [Fact]
        public void NullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => evaluator.Evaluate(null, signHash));
        }

        [Fact]
        public void WrongObjectTypeThrows()
        {
            Assert.Throws<InvalidOperationException>(() => evaluator.Evaluate(new object[] {7}, signHash));
        }

        [Fact]
        public void NullOnTopThrows()
        {
            Assert.Throws<InvalidOperationException>(() => evaluator.Evaluate(new object[] { null }, signHash));
        }

        [Fact]
        public void EmptyCommandsIsFalse()
        {
            Assert.False(evaluator.Evaluate(new object[0], signHash));
        }

        [Fact]
        public void EmptyOnTopIsFalse()
        {
            Assert.False(evaluator.Evaluate(new object[] { new byte[0] }, signHash));
        }

        [Fact]
        public void NonEmptyOnTopIsTrue()
        {
            Assert.True(evaluator.Evaluate(new object[] { new byte[] {1} }, signHash));
        }

        [Fact]
        public void Op0IsFalse()
        {
            Assert.False(evaluator.Evaluate(new object[] { OpCode.OP_0 }, signHash));
        }

        [Fact]
        public void OpNoOpIsFalse()
        {
            var nops = new[] { OpCode.OP_NOP, OpCode.OP_NOP1, OpCode.OP_NOP4, OpCode.OP_NOP5, OpCode.OP_NOP6, OpCode.OP_NOP7, OpCode.OP_NOP8, OpCode.OP_NOP9, OpCode.OP_NOP10 };
            foreach (var opCode in nops)
            {
                Assert.False(evaluator.Evaluate(new object[] { opCode }, signHash));
            }
        }

        [Fact]
        public void Op1NegateIsTrue()
        {
            Assert.True(evaluator.Evaluate(new object[] { OpCode.OP_1NEGATE }, signHash));
        }

        [Fact]
        public void Op1IsTrue()
        {
            for (var op = OpCode.OP_1; op < OpCode.OP_16; op++)
            {
                Assert.True(evaluator.Evaluate(new object[] { op }, signHash));
            }
        }

        [Fact(Skip = "Not ready")]
        public void CheckSigTest()
        {
            var key = new PublicKey(
                "0887387e452b8eacc4acfde10d9aaf7f6d9a0f975aabb10d006e4da568744d06c",
                "061de6d95231cd89026e286df3b6ae4a894a3378e393e93a0f45b666329a0ae34");

            var signature = new Signature(
                "0ac8d1c87e51d0d441be8b3dd5b05c8795b48875dffe00b7ffcfac23010d3a395",
                "068342ceff8935ededd102dd876ffd6ba72d6a427a3edb13d26eb0781cb423c4");
            var der = Cipher.ToBytes(signature.ToDerString());
            var commands = new List<object> {der, key.ToSec(), OpCode.OP_CHECKSIG};
            var sigHash = Cipher.ToBytes("0ec208baa0fc1c19f708a9ca96fdeff3ac3f230bb4a7ba4aede4942ad003c0f600");
            Assert.True(new ScriptEvaluator().Evaluate(commands, signHash));
        }
    }
}
