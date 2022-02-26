using System;

namespace BitcoinBook;

public static class Address
{
    public static string ToAddress(byte prefix, byte[] hash)
    {
        return Cipher.ToBase58Check(new[] { prefix }.Concat(hash));
    }

    public static string ToPayToPublicKeyHashAddress(byte[] hash, bool testnet = false)
    {
        return ToAddress(PublicKeyHashPrefix(testnet), hash);
    }

    public static string ToPayToScriptHashAddress(byte[] hash, bool testnet = false)
    {
        return ToAddress(ScriptHashPrefix(testnet), hash);
    }

    public static byte[] HashFromAddress(string address)
    {
        return Cipher.FromBase58Check(address).Copy(1);
    }

    public static byte[] HashFromAddress(byte prefix, string address)
    {
        var addressBytes = Cipher.FromBase58Check(address);
        if (addressBytes.Length < 2 || addressBytes[0] != prefix)
        {
            throw new FormatException("Wrong prefix");
        }
        return addressBytes.Copy(1);
    }

    public static byte[] FromPayToPublicKeyHashAddress(string address, bool testnet = false)
    {
        return HashFromAddress(PublicKeyHashPrefix(testnet), address);
    }

    public static byte[] FromPayToScriptHashAddress(string address, bool testnet = false)
    {
        return HashFromAddress(ScriptHashPrefix(testnet), address);
    }

    static byte PublicKeyHashPrefix(bool testnet)
    {
        return testnet ? (byte)'\x6f' : (byte)'\x00';
    }

    static byte ScriptHashPrefix(bool testnet)
    {
        return testnet ? (byte)'\xc4' : (byte)'\x05';
    }
}