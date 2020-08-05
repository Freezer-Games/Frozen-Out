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

        public List<TextAsset> NodeStories;

        private IDictionary<string, TextAsset> Stories;

        private Story Story;

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
            if(Stories.ContainsKey(acter.TalkToNode))
            {
                TextAsset selectedStory = Stories[acter.TalkToNode];

                Story = new Story(selectedStory.text);

                StartCoroutine(DoStory());
            }
            else
            {
                Debug.LogError("The current node to talk does not exist - " + acter.TalkToNode);
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

        public override void SetVariable<T>(string variableName, T value, bool includeLeading = true)
        {
            if (IsRunning())
            {
                if (includeLeading)
                {
                    variableName = AddLeading(variableName);
                }

                Story.variablesState[variableName] = value;
            }
        }

        public override bool GetBoolVariable(string variableName, bool includeLeading = true)
        {
            object variable = GetObjectValue(variableName, includeLeading);

            bool success = bool.TryParse(variable.ToString(), out bool result);

            if(!success)
            {
                result = false;
            }

            return result;
        }

        public override string GetStringVariable(string variableName, bool includeLeading = true)
        {
            object variable = GetObjectValue(variableName, includeLeading);

            string result = variable.ToString();

            return result;
        }

        public override float GetNumberVariable(string variableName, bool includeLeading = true)
        {
            object variable = GetObjectValue(variableName, includeLeading);

            bool success = float.TryParse(variable.ToString(), out float result);

            if (!success)
            {
                result = 0.0f;
            }

            return result;
        }

        private object GetObjectValue(string variableName, bool includeLeading = true)
        {
            if (IsRunning())
            {
                if (includeLeading)
                {
                    variableName = AddLeading(variableName);
                }

                return Story.variablesState[variableName];
            }

            return "";
        }

        private string AddLeading(string variableName)
        {
            return "$" + variableName;
        }

        public override void SetLanguage(Locale locale)
        {
            // TODO
        }

        private const string StyleSeparator = "++";
        private const string DialogueSeparator = ": ";
        private bool RequestedContinue;

        private IEnumerator DoStory()
        {
            DialogueManager.OnDialogueStarted();

            while(Story != null)
            {
                while (Story.canContinue)
                {
                    DialogueManager.OnLineStarted();

                    string line = Story.Continue();

                    SeparateNameAndDialogue(line, out string characterStyleName, out string characterName, out string characterDialogue);

                    DialogueManager.OnLineStyleUpdated(characterStyleName);
                    DialogueManager.OnLineNameUpdated(characterName);
                    DialogueManager.OnLineDialogueUpdated(characterDialogue);

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

        private void SeparateNameAndDialogue(string text, out string style, out string name, out string dialogue)
        {
            int indexOfStyleSeparator = text.IndexOf(StyleSeparator);
            int indexOfDialogueSeparator = text.IndexOf(DialogueSeparator);

            if (indexOfDialogueSeparator == -1) // No se especifica nombre
            {
                name = "";
                style = "";
                dialogue = text;
            }
            else
            {
                if (indexOfStyleSeparator == -1) // No hay estilo adicional
                {
                    name = text.Substring(0, indexOfDialogueSeparator);
                    style = name;
                }
                else
                {
                    name = text.Substring(0, indexOfStyleSeparator);

                    int styleLength = indexOfDialogueSeparator - (indexOfStyleSeparator + StyleSeparator.Length);
                    style = text.Substring(indexOfStyleSeparator + StyleSeparator.Length, styleLength);
                }
                dialogue = text.Substring(indexOfDialogueSeparator + DialogueSeparator.Length);
            }
        }
    }
}