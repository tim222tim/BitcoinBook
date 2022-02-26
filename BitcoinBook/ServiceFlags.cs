using System;

namespace BitcoinBook;

[Flags]
public enum ServiceFlags
{
    Network = 1,
    GetUtxo = 2,
    Bloom = 4,
    Witness = 8,
    NetworkLimited = 1024,
}