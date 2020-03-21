namespace BitcoinBook.Test
{
    public static class IntegrationSetup
    {
        public static IntegrationDependencies Mainnet { get; } = new IntegrationDependencies("https://blockstream.info/api/");
        public static IntegrationDependencies Testnet { get; } = new IntegrationDependencies("https://blockstream.info/testnet/api/");
    }
}
