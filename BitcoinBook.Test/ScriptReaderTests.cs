using Xunit;

namespace BitcoinBook.Test;

public class ScriptReaderTests
{
    [Fact]
    public void ReadUnlockScriptTest()
    {
        var reader = new TransactionReader(unlockHex);
        var script = reader.ReadScript();
        Assert.Equal(2, script.Commands.Count);
        Assert.IsType<byte[]>(script.Commands[0]);
        Assert.Equal(71, ((byte[])script.Commands[0]).Length);
        Assert.IsType<byte[]>(script.Commands[1]);
        Assert.Equal(33, ((byte[])script.Commands[1]).Length);
    }

    [Fact]
    public void ReadLockScriptTest()
    {
        var reader = new TransactionReader(lockHex);
        var script = reader.ReadScript();
        Assert.Equal(5, script.Commands.Count);
        Assert.Equal(OpCode.OP_DUP, script.Commands[0]);
        Assert.Equal(OpCode.OP_HASH160, script.Commands[1]);
        Assert.IsType<byte[]>(script.Commands[2]);
        Assert.Equal(20, ((byte[])script.Commands[2]).Length);
        Assert.Equal(OpCode.OP_EQUALVERIFY, script.Commands[3]);
        Assert.Equal(OpCode.OP_CHECKSIG, script.Commands[4]);
    }

    const string unlockHex = "6a47304402204585bcdef85e6b1c6af5c2669d4830ff86e42dd205c0e089bc2a821657e951c002201024a10366077f87d6bce1f7100ad8cfa8a064b39d4e8fe4ea13a7b71aa8180f012102f0da57e85eec2934a82a585ea337ce2f4998b50ae699dd79f5880e253dafafb7";
    const string lockHex = "1976a914ab0c0b2e98b1ab6dbf67d4750b0a56244948a87988ac";
}