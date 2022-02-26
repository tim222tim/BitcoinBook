using System;
using System.Threading.Tasks;

namespace BitcoinBook;

public class TransactionSigner
{
    readonly ITransactionFetcher fetcher;
    readonly ScriptClassifier scriptClassifier;
    readonly SignerMap signerMap;

    public TransactionSigner(ITransactionFetcher fetcher, ScriptClassifier scriptClassifier, SignerMap signerMap)
    {
        this.fetcher = fetcher;
        this.scriptClassifier = scriptClassifier;
        this.signerMap = signerMap;
    }

    public async Task<Transaction> SignTransaction(Wallet wallet, Transaction transaction, SigHashType sigHashType, bool ignoreKeyNotFound = false)
    {
        foreach (var input in transaction.Inputs)
        {
            var priorOutput = await fetcher.FetchPriorOutput(input);
            var scriptType = scriptClassifier.GetScriptType(priorOutput.ScriptPubKey);
            var signer = signerMap[scriptType];
            if (signer != null)
            {
                try
                {
                    var sigScript = await signer.CreateSigScript(wallet, transaction, input, sigHashType);
                    transaction = transaction.CloneWithReplacedSigScript(input, sigScript);
                }
                catch (PrivateKeyNotFoundException)
                {
                    if (!ignoreKeyNotFound)
                    {
                        throw;
                    }
                }
            }
            else if (!ignoreKeyNotFound)
            {
                throw new InvalidOperationException($"Unknown script type for prior output {input.PreviousTransaction}:{input.PreviousIndex}");
            }
        }

        return transaction;
    }
}