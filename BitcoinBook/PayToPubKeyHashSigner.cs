using System;
using System.Threading.Tasks;

namespace BitcoinBook
{
    public class PayToPubKeyHashSigner : InputSignerBase
    {
        public PayToPubKeyHashSigner(ITransactionFetcher fetcher, TransactionHasher hasher) : base(fetcher, hasher)
        {
        }

        public override async Task<Script> CreateSigScript(PrivateKey privateKey, Transaction transaction, TransactionInput input, SigHashType sigHashType)
        {
            var signature = await ComputeSignatureBytes(privateKey, transaction, input, sigHashType);
            var sec = privateKey.PublicKey.ToSec();
            return new Script(signature, sec);
        }

        public override async Task<Script> CreateSigScript(Wallet wallet, Transaction transaction, TransactionInput input, SigHashType sigHashType)
        {
            var priorOutput = await Fetcher.GetPriorOutput(input);
            if (priorOutput.ScriptPubKey.Commands.Count <3 ||
                !(priorOutput.ScriptPubKey.Commands[2] is byte[] hash))
            {
                throw new FormatException("Unexpected opcode in output script");
            }
            var privateKey = wallet.FindByHash(hash);
            if (privateKey == null)
            {
                throw new InvalidOperationException("Key not found in wallet");
            }
            return await CreateSigScript(privateKey, transaction, input, sigHashType);
        }
    }
}
