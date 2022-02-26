using System.Net;

namespace BitcoinBook;

public class NodeSetting
{
    public IPAddress Address { get; }
    public bool Testnet { get; }

    public NodeSetting(IPAddress address, bool testnet)
    {
        Address = address;
        Testnet = testnet;
    }
}