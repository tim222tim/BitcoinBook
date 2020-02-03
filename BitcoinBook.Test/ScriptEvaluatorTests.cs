using System;
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
    }
}
