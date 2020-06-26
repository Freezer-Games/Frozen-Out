using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

using Scripts.Settings;
using Scripts.Level.Item;
using Scripts.Level.NPC;
using Scripts.Level.Dialogue.Utils;
using Scripts.Level.Dialogue.Runner.Instagram;
using Scripts.Level.Dialogue.Runner;

namespace Scripts.Level.Dialogue
{
    public class NormalDialogueManager : DialogueManager
    {
        public LevelManager LevelManager;

        public InstagramSystem InstagramDialogueSystem;
        public DialogueSystem NPCDialogueSystem;
        //public ConversationSystem ConversationDialogueSystem;

        public DialoguePromptController PromptController;

        private SettingsManager SettingsManager => LevelManager.GetSettingsManager();
        private Inventory Inventory => LevelManager.GetInventory();
        private NPCManager NPCManager => LevelManager.GetNPCManager();

        public DialogueActer GameOverActer;

        private DialogueActer CurrentActer;
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
            DialogueSystem = NPCDialogueSystem;
            SetLanguage();
        }

        void Start()
        {
            StartStyles();

            SetVariable<string>("crouchKey", SettingsManager.CrouchKey.ToString());
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
        public override bool IsItemInInventory(string itemVariableName)
        {
            ItemBase item = new ItemBase()
            {
                VariableName = itemVariableName
            };
            return Inventory.IsItemInInventory(item);
        }
        public override bool IsItemUsed(string itemVariableName)
        {
            ItemBase item = new ItemBase()
            {
                VariableName = itemVariableName
            };
            return Inventory.IsItemUsed(item);
        }
        public override void PickItem(string itemVariableName, int quantity)
        {
            ItemPickerInfo pickerInfo = new ItemPickerInfo()
            {
                VariableName = itemVariableName,
                Quantity = quantity
            };
            Inventory.PickItem(pickerInfo);
        }
        public override void UseItem(string itemVariableName, int quantity)
        {
            ItemUserInfo userInfo = new ItemUserInfo()
            {
                VariableName = itemVariableName
            };
            Inventory.UseItem(userInfo);
        }

        public override void SetNPCAnimation(string npcName, string animation)
        {
            NPCManager.StartAnimation(npcName, animation);
        }

        public override void SetNPCAnimationWithSimilarName(string npcName, string animation)
        {
            NPCManager.StartAnimationWithSimilarName(npcName, animation);
        }

        public override void StopNPCAnimation(string npcName)
        {
            NPCManager.StopAnimation(npcName);
        }

        public override void StopNPCAnimationWithSimilarName(string npcName)
        {
            NPCManager.StopAnimationsWithSimilarName(npcName);
        }

        public override GameObject GetPlayer()
        {
            return LevelManager.GetPlayerManager().Player;
        }
        #endregion

        #region System
        public override bool IsReady()
        {
            return IsEnabled() && !IsRunning() && LevelManager.GetPlayerManager().IsGrounded;
        }

        public override bool IsRunning()
        {
            return DialogueSystem.IsRunning();
        }

        public override void StartDialogue(DialogueActer acter)
        {
            if (IsEnabled() && IsReady())
            {
                CurrentActer = acter;

                AddStyle(acter.Style.Name, acter.Style.Style);
                foreach (CharacterDialogueStyle characterStyle in acter.ExtraStyles)
                {
                    AddStyle(characterStyle.Name, characterStyle.Style);
                }

                if(acter is DialogueInstagram)
                {
                    DialogueSystem = InstagramDialogueSystem;
                }
                else
                {
                    DialogueSystem = NPCDialogueSystem;
                }

                DialogueSystem.StartDialogue(acter);
            }
        }

        public override void StartGameOverDialogue()
        {
            StartDialogue(GameOverActer);
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
            return NPCDialogueSystem.GetBoolVariable(variableName, includeLeading);
        }

        public override string GetStringVariable(string variableName, bool includeLeading = true)
        {
            return NPCDialogueSystem.GetStringVariable(variableName, includeLeading);
        }

        public override float GetNumberVariable(string variableName, bool includeLeading = true)
        {
            return NPCDialogueSystem.GetNumberVariable(variableName, includeLeading);
        }

        public override void SetVariable<T>(string variableName, T value, bool includeLeading = true)
        {
            NPCDialogueSystem.SetVariable<T>(variableName, value, includeLeading);
        }
        #endregion

        public override void OpenTalkPrompt(DialogueActer dialogueActer)
        {
            if (IsEnabled())
            {
                PromptController.Open(dialogueActer);
            }
        }

        public override void CloseTalkPrompt(DialogueActer dialogueActer)
        {
            PromptController.Close(dialogueActer);
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

        private void StartStyles()
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
                //StartCoroutine(DoParseAccumulated(classifiedCharacterDialogue));
                StartCoroutine(DoParseSingle(classifiedCharacterDialogue));
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

                if (UserRequestedAllLine && !CurrentActer.IsAutomatic)
                {
                    yield return null;
                }
                else
                {
                    yield return new WaitForSeconds(CurrentStyle.Delay);
                }
            }

            yield return new WaitForSeconds(NextDialogueDelay);

            if(!CurrentActer.IsAutomatic)
            {
                UserRequestedNextLine = false;
                while (!UserRequestedNextLine)
                {
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(NextDialogueDelay);
            }

            DialogueSystem.RequestNextLine();
        }
        #endregion

        #region Events
        public override void OnDialogueStarted()
        {
            if(CurrentActer != null && CurrentActer.IsBlocking)
            {
                base.OnDialogueStarted();
            }

            CurrentActer?.OnStartTalk();

            TextManager.Open();
            VoiceManager.Open();
        }

        public override void OnDialogueEnded()
        {
            if(CurrentActer != null && CurrentActer.IsBlocking)
            {
                base.OnDialogueEnded();
            }

            CurrentActer?.OnEndTalk();
            CurrentActer = null;

            StopAllCoroutines();

            TextManager.Close();
            VoiceManager.Close();
        }

        public override void OnLineStyleUpdated(string styleName)
        {
            DialogueStyle characterStyle = GetUpdatedStyle(styleName);
            CurrentStyle = characterStyle;

            TextManager.SetStyle(characterStyle.TextStyle);

            VoiceManager.SetStyle(characterStyle.VoiceStyle);
        }

        public override void OnLineNameUpdated(string name)
        {
            TextManager.ShowName(name);
        }

        public override void OnLineDialogueUpdated(string dialogue)
        {
            ProcessDialogue(dialogue);
        }
        #endregion
    }
}