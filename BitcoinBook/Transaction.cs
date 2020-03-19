using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace BitcoinBook
{
    public class Transaction : ICloneable
    {
        readonly List<TransactionInput> inputs;
        readonly List<TransactionOutput> outputs;

        public string Id { get; }
        public bool Segwit { get; }
        public int Version { get; }
        public ReadOnlyCollection<TransactionInput> Inputs => inputs.AsReadOnly();
        public ReadOnlyCollection<TransactionOutput> Outputs => outputs.AsReadOnly();
        public uint LockTime { get; }
        public bool Testnet { get; }

        public Transaction(int version, bool segwit, IEnumerable<TransactionInput> inputs, IEnumerable<TransactionOutput> outputs, uint lockTime, bool testnet = false)
        {
            Version = version;
            Segwit = segwit;
            this.inputs = new List<TransactionInput>(inputs ?? throw new ArgumentNullException(nameof(inputs)));
            this.outputs = new List<TransactionOutput>(outputs ?? throw new ArgumentNullException(nameof(outputs)));
            LockTime = lockTime;
            Testnet = testnet;
            Id = ComputeId();
        }

        string ComputeId()
        {
            var transaction = Segwit ? CloneNonMalleable() : this;
            var stream = new MemoryStream();
            var writer = new TransactionWriter(stream);
            writer.Write(transaction);
            var hash = Cipher.Hash256(stream.ToArray());
            Array.Reverse(hash);
            return hash.ToHex();
        }

        public byte[] ToBytes()
        {
            var stream = new MemoryStream();
            var writer = new TransactionWriter(stream);
            writer.Write(this);
            return stream.ToArray();
        }

        public string ToHex()
        {
            return ToBytes().ToHex();
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

        public Transaction CloneWithReplacedInput(TransactionInput oldInput, TransactionInput newInput)
        {
            var newInputs = Inputs.Select(i => i == oldInput ? newInput : i);
            return new Transaction(Version, Segwit, newInputs, Outputs, LockTime, Testnet);
        }

        public Transaction CloneWithReplacedOutput(TransactionOutput oldOutput, TransactionOutput newOutput)
        {
            var newOutputs = Outputs.Select(o => o == oldOutput ? newOutput : o);
            return new Transaction(Version, Segwit, Inputs, newOutputs, LockTime, Testnet);
        }

        public Transaction CloneWithReplacedSigScript(TransactionInput input, Script script)
        {
            var newInputs = Inputs.Select(i => i == input ? i.CloneWithSigScript(script) : i.CloneWithoutSigScript());
            return new Transaction(Version, Segwit, newInputs, Outputs, LockTime, Testnet);
        }

        public Transaction CloneNonMalleable()
        {
            return new Transaction(Version, false, Inputs.Select(i => i.CloneWithWitness(new Script())),
                Outputs.Select(o => o.Clone()), LockTime, Testnet);
        }
    }
}
