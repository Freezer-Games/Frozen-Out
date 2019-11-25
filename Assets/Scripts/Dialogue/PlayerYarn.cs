using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

using Assets.Scripts.Item;

namespace Assets.Scripts.Dialogue
{
    public class PlayerYarn : MonoBehaviour
    {
        public string dialogueInput = "Use";
        public string itemInput = "Use";
        public Text promptText;

        private DialogueRunner dialogueSystemYarn;
        private Inventory inventory;

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

            PlayerController controller = FindObjectOfType<PlayerController>();
            controller.Moving += Player_Moving;
        }

        private void Player_Moving(object sender, PlayerControllerEventArgs e)
        {
            if (dialogueSystemYarn.isDialogueRunning) e.Cancel = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (this.isActiveAndEnabled && !dialogueSystemYarn.isDialogueRunning)
            {
                if(other.gameObject.tag == "NPC")
                {
                    if(Input.GetButtonDown(dialogueInput)){
                        promptText.text = "";
                        NPCYarn target = other.gameObject.GetComponent<NPCYarn>();
                        if (target != null)
                        {
                            dialogueSystemYarn.StartDialogue(target.talkToNode);
                        }
                    }
                    else
                    {
                        if (PlayerPrefs.GetString("Language")=="Es") { promptText.text = "Pulsa [" + "F" + "] para hablar"; }
                        else { promptText.text = "Press [" + "F" + "] to talk"; }
                    }
                }
                else if(other.gameObject.tag == "Item")
                {
                    if(Input.GetButtonDown(itemInput))
                    {
                        promptText.text = "";
                        inventory.GetItem(other.gameObject);
                    }
                    else
                    {
                        ItemInfo itemInfo = other.gameObject.GetComponent<ItemInfo>();
                        if (PlayerPrefs.GetString("Language") == "Es") { promptText.text = "Pulsa [" + "F" + "] para coger <b>" + itemInfo.itemName + "</b>"; }
                        else { promptText.text = "Press [" + "F" + "] to get <b>" + itemInfo.itemName + "</b>"; }
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "NPC" || other.gameObject.tag == "Item")
            {
                promptText.text = "";
            }
        }
    }
}
