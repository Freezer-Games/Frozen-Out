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

    private AudioSource audioSource;
    private float localDelay;

    private string LINE_SEPARATOR = ": ";
    private string MAIN_NAME = "Pol";

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

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

    void Update() {
        if(Input.anyKeyDown) { localDelay = localDelay / 2; }
    }

    public override IEnumerator RunLine(Yarn.Line line) {

        string characterName;
        string characterDialogue;
        SeparateLine(line.text, out characterName, out characterDialogue);
        
        Text currentNameText = mainNameText;
        Text currentDialogueText = mainDialogueText;
        if(characterName != MAIN_NAME)
        {
            currentNameText = otherNameText;
            currentDialogueText = otherDialogueText;
        }

        currentNameText.text = characterName;
        currentDialogueText.gameObject.SetActive (true);

        if (letterDelay > 0.0f) {
            // Display the line one character at a time
            var stringBuilder = new StringBuilder ();

            localDelay = letterDelay;

            foreach (char c in characterDialogue) {
                stringBuilder.Append (c);
                currentDialogueText.text = stringBuilder.ToString ();
                yield return new WaitForSeconds (localDelay);
            }
        }
        currentDialogueText.text = characterDialogue;

        // Show the 'press any key' prompt when done, if we have one
        if (continuePrompt != null) {
            continuePrompt.gameObject.SetActive (true);
        }

        while (Input.anyKeyDown == false) {
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        currentDialogueText.gameObject.SetActive (false);

        if (continuePrompt != null){
            continuePrompt.gameObject.SetActive (false);
        }
    }

    public override IEnumerator RunOptions(Yarn.Options optionsCollection, Yarn.OptionChooser optionChooser) {
        yield return null;
    }

    public override IEnumerator RunCommand (Yarn.Command command)
    {
        yield break;
    }

    public override IEnumerator DialogueStarted ()
    {
        // Enable the dialogue controls.
        if (dialogueBoxGUI != null) {
            dialogueBoxGUI.SetActive(true);
        }
        
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