using System.Collections;
using UnityEngine;

namespace Scripts.Level.NPC
{
    public class AuricularesInfo : NPCInfo
    {
        protected readonly string[] DiscursoTriggers = new string[]
        {
            "Discurso_1",
            "Discurso_2",
            "Discurso_3",
            "Discurso_4",
            "Discurso_5"
        };
        protected readonly string[] MusicTriggers = new string[]
        {
            "Musica_Der",
            "Musica_Izq"
        };
        private int lastMusicIndex = 0;

        private const float DiscursoDelay = 1.0f;

        public override void StartAnimation(string animation)
        {
            StopAllCoroutines();

            switch(animation)
            {
                case "Discurso":
                    StartCoroutine(DoDiscurso());
                    break;
                case "Musica":
                    lastMusicIndex = SetSequentialTrigger(MusicTriggers, lastMusicIndex);
                    break;
                default:
                    break;
            }
        }

        public override void StopAnimation()
        {
            StopAllCoroutines();
        }

        private IEnumerator DoDiscurso()
        {
            while(true)
            {
                SetRandomTrigger(DiscursoTriggers);

                yield return new WaitForSeconds(DiscursoDelay);
            }
        }
    }
}