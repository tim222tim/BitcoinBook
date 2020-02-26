﻿using System;
using System.Threading.Tasks; 

namespace BitcoinBook
{
    public class PayToPubKeySigner : TransactionSignerBase
    {
        public PayToPubKeySigner(ITransactionFetcher fetcher, TransactionHasher hasher) : base(fetcher, hasher)
        {
        }

        public override async Task<Script> CreateSigScript(PrivateKey privateKey, Transaction transaction, TransactionInput input, SigHashType sigHashType)
        {
            var signature = await ComputeSignatureBytes(privateKey, transaction, input, sigHashType);
            return new Script(signature);
        }

        public override async Task<Script> CreateSigScript(Wallet wallet, Transaction transaction, TransactionInput input, SigHashType sigHashType)
        {
            var priorOutput = await Fetcher.GetPriorOutput(input);
            if (priorOutput.ScriptPubKey.Commands.Count < 1 || 
                !(priorOutput.ScriptPubKey.Commands[0] is byte[] sec))
            {
                throw new FormatException("Unexpected opcode in output script");
            }
            var privateKey = wallet.FindBy(PublicKey.FromSec(sec));
            if (privateKey == null)
            {
                throw new InvalidOperationException("Key not found in wallet");
            }
            return await CreateSigScript(privateKey, transaction, input, sigHashType);
        }
    }
}
