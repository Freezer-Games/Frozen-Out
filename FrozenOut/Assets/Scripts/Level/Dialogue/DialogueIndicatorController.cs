using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Dialogue
{
    public class DialogueIndicatorController : MonoBehaviour
	{
		public Image IndicatorImage;

		public Color normalColour;
		public Color selectedColour;

		public void Highlight()
		{
			IndicatorImage.color = selectedColour;
		}

		public void Unhighlight()
		{
			IndicatorImage.color = normalColour;
		}
    }
}