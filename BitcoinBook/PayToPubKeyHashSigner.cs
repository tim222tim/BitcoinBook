﻿using System.Threading.Tasks;

namespace BitcoinBook
{
    public class PayToPubKeyHashSigner : TransactionSignerBase
    {
        public PayToPubKeyHashSigner(TransactionHasher hasher) : base(hasher)
        {
        }

        public override async Task<Script> CreateSigScript(PrivateKey privateKey, Transaction transaction, TransactionInput input, SigHashType sigHashType)
        {
            var signature = await ComputerSignatureBytes(privateKey, transaction, input, sigHashType);
            var sec = privateKey.PublicKey.ToSec();
            return new Script(signature, sec);
        }
    }
}