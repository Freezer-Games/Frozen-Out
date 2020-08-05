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

        [Tooltip("Afecta tamaño de texto y volumen de habla\nPositivo para que sea más grande\nNegativo para que sea más pequeño")]
        [Range(-1.0f, 1.0f)]
        public float RelativeIntensity;

        public TextStyle TextStyle;
        public VoiceStyle VoiceStyle;

        public void NormaliseText(TextStyleConfiguration textConfiguration)
        {
            TextStyle.Delay = NormaliseRelative(RelativeDelay, textConfiguration.DelayConfiguration);
            TextStyle.Size = (int) NormaliseRelative(RelativeIntensity, textConfiguration.SizeConfiguration);
        }

        public void NormaliseVoice(VoiceStyleConfiguration voiceConfiguration)
        {
            VoiceStyle.Delay = NormaliseRelative(RelativeDelay, voiceConfiguration.DelayConfiguration);
            VoiceStyle.Volume = NormaliseRelative(RelativeIntensity, voiceConfiguration.VolumeConfiguration);

            VoiceStyle.Pitch = NormaliseRelative(VoiceStyle.RelativePitch, voiceConfiguration.PitchConfiguration);
        }

        private static float NormaliseRelative(float relativeValue, StyleConfiguration configuration)
        {
            return NormaliseRelative(relativeValue, configuration.Default, configuration.Minimum, configuration.Maximum);
        }

        public static float NormaliseRelative(float relativeValue, float defaultValue, float minValue, float maxValue)
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
        [NonSerialized]
        public float Delay;

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

    [Serializable]
    public class StyleConfiguration
    {
        public float Default;
        public float Minimum;
        public float Maximum;
    }

    [Serializable]
    public class TextStyleConfiguration
    {
        public StyleConfiguration DelayConfiguration;

        public StyleConfiguration SizeConfiguration;
    }

    [Serializable]
    public class VoiceStyleConfiguration
    {
        public StyleConfiguration DelayConfiguration;

        public StyleConfiguration PitchConfiguration;
        public StyleConfiguration VolumeConfiguration;
    }
}