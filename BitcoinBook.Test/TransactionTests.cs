using System.Linq;
using Xunit;

namespace BitcoinBook.Test;

public class TransactionTests
{
    readonly Transaction transaction = new TransactionReader(rawTransaction).ReadTransaction();

    [Fact]
    public void CloneIdShouldBeEqual()
    {
        var transactionClone = transaction.Clone();
        Assert.NotSame(transaction, transactionClone);
        Assert.Equal(transaction.Id, transactionClone.Id);
    }

    [Fact]
    public void CloneWithoutSigScriptsTest()
    {
        var transaction2 = transaction.CloneWithoutSigScripts();

        Assert.Equal(transaction.Inputs.Count, transaction2.Inputs.Count);
        Assert.True(transaction2.Inputs.All(o => o.SigScript.Commands.Count == 0));
        Assert.Equal(transaction.Outputs.Count, transaction2.Outputs.Count);
        Assert.True(transaction2.Outputs.All(o => o.ScriptPubKey.Commands.Count > 0));
    }

    const string rawTransaction =
        "0100000001be2ae9a2a3d91cdbbc53aca340a0837416519b129f627239a7dcf7fd069ae074000000006a473044022035a874a246f4de3570295fa8e32ca48a3eb1cf3a4bea6cbea6d18f122f2da51a02204ee9fe995e4934445d381be89b5635bf16bcf9bd023d81e5dc54991d7124921101210279fc02b440c755d18e80add59b5f1ec9452ab8348e75ced61e47c0750408e028feffffff042ec00900000000001976a914664403549f16e3ded11e2a5049e7837269d129ed88acf8e20100000000001976a9149a0093bfcbd8cb1162fd3c3355c6a8fddba2df6488ac5512fb12000000001976a9141a225100a114159cd16dc22650bf39043ba5eaca88ac8f990f00000000001976a914888a6f53d62ea69aabd94f59e8356fc73ad1f37f88ac2dc70600";
}