using System.Text;
using Xunit;

namespace BitcoinBook.Test
{
    public class AddressTests
    {
        [Fact]
        public void HashFromAddressTest()
        {
            var hash = Cipher.Hash256(Encoding.ASCII.GetBytes("Tim's testnet address"));
            var privateKey = new PrivateKey(hash.ToBigInteger());
            var address = new PublicKey(privateKey).ToAddress(true, true);
            Assert.Equal(privateKey.PublicKey.ToHash160().ToHex(), 
                Address.FromPayToPublicKeyHashAddress(address, true).ToHex());
        }
    }
}
