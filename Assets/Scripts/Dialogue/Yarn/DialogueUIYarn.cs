using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Yarn.Unity;

public class DialogueUIYarn : Yarn.Unity.DialogueUIBehaviour {
    public const string MAIN_NAME = "Pol";
    public const string LINE_SEPARATOR = ": ";

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
                    yield return ParseTag(stringBuilder, characterDialogue, index);
                    index = currentIndex;
                }
                else
                {
                    stringBuilder.Append(c);
                }
 
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

    private IEnumerator ParseTag(StringBuilder builder, string line, int index)
    {
        ExtractTag(line, index, out string tagOptionFull, out string tagOption, out TagOptionPosition tagOptionPosition, out string _);
        yield return ParseTag(builder, line, index, tagOptionFull, tagOption, tagOptionPosition);
    }

    private IEnumerator ParseTag(StringBuilder builder, string line, int index, string tagOptionFull, string tagOption, TagOptionPosition tagOptionPosition)
    {
        if (tagOptionPosition == TagOptionPosition.start)
        {
            while (index >= 0 && index < line.Length)
            {
                string lineWithoutInit = line.Substring(index + tagOptionFull.Length);
                int indexOfNextTagInit = lineWithoutInit.IndexOf(TAG_SEPARATOR_INIT);

                if (indexOfNextTagInit >= 0)
                {
                    ExtractTag(lineWithoutInit, indexOfNextTagInit, out string nextTagOptionFull, out string nextTagOption, out TagOptionPosition nextTagOptionPosition, out string _);
                    if (nextTagOptionPosition == TagOptionPosition.end)
                    {
                        if (tagOption == nextTagOption)
                        {
                            yield return RunTaggedLine(builder, line, tagOptionFull, nextTagOptionFull);
                        }
                        else
                        {
                            LogWarningEndTagBeforeStart(index);
                        }
                    }
                    else
                    {
                        index += tagOptionFull.Length + indexOfNextTagInit;
                    }
                }
                else
                {
                    LogWarningStartTagWithoutEnd(index);
                    currentIndex = index;
                    yield break;
                }
            }
        }
        else
        {
            LogWarningEndTagBeforeStart(index);
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

    private IEnumerator RunTaggedLine(StringBuilder builder, string line, string startTagOptionFull, string endTagOptionFull)
    {
        builder.Append(startTagOptionFull + endTagOptionFull);
        for (int indexWord = 0; indexWord < line.Length; indexWord++)
        {
            char w = line[indexWord];
            if (w.Equals(TAG_SEPARATOR_INIT))
            {
                yield return ParseTag(builder, line, indexWord);
                indexWord = currentIndex;
            }
            else
            {
                builder.Insert(builder.Length - endTagOptionFull.Length, w);
            }
            currentDialogueText.text = builder.ToString();
            yield return new WaitForSeconds(localDelay);
        }

        int taggedLineLength = startTagOptionFull.Length + line.Length + endTagOptionFull.Length;
        currentIndex += taggedLineLength;
    }

    private void LogWarningEndTagBeforeStart(int index) => Debug.LogWarning($"Warning: End tag before start (line {currentLineNumber}, position {index}). Skipping tag.");
    private void LogWarningStartTagWithoutEnd(int index) => Debug.LogWarning($"Warning: Start tag without end (line {currentLineNumber}, position {index}). Skipping tag.");

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