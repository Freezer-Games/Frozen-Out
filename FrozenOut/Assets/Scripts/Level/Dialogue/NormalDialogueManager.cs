using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

using Scripts.Settings;
using Scripts.Level.Item;
using Scripts.Level.NPC;
using Scripts.Level.Dialogue.Utils;
using Scripts.Level.Dialogue.System;
using Scripts.Level.Mission;

namespace Scripts.Level.Dialogue
{
    public class NormalDialogueManager : DialogueManager
    {
        public LevelManager LevelManager;

        public MainDialogueSystem MainDialogueSystem;
        public SecondaryDialogueSystem InstagramDialogueSystem;
        public ChoiceSystem ChoiceSystem;

        public DialoguePromptController PromptController;

        public DialogueActer GameOverActer;

        private DialogueActer CurrentActer;
        private DialogueStyle CurrentStyle;

        private bool UserRequestedAllLine;
        private bool UserRequestedNextLine;

        private const float NextDialogueDelay = 0.3f;

        private SettingsManager SettingsManager => LevelManager.GetSettingsManager();
        private Inventory Inventory => LevelManager.GetInventory();
        private MissionManager MissionManager => LevelManager.GetMissionManager();
        private NPCManager NPCManager => LevelManager.GetNPCManager();

        void Awake()
        {
            SwitchToMain();
            SetLanguage();
        }

        void Start()
        {
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

        public void SetLanguage()
        {
            DialogueSystem.SetLanguage(SettingsManager.Locale);
        }

        public void SetVariable<T>(string variableName, T value, bool includeLeading = true)
        {
            MainDialogueSystem.SetVariable<T>(variableName, value, includeLeading);
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
        public override int QuantityOfItem(string itemVariableName)
        {
            ItemBase item = new ItemBase()
            {
                VariableName = itemVariableName
            };
            return Inventory.QuantityOfItem(item);
        }
        public override bool MarkMissionDone(string missionVariableName)
        {
            MissionBase mission = new MissionBase()
            {
                VariableName = missionVariableName
            };
            return MissionManager.IsMissionDone(mission);
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

                SwitchToMain();
                MainDialogueSystem.StartDialogue(acter);
            }
        }

        public override void StartGameOverDialogue()
        {
            if(IsRunning())
            {
                StopDialogue();
            }

            StartDialogue(GameOverActer);
        }

        public override void StopDialogue()
        {
            DialogueSystem.Stop();

            TextManager.Close();
            VoiceManager.Close();
        }

        private global::System.Action onSwitchBack;
        public override void SwitchToInstagram(global::System.Action onComplete)
        {
            DialogueSystem = InstagramDialogueSystem;
            DialogueSystem.RequestNextLine();

            onSwitchBack = onComplete;
        }

        public override void SwitchToMain()
        {
            onSwitchBack?.Invoke();
            onSwitchBack = null;

            DialogueSystem = MainDialogueSystem;
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

        #region Parser
        private void ProcessDialogue(string dialogue)
        {
            // Antes de hacer nada se analiza el texto y se clasifican internamente las partes con tags y las simples
            IDialogueText classifiedCharacterDialogue = DialogueTextAnalyser.AnalyseText(dialogue);

            if (CurrentStyle.Delay > 0.0f)
            {
                //StartCoroutine(DoParseAccumulated(classifiedCharacterDialogue));
                StartCoroutine(DoParseSingle(classifiedCharacterDialogue));
            }
            else
            {
                TextManager.ShowDialogueAccumulated(dialogue);
            }

            VoiceManager.SpeakDialogueAccumulated(classifiedCharacterDialogue.ToStringClean());
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

        public override void OnLineStarted()
        {
            TextManager.StartLine();
            VoiceManager.StartLine();
        }

        public override void OnLineStyleUpdated(string styleName)
        {
            DialogueStyle characterStyle = StylesController.GetStyle(styleName);
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

        public override void OnChoicesStarted(IEnumerable<DialogueChoice> dialogueChoice)
        {
            ChoiceSystem.StartChoice(dialogueChoice);
        }

        public override void OnChoiceSelected(DialogueChoice choice)
        {
            MainDialogueSystem.RequestSelectChoice(choice);
        }
        #endregion
    }
}