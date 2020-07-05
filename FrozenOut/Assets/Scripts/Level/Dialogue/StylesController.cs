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

            style.NormaliseDelay(0.1f, 0.1f, 1.0f);
            style.NormaliseSize(DialogueManager.GetTextSize(), 14, 24);
            style.NormaliseVolume(1.0f, 0.1f, 1.0f);
            style.NormalisePitch(2.0f, 1.0f, 3.0f);

            style.TextStyle.UpdateOptionals(DefaultStyle.TextStyle);
            style.VoiceStyle.UpdateOptionals(DefaultStyle.VoiceStyle);

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