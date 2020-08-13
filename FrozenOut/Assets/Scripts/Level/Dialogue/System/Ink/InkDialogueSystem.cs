using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

using Ink.Runtime;

namespace Scripts.Level.Dialogue.System.Ink
{
    public class InkDialogueSystem : MainDialogueSystem
    {
        public DialogueManager DialogueManager;
        public InkCommands InkCommands;

        public List<TextAsset> NodeStories;

        private IDictionary<string, TextAsset> Stories;

        private Story Story;
        private bool RequestedContinue;

        private readonly DialogueLineSeparator DialogueSeparator = new DialogueLineSeparator("++", ": ");

        private void Start()
        {
            Stories = new Dictionary<string, TextAsset>();
            foreach(TextAsset json in NodeStories)
            {
                Stories.Add(json.name, json);
            }
        }

        public override bool IsRunning()
        {
            return Story != null;
        }

        public override void StartDialogue(DialogueActer acter)
        {
            if(Stories.ContainsKey(acter.StoryNode))
            {
                TextAsset selectedStory = Stories[acter.StoryNode];

                Story = new Story(selectedStory.text);

                StartCoroutine(DoStory());
            }
            else
            {
                Debug.LogError("The current node to talk does not exist - " + acter.StoryNode);
            }
        }

        public override void RequestNextLine()
        {
            RequestedContinue = true;
        }

        public override void RequestSelectChoice(DialogueChoice choice)
        {
            Story.ChooseChoiceIndex(choice.ID);

            RequestedContinue = true;
        }

        public override void Stop()
        {
            StopAllCoroutines();
            Story = null;
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

            if(!success)
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

        private object GetObjectValue(string variableName)
        {
            if (IsRunning())
            {
                return Story.variablesState[variableName];
            }

            return "";
        }

        public override void SetVariable<T>(string variableName, T value)
        {
            if (IsRunning())
            {
                Story.variablesState[variableName] = value;
            }
        }

        public override void SetLanguage(Locale locale)
        {
            // TODO
        }

        private IEnumerator DoStory()
        {
            DialogueManager.OnDialogueStarted();

            while(Story != null)
            {
                while (Story.canContinue)
                {
                    DialogueManager.OnLineStarted();

                    string line = Story.Continue();
                    IEnumerable<string> tags = Story.currentTags;

                    DialogueSeparator.Separate(line, out string characterStyleName, out string characterName, out string characterDialogue);

                    DialogueManager.OnLineStyleUpdated(characterStyleName);
                    DialogueManager.OnLineNameUpdated(characterName);
                    DialogueManager.OnLineDialogueUpdated(characterDialogue);

                    InkCommands.ChooseFunctionFromTags(tags);

                    while (!RequestedContinue)
                    {
                        yield return null;
                    }
                    RequestedContinue = false;
                }

                if (Story.currentChoices.Count > 0)
                {
                    IEnumerable<DialogueChoice> dialogueChoices = Story.currentChoices.Select((choice) =>
                    {
                        string text = choice.text;

                        DialogueChoice dialogueChoice = new DialogueChoice()
                        {
                            Text = text,
                            ID = choice.index
                        };

                        return dialogueChoice;
                    });

                    DialogueManager.OnChoicesStarted(dialogueChoices);

                    while (!RequestedContinue)
                    {
                        yield return null;
                    }
                    RequestedContinue = false;
                }
                else
                {
                    Story = null;
                }
            }

            yield return new WaitForEndOfFrame();

            DialogueManager.OnDialogueEnded();
        }
    }
}