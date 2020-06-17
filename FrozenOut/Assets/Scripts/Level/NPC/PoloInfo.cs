using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.NPC
{
    public class PoloInfo : NPCInfo
    {
        protected readonly string[] AwakenTriggers = new string[]
        {
            "Anim_Awaken_1",
            "Anim_Awaken_2",
            "Anim_Awaken_3"
        };
        protected readonly string[] DenyTriggers = new string[]
        {
            "Anim_Deny_1",
            "Anim_Deny_2",
            "Anim_Deny_3"
        };
        protected readonly string[] NodTriggers = new string[]
        {
            "Anim_Nod_1",
            "Anim_Nod_2"
        };
        protected readonly string[] WorkTriggers = new string[]
        {
            "Anim_Mining"
        };
        protected readonly string[] SneakTriggers = new string[]
        {
            "Anim_Sneaking"
        };
        protected readonly string[] PalancaTriggers = new string[]
        {
            "Anim_Palanca"
        };
        protected readonly string DanceBool = "Anim_Dance";
        protected readonly string TiredBool = "Anim_Tired";

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
                case "Work":
                    SetRandomTrigger(WorkTriggers);
                    break;
                case "Sneak":
                    SetRandomTrigger(SneakTriggers);
                    break;
                case "Dance":
                    SetBool(DanceBool, true);
                    break;
                case "Tired":
                    SetBool(TiredBool, true);
                    break;
                default:
                    break;
            }
        }

        public override void StopAnimation()
        {
            StopAllCoroutines();
        }
    }
}