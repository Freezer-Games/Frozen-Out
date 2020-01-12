using System;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue
{
    [System.Serializable]
    public class NPCYarn : MonoBehaviour
    {

        public string characterName = "";
        public string talkToNode = "";
		
		public GameObject prefabIndicator;
		
		private Animator npcAnimator;
		private GameObject indicator;
		
		void Start()
		{
			npcAnimator = GetComponent<Animator>();
			indicator = CreateIndicator();
			
			if(characterName.Equals("")) {
				characterName = talkToNode;
			}
			
			DialogueRunner dialogueSystem = FindObjectOfType<DialogueRunner>();
            dialogueSystem.Started += HideIndicatorWhenStarted;
			dialogueSystem.Ended += ShowIndicatorWhenEnded;
		}
		
		GameObject CreateIndicator() {
			
			return GameObject.Instantiate(prefabIndicator, transform);
			
		}
		
		[YarnCommand("setanim")]
		public void TriggerAnimation(string animationName) {
			
			if(npcAnimator != null) {				
				animationName = "Anim_" + animationName;
			
				npcAnimator.SetTrigger(animationName);
			}
			
		}
		
		private void HideIndicatorWhenStarted(object sender, DialogueEventArgs e)
        {
            if (indicator != null)
            {
                indicator.SetActive(false);
            }
        }
		
		private void ShowIndicatorWhenEnded(object sender, EventArgs e)
        {
            if (indicator != null)
            {
                indicator.SetActive(true);
            }
        }
		
    }
}

