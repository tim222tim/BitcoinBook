using System;
using System.Net.Http;

namespace BitcoinBook.Test
{
    public static class IntegrationSetup
    {
        public static IntegrationDependencies Mainnet { get; } = new IntegrationDependencies("http://mainnet.programmingbitcoin.com");
        public static IntegrationDependencies Testnet { get; } = new IntegrationDependencies("http://testnet.programmingbitcoin.com");
    }
}
