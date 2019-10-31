using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Yarn.Unity;

[System.Serializable]
public class NPCYarn : MonoBehaviour {

    public string characterName = "";
    public string talkToNode = "";

    [Header("Optional")]
    public TextAsset scriptToLoad;

    private DialogueRunner dialogSystemYarn;

    void Start () {
        dialogSystemYarn = FindObjectOfType<DialogueRunner>();

        if (scriptToLoad != null) {
            dialogSystemYarn.AddScript(scriptToLoad);
        }
    }
}

