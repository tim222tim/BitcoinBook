using System.Collections.Generic;

namespace BitcoinBook
{
    public class SignerMap
    {
        readonly IDictionary<ScriptType, IInputSigner> dictionary;

        public SignerMap(PayToPubKeySigner payToPubKeySigner, PayToPubKeyHashSigner payToPubKeyHashSigner)
        {
            dictionary = new Dictionary<ScriptType, IInputSigner>
            {
                {ScriptType.PayToPubKey, payToPubKeySigner },
                {ScriptType.PayToPubKeyHash, payToPubKeyHashSigner },
            };
        }

        public IInputSigner? this[ScriptType scriptType] => dictionary.TryGetValue(scriptType, out var signer) ? signer : null;
    }
}
