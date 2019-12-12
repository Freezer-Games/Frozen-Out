using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

using Assets.Scripts.Item;
using Assets.Scripts.Dialogue.Texts;
using Assets.Scripts.Dialogue.Texts.Tags;

namespace Assets.Scripts.Dialogue
{
    public class PlayerYarn : MonoBehaviour
    {
        public Text promptText;

        private DialogueRunner dialogueSystemYarn;
        private PlayerController playerController;
        private Inventory inventory;

        private static readonly Tag tagNegrita = new Tag("b", TagFormat.RichTextTagFormat);

        /// Draw the range at which we'll start talking to people.
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
        }

        void Start()
        {
            dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
            inventory = FindObjectOfType<Inventory>();
            promptText.text = "";

            playerController = FindObjectOfType<PlayerController>();
            playerController.Moving += Player_Moving;
        }

        private void Player_Moving(object sender, PlayerControllerEventArgs e)
        {
            if (dialogueSystemYarn.isDialogueRunning || dialogueSystemYarn.isDialogueWaiting)
            {
                e.Cancel = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (this.isActiveAndEnabled && !dialogueSystemYarn.isDialogueRunning && playerController.IsGrounded)
            {
                if(other.gameObject.CompareTag("NPC"))
                {
                    if(Input.GetKey(GameManager.instance.interact)){
                        ClearPromptText();
                        NPCYarn target = other.gameObject.GetComponent<NPCYarn>();
                        if (target != null)
                        {
                            dialogueSystemYarn.StartDialogue(target.talkToNode);
                        }
                    }
                    else
                    {
                        promptText.text = LocalizationManager.instance.GetLocalizedValue("Press") + PlayerPrefs.GetString("InteractKey", "F") + LocalizationManager.instance.GetLocalizedValue("speak");
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
                ClearPromptText();
            }
        }

        private void ClearPromptText()
        {
            promptText.text = "";
        }
    }
}
