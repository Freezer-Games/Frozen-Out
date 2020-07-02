using System.Collections;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    [Serializable]
    public class CharacterDialogueStyle
    {
        public string Name;
        public DialogueStyle Style;
    }

    [Serializable]
    public class DialogueStyle
    {
        [Tooltip("Afecta velocidad de texto y de habla\nPositivo para que vaya más lento\nNegativo para que vaya más rápido")]
        [Range(-1.0f, 1.0f)]
        public float RelativeDelay;
        [NonSerialized]
        public float Delay;

        [Tooltip("Afecta tamaño de texto y volumen de habla\nPositivo para que sea más grande\nNegativo para que sea más pequeño")]
        [Range(-1.0f, 1.0f)]
        public float RelativeIntensity;

        public TextStyle TextStyle;
        public VoiceStyle VoiceStyle;

        public void NormaliseDelay(float defaultDelay, float minDelay, float maxDelay)
        {
            float normalisedDelay = NormaliseRelative(this.RelativeDelay, defaultDelay, minDelay, maxDelay);

            this.Delay = normalisedDelay;
            this.VoiceStyle.Delay = normalisedDelay;
        }

        public void NormaliseVolume(float defaultVolume, float minVolume, float maxVolume)
        {
            float normalisedVolume = NormaliseRelative(this.RelativeIntensity, defaultVolume, minVolume, maxVolume);

            this.VoiceStyle.Volume = normalisedVolume;
        }

        public void NormaliseSize(int defaultSize, int minSize, int maxSize)
        {
            int normalisedSize = (int) NormaliseRelative(this.RelativeIntensity, defaultSize, minSize, maxSize);

            this.TextStyle.Size = normalisedSize;
        }

        public void NormalisePitch(float defaultPitch, float minPitch, float maxPitch)
        {
            float normalisedPitch = NormaliseRelative(this.VoiceStyle.RelativePitch, defaultPitch, minPitch, maxPitch);

            this.VoiceStyle.Pitch = normalisedPitch;
        }

        private static float NormaliseRelative(float relativeValue, float defaultValue, float minValue, float maxValue)
        {
            float percentage = 1.0f + relativeValue;
            float rawValue = defaultValue * percentage;

            float clampedVolume = Mathf.Clamp(rawValue, minValue, maxValue);

            return clampedVolume;
        }
    }

    [Serializable]
    public class TextStyle
    {
        [Tooltip("Afecta a la fuente del texto\nSi se deja vacío se usara la fuente por defecto")]
        public Font Font;
        [Tooltip("Afecta al color del texto")]
        public Color Colour;
        [NonSerialized]
        public int Size;

        [Header("Efectos")]
        public TextEffect Effect;

        [Serializable]
        public enum TextEffect
        {
            None,
            Jumping,
            Fading,
            Highlighted,
            Interrupted
        }

        public void UpdateOptionals(TextStyle defaultStyle)
        {
            if (this.Font == null)
            {
                this.Font = defaultStyle.Font;
            }
        }
    }

    [Serializable]
    public class VoiceStyle
    {
        [Range(-1.0f, 1.0f)]
        public float RelativePitch;
        [NonSerialized]
        public float Pitch;
        [Range(-1.0f, 1.0f)]
        public float RelativeGender;
        [NonSerialized]
        public float Gender;
        [Range(-1.0f, 1.0f)]
        public float RelativeTone;
        [NonSerialized]
        public float Tone;

        [NonSerialized]
        public float Volume;
        [NonSerialized]
        public float Delay;
        
        [Header("Efectos")]
        public VoiceEffect Effect;

        [Serializable]
        public enum VoiceEffect
        {
            None,
            Radio
        }

        public void UpdateOptionals(VoiceStyle defaultStyle)
        {

        }
    }
}