using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Dialogue
{
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class TextStyleController : MonoBehaviour
    {
        private UnityEngine.UI.Text Text;

        private void Start()
        {
            Text = GetComponent<UnityEngine.UI.Text> ();
        }

        public void SetStyle(TextStyle style)
        {
            Text.font = style.Font;
            Text.fontSize = style.Size;
            Text.color = style.Colour;
        }
    }
}