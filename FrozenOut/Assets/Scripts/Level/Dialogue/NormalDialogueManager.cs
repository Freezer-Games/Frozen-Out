using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Scripts.Settings;
using Scripts.Level.Dialogue.Utils;

namespace Scripts.Level.Dialogue
{
    public class NormalDialogueManager : DialogueManager
    {
        public LevelManager LevelManager;

        public DialoguePromptController PromptController;

        private SettingsManager SettingsManager => LevelManager.GetSettingsManager();
        private DialogueTalker CurrentTalker;
        private DialogueStyle CurrentStyle;
        
        private IDictionary<string, DialogueStyle> Styles;
        public DialogueStyle DefaultStyle;

        private bool UserRequestedAllLine;
        private bool UserRequestedNextLine;

        private const float LetterDelay = 0.1f;
        private const float NextDialogueDelay = 0.3f;
        private const string PlayerName = "Pol";

        void Awake()
        {
            SetLanguage();
        }

        void Start()
        {
            SubscribeToEvents();
            SetStyles();
        }
        void Update()
        {
            if (IsRunning() && Input.GetKeyDown(GetNextDialogueKey()))
            {
                UserRequestedAllLine = true;
                UserRequestedNextLine = true;
            }
        }

        #region Settings
        public override KeyCode GetNextDialogueKey()
        {
            return SettingsManager.NextDialogueKey;
        }

        public override KeyCode GetInteractKey()
        {
            return SettingsManager.InteractKey;
        }

        public override int GetTextSize()
        {
            return SettingsManager.TextSize;
        }
        #endregion

        #region System
        public override bool IsReady()
        {
            return !IsRunning() && LevelManager.GetPlayerManager().IsGrounded;
        }

        public override bool IsRunning()
        {
            return DialogueSystem.IsRunning();
        }

        public override void StartDialogue(DialogueTalker talker)
        {
            CurrentTalker = talker;
            AddStyle(talker.Name, talker.Style);
            DialogueSystem.StartDialogue(talker);
        }

        public override void StopDialogue()
        {
            DialogueSystem.Stop();
        }

        public override void SetLanguage()
        {
            DialogueSystem.SetLanguage(SettingsManager.Locale);
        }
        #endregion

        #region Variables
        public override bool GetBoolVariable(string variableName, bool includeLeading = true)
        {
            return DialogueSystem.GetBoolVariable(variableName, includeLeading);
        }

        public override string GetStringVariable(string variableName, bool includeLeading = true)
        {
            return DialogueSystem.GetStringVariable(variableName, includeLeading);
        }

        public override float GetNumberVariable(string variableName, bool includeLeading = true)
        {
            return DialogueSystem.GetNumberVariable(variableName, includeLeading);
        }

        public override void SetVariable<T>(string variableName, T value, bool includeLeading = true)
        {
            DialogueSystem.SetVariable<T>(variableName, value, includeLeading);
        }
        #endregion

        public override void OpenTalkPrompt(DialogueTalker dialogueTalker)
        {
            PromptController.Open(dialogueTalker);
        }

        public override void CloseTalkPrompt()
        {
            PromptController.Close();
        }

        #region Style
        private DialogueStyle GetUpdatedStyle(string characterName)
        {
            DialogueStyle style = GetStyle(characterName);

            style.NormaliseDelay(LetterDelay, 0.1f, 1.0f);
            style.NormaliseSize(GetTextSize(), 14, 24);
            style.NormaliseVolume(1.0f, 0.1f, 1.0f);

            style.TextStyle.UpdateOptionals(DefaultStyle.TextStyle);
            style.VoiceStyle.UpdateOptionals(DefaultStyle.VoiceStyle);

            /*style.UpdateDelay(LetterDelay);
            style.UpdateSize(GetTextSize());
            style.UpdateFont(DefaultStyle.Font);*/

            return style;
        }

        private DialogueStyle GetStyle(string characterName)
        {
            DialogueStyle characterStyle = DefaultStyle;
            if (Styles.ContainsKey(characterName))
            {
                characterStyle = Styles[characterName];
            }

            return characterStyle;
        }

        private void AddStyle(string characterName, DialogueStyle characterStyle)
        {
            if (characterStyle != null)
            {
                Styles[characterName] = characterStyle;
            }
        }

        private void SetStyles()
        {
            Styles = new Dictionary<string, DialogueStyle>();
            AddStyle(PlayerName, DefaultStyle);
        }
        #endregion

        #region Parser
        private void ProcessDialogue(string dialogue)
        {
            // Antes de hacer nada se analiza el texto y se clasifican internamente las partes con tags y las simples
            IDialogueText classifiedCharacterDialogue = ComplexDialogueText.AnalyzeText(dialogue);

            if (CurrentStyle.Delay > 0.0f)
            {
                StartCoroutine(DoParseAccumulated(classifiedCharacterDialogue));
                //StartCoroutine(DoParseSingle(classifiedCharacterDialogue));
            }
            else
            {
                TextManager.ShowDialogueAccumulated(dialogue);
            }

            VoiceManager.Speak(classifiedCharacterDialogue.ToStringClean());
        }

        private IEnumerator DoParseAccumulated(IDialogueText dialogueText)
        {
            UserRequestedAllLine = false;

            foreach (string currentText in dialogueText.ParseAccumulated())
            {
                TextManager.ShowDialogueAccumulated(currentText);

                if (UserRequestedAllLine)
                {
                    TextManager.ShowDialogueAccumulated(dialogueText.ToStringFull());
                    break;
                }

                yield return new WaitForSeconds(CurrentStyle.Delay);
            }

            yield return new WaitForSeconds(NextDialogueDelay);

            UserRequestedNextLine = false;
            while (!UserRequestedNextLine)
            {
                yield return null;
            }

            DialogueSystem.RequestNextLine();
        }

        public IEnumerator DoParseSingle(IDialogueText dialogueText)
        {
            UserRequestedAllLine = false;

            foreach (string currentLetter in dialogueText.ParseSingle())
            {
                TextManager.ShowDialogueSingle(currentLetter);

                if (UserRequestedAllLine)
                {
                    TextManager.ShowDialogueAccumulated(dialogueText.ToStringFull());
                    break;
                }

                yield return new WaitForSeconds(CurrentStyle.Delay);
            }

            yield return new WaitForSeconds(NextDialogueDelay);

            UserRequestedNextLine = false;
            while (!UserRequestedNextLine)
            {
                yield return null;
            }

            DialogueSystem.RequestNextLine();
        }
        #endregion

        #region Events
        private void SubscribeToEvents()
        {
            Started += OnStartDialogue;
            Ended += OnEndDialogue;

            LineNameUpdated += OnLineNameUpdated;
            LineDialogueUpdated += OnLineDialogueUpdated;
        }

        private void OnStartDialogue(object sender, EventArgs args)
        {
            CurrentTalker?.OnStartTalk();

            TextManager.Open();
            VoiceManager.Open();
        }

        private void OnEndDialogue(object sender, EventArgs args)
        {
            CurrentTalker?.OnEndTalk();
            CurrentTalker = null;

            TextManager.Close();
            VoiceManager.Close();
        }

        private void OnLineNameUpdated(object sender, string name)
        {
            DialogueStyle characterStyle = GetUpdatedStyle(name);
            CurrentStyle = characterStyle;

            TextManager.SetStyle(characterStyle.TextStyle);
            TextManager.ShowName(name);

            VoiceManager.SetStyle(characterStyle.VoiceStyle);
        }

        private void OnLineDialogueUpdated(object sender, string dialogue)
        {
            ProcessDialogue(dialogue);
        }
        #endregion
    }
}