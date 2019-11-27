using System.Collections.Generic;

namespace Assets.Scripts.Dialogue
{
    public class SimpleDialogueSnippetSystem : DialogueSnippetSystem<string>
    {
        public List<string> Names;
        public List<string> Values;

        protected override void Start()
        {
            for (int i = 0; i < Names.Count; i++)
            {
                snippets[Names[i]] = Values[i];
            }

            base.Start();
        }
    }
}
