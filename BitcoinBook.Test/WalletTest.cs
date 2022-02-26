using Xunit;

namespace BitcoinBook.Test;

public class WalletTest
{
    readonly PrivateKey privateKey = new(12345);
    readonly Wallet wallet;

    public WalletTest()
    {
        wallet = new Wallet(new[] {new PrivateKey(56789), privateKey, new PrivateKey(8237327312),});
    }

    [Fact]
    public void FindByPublicKeyNullTest()
    {
        Assert.Null(wallet.FindBy(new PrivateKey(7237327).PublicKey));
    }

    [Fact]
    public void FindByPublicKeyTest()
    {
        Assert.Equal(privateKey, wallet.FindBy(privateKey.PublicKey));
    }

    [Fact]
    public void FindByHashNullTest()
    {
        Assert.Null(wallet.FindByHash(new PrivateKey(7237327).PublicKey.ToHash160()));
    }

    [Fact]
    public void FindByHashTest()
    {
        Assert.Equal(privateKey, wallet.FindByHash(privateKey.PublicKey.ToHash160()));
    }
}