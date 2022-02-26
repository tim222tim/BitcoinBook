﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BitcoinBook;

public class TransactionHasher
{
    readonly ITransactionFetcher fetcher;

    public TransactionHasher(ITransactionFetcher fetcher)
    {
        this.fetcher = fetcher;
    }

    public async Task<byte[]> ComputeSigHash(Transaction transaction, int inputIndex, SigHashType sigHashType)
    {
        CheckInputIndex(transaction, inputIndex);
        return await ComputeSigHash(transaction, transaction.Inputs[inputIndex], sigHashType);
    }

    public async Task<byte[]> ComputeSigHash(Transaction transaction, TransactionInput input, SigHashType sigHashType)
    {
        var priorOutput = await fetcher.FetchPriorOutput(input);
        return ComputeSigHash(transaction, input, priorOutput, sigHashType);
    }

    public byte[] ComputeSigHash(Transaction transaction, TransactionInput input, TransactionOutput priorOutput, SigHashType sigHashType)
    {
        if (sigHashType != SigHashType.All)
        {
            throw new NotImplementedException("Only supports SigHashType.All");
        }

        var hashingScript = IsPayToScriptHash(input.SigScript, priorOutput.ScriptPubKey) ?
            DecodeRedeemScript((byte[]) input.SigScript.Commands[^1]) : 
            priorOutput.ScriptPubKey;
        transaction = transaction.CloneWithReplacedSigScript(input, hashingScript);
        var stream = new MemoryStream();
        var writer = new TransactionWriter(stream);
        writer.Write(transaction);
        writer.Write((int)sigHashType, 4);

        return Cipher.Hash256(stream.ToArray());
    }

    bool IsPayToScriptHash(Script sigScript, Script scriptPubKey)
    {
        return sigScript.Commands.Count > 0 &&
               sigScript.Commands[^1] is byte[] redeemBytes &&
               scriptPubKey.Commands.Count == 3 &&
               scriptPubKey.Commands[0] is OpCode hashCode && hashCode == OpCode.OP_HASH160 &&
               scriptPubKey.Commands[1] is byte[] scriptHash && scriptHash.Length == 20 &&
               scriptPubKey.Commands[2] is OpCode equalCode && equalCode == OpCode.OP_EQUAL &&
               Cipher.Hash160(redeemBytes).SequenceEqual(scriptHash);
    }

    Script DecodeRedeemScript(byte[] scriptBytes)
    {
        var reader = new TransactionReader(new MemoryStream(scriptBytes));
        return reader.ReadScript(scriptBytes.Length);
    }

    void CheckInputIndex(Transaction transaction, int inputIndex)
    {
        if (inputIndex < 0 || inputIndex >= transaction.Inputs.Count)
        {
            throw new IndexOutOfRangeException(
                $"Input index {inputIndex} is invalid for transaction with {transaction.Inputs.Count} inputs");
        }
    }
}