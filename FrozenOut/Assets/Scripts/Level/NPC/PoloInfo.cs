using System.Collections;
using UnityEngine;

namespace Scripts.Level.NPC
{
    public class PoloInfo : NPCInfo
    {
        private readonly string[] AwakenTriggers = new string[]
        {
            "Anim_Awaken_1",
            "Anim_Awaken_2",
            "Anim_Awaken_3"
        };
        private readonly string[] DenyTriggers = new string[]
        {
            "Anim_Deny_1",
            "Anim_Deny_2",
            "Anim_Deny_3"
        };
        private readonly string[] NodTriggers = new string[]
        {
            "Anim_Nod_1",
            "Anim_Nod_2"
        };
        private readonly string DanceBool = "Anim_Dance";
        private readonly string TiredBool = "Anim_Tired";
        private readonly string[] WorkTriggers = new string[]
        {
            "Anim_Mining"
        };

        public override void StartAnimation(string animation)
        {
            StopAllCoroutines();

            switch (animation)
            {
                case "Awaken":
                    SetRandomTrigger(AwakenTriggers);
                    break;
                case "Deny":
                    SetRandomTrigger(DenyTriggers);
                    break;
                case "Nod":
                    SetRandomTrigger(NodTriggers);
                    break;
                case "Dance":
                    SetBool(DanceBool, true);
                    break;
                case "Tired":
                    SetBool(TiredBool, true);
                    break;
                case "Work":
                    StartCoroutine(DoWork());
                    break;
                default:
                    break;
            }
        }

        public override void StopAnimation()
        {
            StopAllCoroutines();
        }

        private IEnumerator DoWork()
        {
            yield return new WaitForSeconds(0.5f);
            while(true)
            {
                SetRandomTrigger(WorkTriggers);

                float randomDelay = Random.Range(0.5f, 2.0f);
                yield return new WaitForSeconds(randomDelay);
            }
        }
    }
}