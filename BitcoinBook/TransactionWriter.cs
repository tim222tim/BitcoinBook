using System;
using System.Collections.Generic;
using System.IO;

namespace BitcoinBook;

public class TransactionWriter : ByteWriter
{
    public TransactionWriter(BinaryWriter writer) : base(writer)
    {
    }

    public TransactionWriter(Stream stream) : base(stream)
    {
    }

    public void Write(Transaction transaction)
    {
        Write(transaction.Version, 4);
        if (transaction.Segwit)
        {
            Write((byte) 0);
            Write(1);
        }
        Write(transaction.Inputs);
        Write(transaction.Outputs);
        if (transaction.Segwit)
        {
            WriteWitness(transaction.Inputs);
        }
        Write(transaction.LockTime, 4);
    }

    void Write(ICollection<TransactionInput> inputs)
    {
        WriteVar((ulong) inputs.Count);
        foreach (var input in inputs)
        {
            Write(input);
        }
    }

    void WriteWitness(IEnumerable<TransactionInput> inputs)
    {
        foreach (var input in inputs)
        {
            var commands = input.Witness.Commands;
            WriteVar(commands.Count);
            foreach (var command in commands)
            {
                if (command is int length)
                {
                    if (length != 0)
                    {
                        throw new FormatException("Invalid value, should be 0");
                    }
                    Write((byte)0);
                }
                else
                {
                    if (!(command is byte[] bytes))
                    {
                        throw new FormatException("Invalid command type, should be byte[]");
                    }
                    WriteVarBytes(bytes);
                }
            }
        }
    }

    void Write(TransactionInput input)
    {
        WriteReverse(input.PreviousTransaction);
        Write(input.PreviousIndex, 4);
        Write(input.SigScript);
        Write(input.Sequence, 4);
    }

    void Write(ICollection<TransactionOutput> outputs)
    {
        WriteVar(outputs.Count);
        foreach (var output in outputs)
        {
            Write(output);
        }
    }

    void Write(TransactionOutput output)
    {
        Write(output.Amount, 8);
        Write(output.ScriptPubKey);
    }

    public void Write(Script script)
    {
        var stream = new MemoryStream();
        var writer = new TransactionWriter(stream);
        writer.Write(script.Commands);
        var bytes = stream.ToArray();
        WriteVarBytes(bytes);
    }

    void Write(IEnumerable<object> commands)
    {
        foreach (var command in commands)
        {
            if (command is OpCode opCode)
            {
                Write(opCode);
            }
            else
            {
                if (!(command is byte[] bytes))
                {
                    throw new FormatException("Wrong type in script commands");
                }

                var length = bytes.Length;
                if (length < (int)OpCode.OP_PUSHDATA1)
                {
                    Write(length, 1);
                }
                else if (length < 0x100)
                {
                    Write(OpCode.OP_PUSHDATA1);
                    Write(length, 1);
                }
                else if (length < 520)
                {
                    Write(OpCode.OP_PUSHDATA2);
                    Write(length, 2);
                }
                else
                {
                    throw new FormatException("Script value is too long");
                }
                Write(bytes);
            }
        }
    }

}