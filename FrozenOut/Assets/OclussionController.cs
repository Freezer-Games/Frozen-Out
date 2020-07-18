using Scripts.Level.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Scripts.Level.Animation
{
    public class OclussionController : MonoBehaviour
    {
        public LevelManager LevelManager;
        private PlayerManager PlayerManager => LevelManager.GetPlayerManager();

        public PostProcessVolume Volume;

        private AmbientOcclusion Oclussion;
        private ColorGrading Grading;

        void Start()
        {
            Oclussion = Volume.profile.GetSetting<AmbientOcclusion>();
            Grading = Volume.profile.GetSetting<ColorGrading>();

            PlayerManager.NormalOclussion += (sender, args) => NormalOclussion();
            PlayerManager.OrangeOclussion += (sender, args) => OrangeOclussion();
        }

        private void NormalOclussion()
        {
            Oclussion.color.value = new Color(18/255, 49/255, 118/255);
            Grading.temperature.value = -10;
            Debug.Log("normal");
        }

        private void OrangeOclussion()
        {
            Oclussion.color.value = new Color(118/255, 104/255, 18/255);
            Grading.temperature.value = 40;
            Debug.Log("naranja");
        }
    }
}