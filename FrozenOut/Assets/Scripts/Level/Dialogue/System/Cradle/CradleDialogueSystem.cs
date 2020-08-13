using System.Collections.Generic;
using UnityEngine.Localization;

using Cradle;

namespace Scripts.Level.Dialogue.System.Cradle
{
    public class CradleDialogueSystem : MainDialogueSystem
    {
        public DialogueManager DialogueManager;

        public Story Story;

        private readonly DialogueLineSeparator DialogueSeparator = new DialogueLineSeparator("++", ": ");

        void Start()
        {
            ConvertEvents();
        }

        public override bool IsRunning()
        {
            return Story.State == StoryState.Playing;
        }

        public override void StartDialogue(DialogueActer acter)
        {
            //Story.Begin();
            Story.GoTo(acter.StoryNode);
            DialogueManager.OnDialogueStarted();
        }

        public override void RequestNextLine()
        {
            Story.Resume();
        }

        public override void RequestSelectChoice(DialogueChoice choice)
        {
            Story.DoLink(choice.Text);
        }

        public override void Stop()
        {
            Story.Pause();
        }

        public override void SetLanguage(Locale locale)
        {
            // TODO
        }

        public override void Switch()
        {
            // TODO
        }

        #region Proxy
        public void PickItem(string itemVariableName, int quantity)
        {
            DialogueManager.PickItem(itemVariableName, quantity);
        }
        public void UseItem(string itemVariableName, int quantity)
        {
            DialogueManager.UseItem(itemVariableName, quantity);
        }
        public void SetNPCAnimation(string npcName, string animation)
        {
            DialogueManager.SetNPCAnimation(npcName, animation);
        }
        public void SetNPCAnimationWithSimilarName(string npcName, string animation)
        {
            DialogueManager.SetNPCAnimationWithSimilarName(npcName, animation);
        }
        public void StopNPCAnimation(string npcName)
        {
            DialogueManager.StopNPCAnimation(npcName);
        }
        public void StopNPCAnimationWithSimilarName(string npcName)
        {
            DialogueManager.StopNPCAnimationWithSimilarName(npcName);
        }
        #endregion

        public override bool GetBoolVariable(string variableName)
        {
            object variable = GetObjectValue(variableName);

            bool success = bool.TryParse(variable.ToString(), out bool result);

            if (!success)
            {
                result = false;
            }

            return result;
        }

        public override string GetStringVariable(string variableName)
        {
            object variable = GetObjectValue(variableName);

            string result = variable.ToString();

            return result;
        }

        public override float GetNumberVariable(string variableName)
        {
            object variable = GetObjectValue(variableName);

            bool success = float.TryParse(variable.ToString(), out float result);

            if (!success)
            {
                result = 0.0f;
            }

            return result;
        }

        public override void SetVariable<T>(string variableName, T value)
        {
            StoryVar var = new StoryVar(value);

            Story.Vars.SetMember(variableName, var);
        }

        private object GetObjectValue(string variableName)
        {
            StoryVar var = Story.Vars[variableName];

            return var.InnerValue;
        }

        #region Events
        private void ConvertEvents()
        {
            Story.OnOutput += OnOutput;
            Story.OnPassageEnter += OnPassageEnter; 
        }

        private void OnPassageEnter(StoryPassage obj)
        {
            DialogueManager.OnLineStarted();
        }

        private void OnOutput(StoryOutput output)
        {
            if(output is StoryText)
            {
                string line = output.Text;

                DialogueSeparator.Separate(line, out string characterStyleName, out string characterName, out string characterDialogue);

                DialogueManager.OnLineStyleUpdated(characterStyleName);
                DialogueManager.OnLineNameUpdated(characterName);
                DialogueManager.OnLineDialogueUpdated(characterDialogue);
            }
            else if(output is StoryLink)
            {
                IList<DialogueChoice> choices = new List<DialogueChoice>();
                choices.Add(new DialogueChoice()
                {
                    ID = 0,
                    Text = output.Text,
                });

                DialogueManager.OnChoicesStarted(choices);
            }
        }
        #endregion
    }
}