using Assets.Scripts.Dialogue.Texts.Snippets;
using System.Collections.Generic;

namespace Assets.Scripts.Dialogue
{
    public class VariableSnippetSystem : DialogueSnippetSystem<object>
    {
        public static readonly SnippetFormat<object> VariableYarnFormat = new SnippetFormat<object>("$", " ");

        private VariableStorageYarn variableStorageYarn;

        protected override void Start()
        {
            variableStorageYarn = FindObjectOfType<VariableStorageYarn>();

            if (variableStorageYarn.IsInitialized)
            {
                var variables = variableStorageYarn.Variables;
                Init(variables);
            }

            variableStorageYarn.Initialized += (sender, variables) => Init(variables);
        }

        private void Init(IReadOnlyDictionary<string, Yarn.Value> variables)
        {
            snippets.Clear();

            foreach (string key in variables.Keys)
            {
                snippets[key] = variables[key].AsString;
            }

            VariableYarnFormat.Snippets = snippets;
            Format = VariableYarnFormat;
        }
    }
}
