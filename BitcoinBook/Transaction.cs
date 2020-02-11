﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BitcoinBook
{
    public class Transaction : ICloneable
    {
        public string Id { get; }
        public int Version { get; }
        public IList<TransactionInput> Inputs { get; }
        public IList<TransactionOutput> Outputs { get; }
        public uint LockTime { get; }
        public bool Testnet { get; }

        public Transaction(int version, IEnumerable<TransactionInput> inputs, IEnumerable<TransactionOutput> outputs, uint lockTime, bool testnet = false)
        {
            Version = version;
            Inputs = new List<TransactionInput>(inputs ?? throw new ArgumentNullException(nameof(inputs)));
            Outputs = new List<TransactionOutput>(outputs ?? throw new ArgumentNullException(nameof(outputs)));
            LockTime = lockTime;
            Testnet = testnet;
            Id = ComputeId();
        }

        string ComputeId()
        {
            var stream = new MemoryStream();
            var writer = new TransactionWriter(stream);
            writer.Write(this);
            var hash = Cipher.Hash256(stream.ToArray());
            Array.Reverse(hash);
            return Cipher.ToHex(hash);
        }

        public Transaction Clone()
        {
            return new Transaction(Version, Inputs.Select(i => i.Clone()), 
                Outputs.Select(o => o.Clone()), LockTime, Testnet);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public Transaction CloneWithoutSigScripts()
        {
            return new Transaction(Version, Inputs.Select(i => i.CloneWithoutSigScript()), 
                Outputs.Select(o => o.Clone()), LockTime, Testnet);
        }

        public Transaction CloneWithReplacedSigScript(int inputIndex, Script script)
        {
            if (inputIndex < 0 || inputIndex >= Inputs.Count)
            {
                throw new IndexOutOfRangeException($"Input index {inputIndex} is invalid for transaction with {Inputs.Count} inputs");
            }

            var inputs = Inputs.Select(i => i.CloneWithoutSigScript()).ToArray();
            inputs[inputIndex] = Inputs[inputIndex].CloneWithSigScript(script);
            return new Transaction(Version, inputs,
                Outputs.Select(o => o.Clone()), LockTime, Testnet);
        }
    }
}
