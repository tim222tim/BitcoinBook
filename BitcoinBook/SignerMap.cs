using System.Collections.Generic;

namespace BitcoinBook
{
    public class SignerMap
    {
        readonly IDictionary<ScriptType, ITransactionSigner> dictionary;

        public SignerMap(PayToPubKeySigner payToPubKeySigner, PayToPubKeyHashSigner payToPubKeyHashSigner)
        {
            dictionary = new Dictionary<ScriptType, ITransactionSigner>
            {
                {ScriptType.PayToPubKey, payToPubKeySigner },
                {ScriptType.PayToPubKeyHash, payToPubKeyHashSigner },
            };
        }

        public ITransactionSigner this[ScriptType scriptType] => dictionary[scriptType];
    }
}
