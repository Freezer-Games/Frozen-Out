using System.Collections;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    [Serializable]
    public class DialogueStyle
    {
        [Tooltip("Positivo para que vaya más lento\nNegativo para que vaya más rápido")]
        [Range(-0.1f, 1.0f)]
        public float RelativeDelay;
        [NonSerialized]
        public float Delay;

        [Tooltip("Positivo para que sea más grande\nNegativo para que sea más pequeño")]
        [Range(-10, 4)]
        public int RelativeSize;
        [NonSerialized]
        public int Size;

        [Header("Optional")]
        [Tooltip("Si se deja vacío se usara el por defecto")]
        public Font Font;
        public Color Colour;

        public void UpdateDelay(float defaultDelay)
        {
            this.Delay = defaultDelay + this.RelativeDelay;
        }

        public void UpdateSize(int defaultSize)
        {
            this.Size = defaultSize + this.RelativeSize;
        }

        public void UpdateFont(Font defaultFont)
        {
            if(this.Font == null)
            {
                this.Font = defaultFont;
            }
        }
    }
}