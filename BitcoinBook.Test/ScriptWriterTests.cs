using System;
using System.IO;
using Xunit;

namespace BitcoinBook.Test
{
    public class ScriptWriterTests
    {
        [Fact]
        public void WriteUnlockScriptTest()
        {
            AssertWriteSameAsRead(unlockHex);
        }

        [Fact]
        public void ReadLockScriptTest()
        {
            AssertWriteSameAsRead(lockHex);
        }

        static void AssertWriteSameAsRead(string hex)
        {
            var reader = new TransactionReader(hex);
            var script = reader.ReadScript();
            Assert.Equal(hex, GetResult(w => w.Write(script)));
        }

        static string GetResult(Action<TransactionWriter> action)
        {
            var stream = new MemoryStream();
            var writer = new TransactionWriter(stream);
            action(writer);
            var array = stream.ToArray();
            return array.ToHex();
        }

        const string unlockHex = "6a47304402204585bcdef85e6b1c6af5c2669d4830ff86e42dd205c0e089bc2a821657e951c002201024a10366077f87d6bce1f7100ad8cfa8a064b39d4e8fe4ea13a7b71aa8180f012102f0da57e85eec2934a82a585ea337ce2f4998b50ae699dd79f5880e253dafafb7";
        const string lockHex = "1976a914ab0c0b2e98b1ab6dbf67d4750b0a56244948a87988ac";

    }
}
