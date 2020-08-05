using UnityEngine;
using UnityEngine.Audio;

using Microsoft.CognitiveServices.Speech;

namespace Scripts.Level.Dialogue.Voice.Azure
{
    public class AzureTextToSpeech : VoiceManager
    {
        public AudioSource AudioSource;

        public AudioMixerGroup RadioMixerGroup;

        private SpeechConfig SpeechConfig;
        private SpeechSynthesizer Synthesizer;

        private const string ServiceRegion = "westeurope";
        private const string SubscriptionKey = "84c068692c8d4dafababeefa3fa4e4f1";

        private void Start()
        {
            SpeechConfig = SpeechConfig.FromSubscription(SubscriptionKey, ServiceRegion);
            SpeechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw16Khz16BitMonoPcm);

            Synthesizer = new SpeechSynthesizer(SpeechConfig, null);
        }

        public override void Open()
        {

        }

        public override void Close()
        {
            //TODO
        }

        public override void StartLine()
        {
            //TODO
        }

        public override void SetStyle(VoiceStyle style)
        {
            //TODO
        }

        public override void SpeakDialogueAccumulated(string dialogue)
        {
            /**
             * Adapted sample code from Microsoft
             * <https://docs.microsoft.com/en-gb/azure/cognitive-services/speech-service/quickstarts/text-to-speech?tabs=unity%2Clinux%2Cjre%2Cwindowsinstall&pivots=programming-language-csharp>
             */
            using (SpeechSynthesisResult result = Synthesizer.SpeakTextAsync(dialogue).Result)
            {
                // Checks result.
                if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                {
                    // Native playback is not supported on Unity yet (currently only supported on Windows/Linux Desktop).
                    // Use the Unity API to play audio here as a short term solution.
                    // Native playback support will be added in the future release.
                    int sampleCount = result.AudioData.Length / 2;
                    float[] audioData = new float[sampleCount];
                    for (int i = 0; i < sampleCount; ++i)
                    {
                        audioData[i] = (short)(result.AudioData[i * 2 + 1] << 8 | result.AudioData[i * 2]) / 32768.0F;
                    }

                    // The output audio format is 16K 16bit mono
                    var audioClip = AudioClip.Create("SynthesizedAudio", sampleCount, 1, 16000, false);
                    audioClip.SetData(audioData, 0);
                    AudioSource.clip = audioClip;
                    AudioSource.Play();
                }
            }
        }

        public override void SpeakDialogueSingle(string dialogueLetter)
        {
            throw new global::System.NotSupportedException();
        }
    }
}