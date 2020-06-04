using System.Collections;
using UnityEngine;

namespace Scripts.Level.NPC
{
    public class AuricularesInfo : NPCInfo
    {
        private readonly string[] DiscursoTriggers = new string[]
        {
            "" // TODO
        };
        private readonly string[] MusicTriggers = new string[]
        {
            ""
        };

        private const float AnimationDelay = 0.5f;

        public override void StartAnimation(string animation)
        {
            switch(animation)
            {
                case "Discurso":
                    StartCoroutine(DoDiscurso());
                    break;
                case "Musica":
                    SetRandomTrigger(MusicTriggers);
                    break;
                default:
                    break;
            }
        }

        public override void StopAnimation()
        {
            StopAllCoroutines();
            base.StopAnimation();
        }

        private IEnumerator DoDiscurso()
        {
            while(true)
            {
                SetRandomTrigger(DiscursoTriggers);

                yield return new WaitForSeconds(AnimationDelay);
            }
        }
    }
}