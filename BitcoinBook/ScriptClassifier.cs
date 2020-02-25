using System;
using System.Collections.Generic;
using System.Linq;

namespace BitcoinBook
{
    public class ScriptClassifier
    {
        readonly Dictionary<ScriptType, object[]> templates = new Dictionary<ScriptType, object[]>
        {
            {ScriptType.Empty, new object[0]},
            {ScriptType.PayToPublicKeyHash, new object[] {                
                OpCode.OP_DUP,
                OpCode.OP_HASH160,
                new[] {20},
                OpCode.OP_EQUALVERIFY,
                OpCode.OP_CHECKSIG}},
        };

        public ScriptType GetScriptType(Script script)
        {
            if (script == null) throw new ArgumentNullException(nameof(script));
            var scriptType = templates.FirstOrDefault(t => Match(t.Value, script.Commands)).Key;
            return scriptType;
        }

        bool Match(IList<object> template, IList<object> commands)
        {
            if (template.Count != commands.Count)
            {
                return false;
            }

            for (var i = 0; i < template.Count; i++)
            {
                if (template[i] is OpCode opcode && (OpCode)commands[i] != opcode)
                {
                    return false;
                }

                if (template[i] is IEnumerable<int> lengths &&
                    !(commands[i] is byte[] bytes && lengths.Contains(bytes.Length)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
