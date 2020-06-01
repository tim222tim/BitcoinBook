using System.Net;

namespace BitcoinBook.Test
{
    public static class IntegrationSetup
    {
        public static NodeSetting BookNode { get; } = new NodeSetting(Dns.GetHostEntry("testnet.programmingbitcoin.com").AddressList[0], true);
        public static NodeSetting TimNode { get; } = new NodeSetting(new IPAddress(new byte[] { 104, 62, 47, 181 }), false);

        public static IntegrationDependencies Mainnet { get; } = new IntegrationDependencies("https://blockstream.info/api/");
        public static IntegrationDependencies Testnet { get; } = new IntegrationDependencies("https://blockstream.info/testnet/api/");
    }
}
