﻿using System;
using System.Globalization;
using System.Numerics;

namespace BitcoinBook;

public record Signature(BigInteger R, BigInteger S)
{
    public Signature(string r, string s, NumberStyles numberStyles = NumberStyles.HexNumber) :
        this(BigInteger.Parse(r, numberStyles), BigInteger.Parse(s, numberStyles))
    {
    }

    public static Signature FromDer(string der)
    {
        return FromDer(Cipher.ToBytes(der));
    }

    public static Signature FromDer(byte[] der)
    {
        if (der == null) throw new ArgumentNullException(nameof(der));
        if (der.Length < 8) throw new FormatException("value not long enough");
        if (der[0] != 0x30) throw new FormatException("wrong DER prefix"); 
        if (der[1] != der.Length - 2) throw new FormatException("wrong length");
        var rBytes = GetPrefixedBytes(der, 2);
        var sBytes = GetPrefixedBytes(der, rBytes.Length + 4);
        return new Signature(rBytes.ToBigInteger(), sBytes.ToBigInteger());
    }

    static byte[] GetPrefixedBytes(byte[] bytes, int index)
    {
        if (bytes[index] != 0x02) throw new FormatException("wrong number prefix");
        var length = bytes[index + 1];
        if (index + length > bytes.Length) throw new FormatException("value not long enough for number");
        return bytes.Copy(index + 2, length);
    }

    public override string ToString()
    {
        return $"Signature({R:X},{S:x})";
    }

    public string ToDerString()
    {
        return ToDer().ToHex();
    }

    public byte[] ToDer()
    {
        var rBytes = ToPrefixedBytes(R);
        var sBytes = ToPrefixedBytes(S);
        var der = new byte[rBytes.Length + sBytes.Length + 2];
        der[0] = 0x30;
        der[1] = (byte) (rBytes.Length + sBytes.Length);
        rBytes.CopyTo(der, 2);
        sBytes.CopyTo(der, rBytes.Length + 2);
        return der;
    }

    byte[] ToPrefixedBytes(BigInteger i)
    {
        return new byte[] {0x02, (byte) i.ToSignedBigBytes().Length}.Concat(i.ToSignedBigBytes());
    }
}