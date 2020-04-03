using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public class DialogueTalker : MonoBehaviour
    {

        public string talkToNode = "";

    }
}
/*
        private LevelManager LevelManager => GameManager.Instance.CurrentLevelManager;

        private IDialogueManager DialogueManager => LevelManager.GetDialogueManager();
        private Inventory Inventory => LevelManager.GetInventory();
        private PlayerManager PlayerManager => LevelManager.GetPlayerManager();

        public Text promptText;

		private NPCTalker target;

        private static readonly Tag tagNegrita = new Tag("b", TagFormat.RichTextTagFormat);

        void Start()
        {
            
        }

        private void OnTriggerStay(Collider other)
        {
            if (/*this.isActiveAndEnabled &&*//* !DialogueManager.IsRunning && PlayerManager.IsGrounded)
            {
                if(other.gameObject.CompareTag("NPC"))
                {
					target = other.gameObject.GetComponent<NPCTalker>();
					
                    if(Input.GetKey(DialogueManager.GetInteractKey())){
                        if (target != null)
                        {
							target.HideIndicator();
                            Dial.StartDialogue(target.talkToNode);
                        }
                    }
                    else
                    {
                        if (target != null)
                        {

							target.ShowIndicator();
						}
                    }
                }
                else if(other.gameObject.CompareTag("Item"))
                {
                    if(Input.GetKeyDown(GameManager.instance.interact))
                    {
                        ClearPromptText();
                        inventory.GetItem(other.gameObject);
                    }
                    else
                    {
                        ItemInfo itemInfo = other.gameObject.GetComponent<ItemInfo>();
                        promptText.text = LocalizationManager.instance.GetLocalizedValue("Press") + PlayerPrefs.GetString("InteractKey", "F") + LocalizationManager.instance.GetLocalizedValue("take") + " " + new DialogueTaggedText(tagNegrita, itemInfo.itemName).FullText;
                    }
                }
            }
            else
            {
                ClearPromptText();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("NPC") || other.gameObject.CompareTag("Item"))
            {
				target.HideIndicator();
                ClearPromptText();
            }
        }

        private void ClearPromptText()
        {
            promptText.text = "";
        }
    }
}*/