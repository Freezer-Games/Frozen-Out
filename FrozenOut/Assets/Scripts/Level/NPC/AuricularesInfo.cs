using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Scripts.Level.Dialogue;

namespace Scripts.Level.NPC
{
    [RequireComponent(typeof(Animator))]
    public class AuricularesInfo : NPCInfo
    {
        private readonly string[] DiscursoTriggers = new string[]
        {
            "", // TODO
        };
        private const string MusicTriggerName = "";

        private const float AnimationDelay = 0.5f;

        public override void StartAnimation(string animationTrigger)
        {
            if(animationTrigger.Equals("Discurso"))
            {
                StartCoroutine(DoDiscurso());
            }
            else if(animationTrigger.Equals("Música"))
            {
                Animator.SetTrigger(MusicTriggerName);
            }
        }

        public override void StopAnimation()
        {
            StopAllCoroutines();
            Animator.SetTrigger("Stop");
        }

        private IEnumerator DoDiscurso()
        {
            while(true)
            {
                int randomIndex = Random.Range(0, DiscursoTriggers.Count());
                string trigger = DiscursoTriggers.ElementAt(randomIndex);

                Animator.SetTrigger(trigger);

                yield return new WaitForSeconds(AnimationDelay);
            }
        }
    }
}