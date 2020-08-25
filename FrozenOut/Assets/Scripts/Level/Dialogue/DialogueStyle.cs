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

        public void UpdateText(TextStyleConfiguration textConfiguration)
        {
            TextStyle.Delay = InterpolateRelative(RelativeDelay, textConfiguration.DelayConfiguration);
            TextStyle.Size = (int) InterpolateRelative(RelativeIntensity, textConfiguration.SizeConfiguration);
        }

        public void UpdateVoice(VoiceStyleConfiguration voiceConfiguration)
        {
            VoiceStyle.Delay = InterpolateRelative(RelativeDelay, voiceConfiguration.DelayConfiguration);
            VoiceStyle.Volume = InterpolateRelative(RelativeIntensity, voiceConfiguration.VolumeConfiguration);

            VoiceStyle.Pitch = InterpolateRelative(VoiceStyle.RelativePitch, voiceConfiguration.PitchConfiguration);
        }

        private static float InterpolateRelative(float relativeValue, StyleConfiguration configuration)
        {
            return InterpolateRelative(relativeValue, configuration.Default, configuration.Minimum, configuration.Maximum);
        }

        public static float InterpolateRelative(float relativeValue, float defaultValue, float minValue, float maxValue)
        {
            float finalValue = defaultValue;

            if(relativeValue < 0)
            {
                finalValue = Mathf.Lerp(minValue, defaultValue, (1+relativeValue));
            }
            else if(relativeValue > 0)
            {
                finalValue = Mathf.Lerp(defaultValue, maxValue, relativeValue);
            }

            return finalValue;
        }
    }

    [Serializable]
    public class TextStyle
    {
        [Tooltip("Afecta a la fuente del texto")]
        public Font Font;
        [Tooltip("Afecta al color del texto")]
        public Color Colour;

        [NonSerialized]
        public float Delay;
        [NonSerialized]
        public int Size;

        [Header("Efects")]
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
    }

    [Serializable]
    public class VoiceStyle
    {
        [Range(-1.0f, 1.0f)]
        public float RelativePitch;
        [NonSerialized]
        public float Pitch;

        [NonSerialized]
        public float Delay;
        [NonSerialized]
        public float Volume;

        [Header("Efects")]
        public VoiceEffect Effect;

        [Serializable]
        public enum VoiceEffect
        {
            None,
            Radio
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
        public StyleConfiguration VolumeConfiguration;

        public StyleConfiguration PitchConfiguration;
    }
}