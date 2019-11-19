using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Yarn.Unity;

public class DialogueUIYarn : Yarn.Unity.DialogueUIBehaviour {
    public const string MAIN_NAME = "Pol";
    public const string LINE_SEPARATOR = ": ";

    public const char TAG_SEPARATOR_INIT = '<';
    public const char TAG_SEPARATOR_END = '>';
    public const char TAG_OPTION_END = '/';

    //Where name of character will be displayed
    public Text mainNameText;
    //Where current dialogue will be displayed
    public Text mainDialogueText;
    public Text otherNameText;
    public Text otherDialogueText;

    //Place where name and dialogue will be contained
    public GameObject dialogueBoxGUI;
    public Text continuePrompt;

    public float letterDelay = 0.1f;

    public AudioClip audioClip;

    private Text currentNameText, currentDialogueText;

    private AudioSource audioSource;
    private float localDelay;
    private readonly float localDelayMultiplier = 1.5f;
	
	private DialogueRunner dialogueSystem;
    private int currentLineNumber;
    private int currentIndex;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
		dialogueSystem = FindObjectOfType<DialogueRunner>();

        if (dialogueBoxGUI != null) {
            dialogueBoxGUI.SetActive(false);
        }
        if(continuePrompt != null) {
            continuePrompt.gameObject.SetActive(false);
        }

        mainNameText.text = "";
        mainDialogueText.text = "";

        otherNameText.text = "";
        otherDialogueText.text = "";
    }

    void FixedUpdate() {
        if (dialogueSystem.isDialogueRunning && Input.anyKey)
        {
            localDelay /= localDelayMultiplier;
        }
    }

    public override IEnumerator RunLine(Yarn.Line line)
    {
        currentLineNumber++;
        currentIndex = 0;

        SeparateLine(line.text, out string characterName, out string characterDialogue);

        GetCurrentDialogueText(characterName);

        currentDialogueText.gameObject.SetActive(true);

        if (letterDelay > 0.0f)
        {
            // Display the line one character at a time
            var stringBuilder = new StringBuilder();

            localDelay = letterDelay;
			
			for(int index = currentIndex; index < characterDialogue.Length; index++) {
                currentIndex = index;
				char c = characterDialogue[index];
                if (c.Equals(TAG_SEPARATOR_INIT))
                {
                    ExtractTag(characterDialogue, index, out string tagOptionFull, out string tagOption, out TagOptionPosition tagOptionPosition, out string remainingText);
                    int nextIndex = index + tagOptionFull.Length;
                    if (tagOptionPosition == TagOptionPosition.start)
                    {
                        yield return RunTaggedLine(stringBuilder, remainingText, nextIndex + 1, tagOptionFull, tagOption);
                    }
                    else
                    {
                        LogWarningEndTagBeforeStart(index);
                    }
                    index = currentIndex;
                }

                stringBuilder.Append(c);
                currentDialogueText.text = stringBuilder.ToString();
                yield return new WaitForSeconds(localDelay);
			}
        }
        currentDialogueText.text = characterDialogue;

        // Show the 'press any key' prompt when done, if we have one
        if (continuePrompt != null)
        {
            continuePrompt.gameObject.SetActive(true);
        }

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        currentDialogueText.gameObject.SetActive(false);

        if (continuePrompt != null)
        {
            continuePrompt.gameObject.SetActive(false);
        }
    }

    private void GetCurrentDialogueText(string characterName)
    {
        currentNameText = mainNameText;
        currentDialogueText = mainDialogueText;
        if (characterName != MAIN_NAME)
        {
            currentNameText = otherNameText;
            currentDialogueText = otherDialogueText;
        }

        currentNameText.text = characterName;
    }

    private IEnumerator RunTaggedLine(StringBuilder builder, string line, int startIndex, string tagOptionFull, string tagOption)
    {
        int indexOfNextTagInit = line.IndexOf(TAG_SEPARATOR_INIT);

        if (indexOfNextTagInit >= 0)
        {
            ExtractTag(line, indexOfNextTagInit, out string nextTagOptionFull, out string nextTagOption, out TagOptionPosition tagOptionPosition, out string nextLine);
            int nextIndex = startIndex + tagOptionFull.Length;
            if (tagOptionPosition == TagOptionPosition.end)
            {             
                if (tagOption == nextTagOption)
                {
                    builder.Append(tagOptionFull + nextTagOptionFull);                 
                    for (int indexWord = 0; indexWord < indexOfNextTagInit; indexWord++)
                    {
                        char w = line[indexWord];
                        builder.Insert(builder.Length - nextTagOptionFull.Length, w);
                        yield return new WaitForSeconds(localDelay);
                    }
                    nextIndex = indexOfNextTagInit + nextTagOptionFull.Length;
                }
                else
                {
                    LogWarningEndTagBeforeStart(startIndex);
                }
            }
            else
            {
                RunTaggedLine(builder, nextLine, nextIndex + 1, tagOptionFull, tagOption);          
            }
            currentIndex = nextIndex;
        }
        else
        {
            LogWarningStartTagWithoutEnd(startIndex);
            currentIndex = startIndex;
        }
    }

    private void LogWarningEndTagBeforeStart(int index) => Debug.LogWarning($"Warning: End tag before start (line {currentLineNumber}, position {index}). Skipping tag.");
    private void LogWarningStartTagWithoutEnd(int index) => Debug.LogWarning($"Warning: Start tag without end (line {currentLineNumber}, position {index}). Skipping tag.");

    private static void ExtractTag(string line, int startIndex, out string tagOptionFull, out string tagOption, out TagOptionPosition tagOptionPosition, out string remainingText)
    {
        string remainingTextWithStart = line.Substring(startIndex);

        int indexOfSeparatorStartEnd = remainingTextWithStart.IndexOf(TAG_SEPARATOR_END);
        tagOptionFull = remainingTextWithStart.Substring(0, indexOfSeparatorStartEnd + 1);
        tagOption = ExtractTagOption(tagOptionFull);

        int indexOfOptionEnd = tagOptionFull.IndexOf(TAG_OPTION_END);
        if (indexOfOptionEnd < 0)
        {
            tagOptionPosition = TagOptionPosition.start;
        }
        else
        {
            tagOptionPosition = TagOptionPosition.end;
            tagOption = tagOption.Remove(indexOfOptionEnd - 1, 1);
        }

        remainingText = remainingTextWithStart.Substring(indexOfSeparatorStartEnd + 1);
    }

    private static string ExtractTagOption(string tagOptionFull) => tagOptionFull.Substring(1, tagOptionFull.Length - 2);

    public override IEnumerator RunOptions(Yarn.Options optionsCollection, Yarn.OptionChooser optionChooser) {
        yield return null;
    }

    public override IEnumerator RunCommand (Yarn.Command command)
    {
        yield return null;
	}

    public override IEnumerator DialogueStarted ()
    {
        // Enable the dialogue controls.
        if (dialogueBoxGUI != null) {
            dialogueBoxGUI.SetActive(true);
        }

        mainNameText.text = "";
        otherNameText.text = "";

        currentLineNumber = 0;

        yield break;
    }

    public override IEnumerator DialogueComplete ()
    {
        // Hide the dialogue interface.
        if (dialogueBoxGUI != null)
            dialogueBoxGUI.SetActive(false);

        yield break;
    }

    public void SeparateLine(string text, out string name, out string dialogue)
    {
        int indexOfNameSeparator = text.IndexOf(LINE_SEPARATOR);
        name = text.Substring(0, indexOfNameSeparator);
        dialogue = text.Substring(indexOfNameSeparator + 2);
    }
}

public enum TagOptionPosition { start, end }