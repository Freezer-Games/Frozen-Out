using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public class StylesController : MonoBehaviour
    {
        public DialogueManager DialogueManager;

        public DialogueStyle DefaultStyle;
        public List<CharacterDialogueStyle> CharacterStyles;

        private IDictionary<string, DialogueStyle> Styles;

        void Start()
        {
            Styles = new Dictionary<string, DialogueStyle>();

            AddStyles();
        }

        public DialogueStyle GetStyle(string characterName)
        {
            DialogueStyle style = GetRawStyle(characterName);

            style.NormaliseText(DialogueManager.TextManager.TextConfiguration);

            style.NormaliseVoice(DialogueManager.VoiceManager.VoiceConfiguration);

            return style;
        }

        private DialogueStyle GetRawStyle(string characterName)
        {
            DialogueStyle characterStyle = DefaultStyle;
            if (Styles.ContainsKey(characterName))
            {
                characterStyle = Styles[characterName];
            }

            return characterStyle;
        }

        private void AddStyles()
        {
            foreach (CharacterDialogueStyle characterStyle in CharacterStyles)
            {
                AddStyle(characterStyle.Name, characterStyle.Style);
            }
        }

        private void AddStyle(string characterName, DialogueStyle characterStyle)
        {
            Styles[characterName] = characterStyle;
        }
    }
}