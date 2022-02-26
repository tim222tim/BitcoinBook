using System;
using System.Collections.Generic;
using System.IO;

namespace BitcoinBook;

public class TransactionReader : ByteReader
{
    public TransactionReader(BinaryReader reader) : base(reader)
    {
    }

    public TransactionReader(Stream stream) : base(stream)
    {
    }

    public TransactionReader(string hex) : base(hex)
    {
    }

    public Transaction ReadTransaction()
    {
        var version = ReadInt(4);
        var count = ReadVarInt();
        var segwit = count == 0;
        if (segwit)
        {
            if (ReadByte() != 1)
            {
                throw new FormatException("Invalid segwit marker");
            }

            count = ReadVarInt();
        }

        var inputs = ReadInputs(count);
        var outputs = ReadOutputs(ReadVarInt());

        if (segwit)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                inputs[i] = inputs[i].CloneWithWitness(ReadWitness());
            }
        }
        var lockTime = ReadUnsignedInt(4);
        return new Transaction(version, segwit, inputs, outputs, lockTime);
    }

    IList<TransactionInput> ReadInputs(int count)
    {
        var inputs = new List<TransactionInput>();
        while (count-- > 0)
        {
            inputs.Add(ReadInput());
        }

        return inputs;
    }

    TransactionInput ReadInput()
    {
        return new(ReadBytesReverse(32), ReadInt(4), ReadScript(), new Script(), ReadUnsignedInt(4));
    }

    IList<TransactionOutput> ReadOutputs(int count)
    {
        var outputs = new List<TransactionOutput>();
        while (count-- > 0)
        {
            outputs.Add(ReadOutput());
        }

        return outputs;
    }

    TransactionOutput ReadOutput()
    {
        return new(ReadLong(8), ReadScript());
    }

    public Script ReadScript()
    {
        return ReadScript(ReadVarInt());
    }

    public Script ReadScript(int length)
    {
        var commands = new List<object>();
        var count = 0;
        while (count < length)
        {
            var b = ReadByte();
            ++count;
            if (b > 0 && b < (int) OpCode.OP_PUSHDATA1)
            {
                commands.Add(ReadBytes(b));
                count += b;
            }
            else if (b == (int) OpCode.OP_PUSHDATA1)
            {
                var size = ReadInt(1);
                commands.Add(ReadBytes(size));
                count += size + 1;
            }
            else if (b == (int) OpCode.OP_PUSHDATA2)
            {
                var size = ReadInt(2);
                commands.Add(ReadBytes(size));
                count += size + 2;
            }
            else
            {
                commands.Add((OpCode) b);
            }
        }

        if (count != length)
        {
            throw new FormatException("Script parsing ended at wrong length");
        }

        return new Script(commands);
    }

    public Script ReadWitness()
    {
        return ReadWitness(ReadVarInt());
    }

    public Script ReadWitness(int count)
    {
        var commands = new List<object>();
        while (count-- > 0)
        {
            var length = ReadVarInt();
            commands.Add(length == 0 ? 0 : ReadBytes(length));
        }

        return new Script(commands);
    }
}