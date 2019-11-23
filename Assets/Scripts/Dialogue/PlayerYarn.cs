using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

using Assets.Scripts.Item;

namespace Assets.Scripts.Dialogue
{
    public class PlayerYarn : MonoBehaviour
    {
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "NPC")
            {
                promptText.text = "Press [" + "F" + "] to talk";
            }
            else if(other.gameObject.tag == "Item")
            {
                ItemInfo itemInfo = other.gameObject.GetComponent<ItemInfo>();
                promptText.text = "Press [" + "F" + "] to get <b>" + itemInfo.itemName + "</b>";
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (this.isActiveAndEnabled && !dialogueSystemYarn.isDialogueRunning)
            {
                if(other.gameObject.tag == "NPC" && Input.GetButtonDown("Use"))
                {
                    promptText.text = "";
                    NPCYarn target = other.gameObject.GetComponent<NPCYarn>();
                    if (target != null)
                    {
                        dialogueSystemYarn.StartDialogue(target.talkToNode);
                    }
                }
                else if(other.gameObject.tag == "Item" && Input.GetButtonDown("Use"))
                {
                    promptText.text = "";
                    inventory.GetItem(other.gameObject);
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
