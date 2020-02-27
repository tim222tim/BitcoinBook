using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BitcoinBook
{
    public class Transaction : ICloneable
    {
        public string Id { get; }
        public bool Segwit { get; }
        public int Version { get; }
        public IList<TransactionInput> Inputs { get; }
        public IList<TransactionOutput> Outputs { get; }
        public uint LockTime { get; }
        public bool Testnet { get; }

        public Transaction(int version, bool segwit, IEnumerable<TransactionInput> inputs, IEnumerable<TransactionOutput> outputs, uint lockTime, bool testnet = false)
        {
            Version = version;
            Segwit = segwit;
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
            return hash.ToHex();
        }

        public Transaction Clone()
        {
            return new Transaction(Version, Segwit, Inputs.Select(i => i.Clone()), 
                Outputs.Select(o => o.Clone()), LockTime, Testnet);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public Transaction CloneWithoutSigScripts()
        {
            return new Transaction(Version, Segwit, Inputs.Select(i => i.CloneWithoutSigScript()), 
                Outputs.Select(o => o.Clone()), LockTime, Testnet);
        }

        public Transaction CloneWithReplacedSigScript(TransactionInput input, Script script)
        {
            var inputs = Inputs.Select(i => i == input ? i.CloneWithSigScript(script) : i.CloneWithoutSigScript());
            return new Transaction(Version, Segwit, inputs, Outputs, LockTime, Testnet);
        }
    }
}
