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
		
		[Header("Indicator")]
		public GameObject prefabIndicator;
		public float indicatorHeightOffset = 0.0f;
		
		private Animator npcAnimator;
		private GameObject indicator;
		
		void Start()
		{
			npcAnimator = GetComponent<Animator>();
			indicator = CreateIndicator();
			HideIndicator();
			
			if(characterName.Equals("")) {
				characterName = talkToNode;
			}
		}
		
		GameObject CreateIndicator()
		{
			if(prefabIndicator != null){
				GameObject prefabInstance = GameObject.Instantiate(prefabIndicator, transform);
				prefabInstance.transform.position += new Vector3(0, indicatorHeightOffset, 0);
				return prefabInstance;
			}
			
			return null;
		}
		
		[YarnCommand("setanim")]
		public void TriggerAnimation(string animationName)
		{
			
			if(npcAnimator != null) {				
				animationName = "Anim_" + animationName;
			
				npcAnimator.SetTrigger(animationName);
			}
			
		}
		
		private void SetIndicator(bool active)
		{
			if (indicator != null)
            {
                indicator.SetActive(active);
            }
		}
		
		public void HideIndicator()
		{
			SetIndicator(false);
		}
		
		public void ShowIndicator()
		{
			SetIndicator(true);
		}
		
    }
}

