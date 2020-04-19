using System.Collections;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    [Serializable]
    public class DialogueStyle : MonoBehaviour
    {
        [Tooltip("Positivo para que vaya más lento\nNegativo para que vaya más rápido")]
        [Range(-0.1f, 1.0f)]
        public float RelativeDelay;

        [Tooltip("Positivo para que sea más grande\nNegativo para que sea más pequeño")]
        [Range(-10.0f, 4.0f)]
        public float RelativeSize;

        [Tooltip("Positivo para que haya más espacio\nNegativo para que haya menos espacio")]
        public int Spacing;

        public Color Colour;
    }
}