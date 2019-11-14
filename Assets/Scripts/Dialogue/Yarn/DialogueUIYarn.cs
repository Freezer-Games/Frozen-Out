using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Yarn.Unity;

public class DialogueUIYarn : Yarn.Unity.DialogueUIBehaviour {

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
    
    private string MAIN_NAME = "Pol";
	private string LINE_SEPARATOR = ": ";
	
	private char TAG_SEPARATOR_INIT = '<';
	private char TAG_SEPARATOR_END = '>';
	
	private DialogueRunner dialogueSystem;

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
        SeparateLine(line.text, out string characterName, out string characterDialogue);

        GetCurrentDialogueText(characterName);

        currentDialogueText.gameObject.SetActive(true);

        if (letterDelay > 0.0f)
        {
            // Display the line one character at a time
            var stringBuilder = new StringBuilder();

            localDelay = letterDelay;
			
			for(int index = 0; index < characterDialogue.Length; index++) {
				
				char c = characterDialogue[index];
				
				if(c.Equals(TAG_SEPARATOR_INIT)){
					string remainingTextWithStart = characterDialogue.Substring(index);
					
					int indexOfSeparatorStartEnd = remainingTextWithStart.IndexOf(TAG_SEPARATOR_END);
					string richTextOption_init = remainingTextWithStart.Substring(
						0,
						indexOfSeparatorStartEnd + 1);
					
					string remainingText = remainingTextWithStart.Substring(indexOfSeparatorStartEnd + 1);
					
					int indexOfSeparatorFinishInit = remainingText.IndexOf(TAG_SEPARATOR_INIT);
					
					string richTextOption_word = remainingText.Substring(
						0,
						indexOfSeparatorFinishInit);
					
					string remainingTextWithEnd = remainingText.Substring(indexOfSeparatorFinishInit);
					
					int indexOfSeparatorFinishEnd = remainingTextWithEnd.IndexOf(TAG_SEPARATOR_END);
					string richTextOption_end = remainingTextWithEnd.Substring(
						0,
						indexOfSeparatorFinishEnd + 1);
						
					stringBuilder.Append(richTextOption_init + richTextOption_end);
					for(int indexWord = 0; indexWord < richTextOption_word.Length; indexWord++){
						char w = richTextOption_word[indexWord];
						
						stringBuilder.Insert(stringBuilder.Length - richTextOption_end.Length, w);
						currentDialogueText.text = stringBuilder.ToString();
						yield return new WaitForSeconds(localDelay);
					}
					
					index += richTextOption_init.Length + richTextOption_word.Length + richTextOption_end.Length - 1;
					continue;
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