using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public class DialogueIndicator : MonoBehaviour
    {

        public GameObject PrefabIndicator;
		public float IndicatorHeightOffset = 0.0f;

        private GameObject Indicator;
        
		void Start()
		{
			Indicator = CreateIndicator();
			HideIndicator();
		}

		void Update()
		{
        	Indicator.transform.rotation = Quaternion.LookRotation(transform.position - UnityEngine.Camera.main.transform.position);
		}
        
        public void HideIndicator()
		{
			SetIndicator(false);
		}
		
		public void ShowIndicator()
		{
			SetIndicator(true);
		}

		private GameObject CreateIndicator()
		{
			if(PrefabIndicator != null){
				GameObject prefabInstance = GameObject.Instantiate(PrefabIndicator, transform);
				prefabInstance.transform.position += new Vector3(0, IndicatorHeightOffset, 0);
				return prefabInstance;
			}
			
			return null;
		}
		
		private void SetIndicator(bool active)
		{
			if (Indicator != null)
            {
                Indicator.SetActive(active);
            }
		}
    }
}