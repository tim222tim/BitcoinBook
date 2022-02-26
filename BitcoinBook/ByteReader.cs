﻿using System;
using System.IO;
using System.Net;
using System.Text;

namespace BitcoinBook;

public class ByteReader
{
    static readonly byte[] ipv4Prefix = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xff, 0xff};

    readonly BinaryReader reader;

    public ByteReader(BinaryReader reader)
    {
        this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
    }

    public ByteReader(Stream stream) : this(new BinaryReader(stream ?? throw new ArgumentNullException(nameof(stream))))
    {
    }

    public ByteReader(byte[] bytes) : this(new MemoryStream(bytes))
    {
    }

    public ByteReader(string hex) : this(Cipher.ToBytes(hex ?? throw new ArgumentNullException(nameof(hex))))
    {
    }

    public byte ReadByte()
    {
        return reader.ReadByte();
    }

    public byte[] ReadBytes(int count)
    {
        var bytes = reader.ReadBytes(count);
        if (bytes.Length != count)
        {
            throw new EndOfStreamException($"Could not read {count} bytes");
        }
        return bytes;
    }

    public uint ReadUnsignedInt(int length, bool bigEndian = false)
    {
        return (uint) ReadUnsignedLong(length, bigEndian);
    }

    public int ReadInt(int length, bool bigEndian = false)
    {
        return (int) ReadUnsignedLong(length, bigEndian);
    }

    public long ReadLong(int length, bool bigEndian = false)
    {
        return (long) ReadUnsignedLong(length, bigEndian);
    }

    public ulong ReadUnsignedLong(int length, bool bigEndian = false)
    {
        ulong i = 0L;
        if (bigEndian)
        {
            while (length-- > 0)
            {
                i *= 256;
                i += reader.ReadByte();
            }
        }
        else
        {
            ulong factor = 1L;
            while (length-- > 0)
            {
                i += reader.ReadByte() * factor;
                factor *= 256;
            }
        }

        return i;
    }

    public int ReadVarInt()
    {
        return (int) ReadVarLong();
    }

    public long ReadVarLong()
    {
        var i = ReadLong(1);
        var len = GetVarLength(i);
        return len == 1 ? i : ReadLong(len);
    }

    protected byte[] ReadBytesReverse(int count)
    {
        return reader.ReadBytes(count).Reverse();
    }

    public byte[] ReadVarBytes()
    {
        return reader.ReadBytes(ReadVarInt());
    }

    public string ReadString(int length)
    {
        var builder = new StringBuilder();
        builder.Append(reader.ReadChars(length));
        return builder.ToString().TrimEnd('\0');
    }

    public string ReadString()
    {
        return ReadString(ReadVarInt());
    }

    protected int GetVarLength(long i)
    {
        switch (i)
        {
            case 0xFD:
                return 2;
            case 0xFE:
                return 4;
            case 0xFF:
                return 8;
            default:
                return 1;
        }
    }

    public NetworkAddress ReadNetworkAddress()
    {
        return new(                    
            ReadUnsignedLong(8),
            ReadIPAddress(),
            (ushort)ReadInt(2, true)
        );
    }

    public TimestampedNetworkAddress ReadTimestampedNetworkAddress()
    {
        var timestamp = ReadUnsignedInt(4);
        return new TimestampedNetworkAddress(ReadNetworkAddress(), timestamp);
    }

    public IPAddress ReadIPAddress()
    {
        var addressBytes = reader.ReadBytes(16);
        var address = new IPAddress(StartsWith(addressBytes, ipv4Prefix) ? addressBytes.Copy(12, 4) : addressBytes);
        return address;
    }

    bool StartsWith(byte[] sequence, byte[] prefix)
    {
        for (var i = 0; i < prefix.Length; i++)
        {
            if (sequence[i] != prefix[i])
            {
                return false;
            }
        }

        return true;
    }

    public bool ReadBool()
    {
        return reader.ReadByte() != 0;
    }
}