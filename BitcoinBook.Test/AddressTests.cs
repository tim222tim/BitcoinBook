using System.Text;
using Xunit;

namespace BitcoinBook.Test;

public class AddressTests
{
    [Theory]
    [InlineData("1", false)]
    [InlineData("m", true)]
    public void PayToPublicKeyHashAddressTest(string prefix, bool testnet)
    {
        var hash = Cipher.Hash256(Encoding.ASCII.GetBytes("Tim's testnet address"));
        var privateKey = new PrivateKey(hash.ToBigInteger());
        var address = new PublicKey(privateKey).ToAddress(true, testnet);
        Assert.StartsWith(prefix, address);
        Assert.Equal(privateKey.PublicKey.ToHash160().ToHex(), 
            Address.FromPayToPublicKeyHashAddress(address, testnet).ToHex());
    }

    [Theory]
    [InlineData("3", false)]
    [InlineData("2", true)]
    public void PayToScriptHashAddressTest(string prefix, bool testnet)
    {
        var scriptHash = Cipher.Hash160("Some script");
        var address = Address.ToPayToScriptHashAddress(scriptHash, testnet);
        Assert.StartsWith(prefix, address);
        Assert.Equal(scriptHash.ToHex(),
            Address.FromPayToScriptHashAddress(address, testnet).ToHex());
    }
}