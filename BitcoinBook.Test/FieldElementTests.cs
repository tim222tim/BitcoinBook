using System;
using Xunit;

namespace BitcoinBook.Test
{
    public class FieldElementTests
    {
        [Fact]
        public void ContructorTest()
        {
            var element = new FieldElement(1, new Field(3));
            Assert.Equal(1, element.Number);
            Assert.Equal(3, element.Prime);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(4, 3)]
        [InlineData(-1, 3)]
        public void OutOfRangeTest(long number, long prime)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new FieldElement(number, new Field(prime)));
        }

        [Fact]
        public void AddInvalidTest()
        {
            Assert.Throws<InvalidOperationException>(() => new FieldElement(1, new Field(3)).Add(new FieldElement(1, new Field(5))));
        }

        [Theory]
        [InlineData(3, 0, 0, 0)]
        [InlineData(3, 1, 0, 1)]
        [InlineData(3, 2, 2, 1)]
        [InlineData(19, 11, 17, 9)]
        public void AddTest(long prime, long number1, long number2, long result)
        {
            var field = new Field(prime);
            var element = field.Element(number1).Add(field.Element(number2));
            Assert.Equal(result, element.Number);
            Assert.Equal(prime, element.Prime);
        }

        [Theory]
        [InlineData(57, 44, 33, 20)]
        [InlineData(3, 1, 0, 1)]
        [InlineData(3, 2, 2, 1)]
        [InlineData(19, 11, 17, 9)]
        public void AddOperatorTest(long prime, long number1, long number2, long result)
        {
            var field = new Field(prime);
            Assert.Equal(result, (field.Element(number1) + field.Element(number2)).Number);
        }

        [Fact]
        public void SubtractInvalidTest()
        {
            Assert.Throws<InvalidOperationException>(() => new FieldElement(1, new Field(3)).Subtract(new FieldElement(1, new Field(5))));
        }

        [Theory]
        [InlineData(3, 0, 0, 0)]
        [InlineData(3, 1, 0, 1)]
        [InlineData(3, 2, 2, 0)]
        [InlineData(19, 11, 17, 13)]
        public void SubtractTest(long prime, long number1, long number2, long result)
        {
            var field = new Field(prime);
            var element = field.Element(number1).Subtract(field.Element(number2));
            Assert.Equal(result, element.Number);
            Assert.Equal(prime, element.Prime);
        }

        [Theory]
        [InlineData(57, 9, 29, 37)]
        [InlineData(3, 1, 0, 1)]
        [InlineData(3, 2, 2, 0)]
        [InlineData(19, 11, 17, 13)]
        public void SubtractOperatorTest(long prime, long number1, long number2, long result)
        {
            var field = new Field(prime);
            Assert.Equal(result, (field.Element(number1) - field.Element(number2)).Number);
        }

        [Fact]
        public void MultiplyOperatorTest()
        {
            var f = new Field(97);
            Assert.Equal(23, (f.Element(95) * f.Element(45) * f.Element(31)).Number);
            Assert.Equal(68, (f.Element(17) * f.Element(13) * f.Element(19) * f.Element(44)).Number);
        }

        [Fact]
        public void DivideOperatorTest()
        {
            var f = new Field(31);
            Assert.Equal(4, (f.Element(3) / f.Element(24)).Number);
        }

        [Fact]
        public void PowerOperatorTest()
        {
            Assert.Equal(2, (new FieldElement(2, new Field(7)) ^ 4).Number);
            var f = new Field(31);
            Assert.Equal(29, (f.Element(17) ^ -3).Number);
            Assert.Equal(13, ((f.Element(4) ^ -4) * f.Element(11)).Number);
        }
    }
}
