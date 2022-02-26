using System;

namespace BitcoinBook;

public class BlockDataItem
{
    public BlockDataType BlockDataType { get; }
    public byte[] Hash { get; }

    public BlockDataItem(BlockDataType blockDataType, byte[] hash)
    {
        BlockDataType = blockDataType;
        Hash = hash ?? throw new ArgumentNullException(nameof(hash));
        if (Hash.Length != 32) throw new ArgumentException("Must be 32 bytes", nameof(hash));
    }
}