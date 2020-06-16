namespace Scripts.Level.NPC
{
    public class ConosInfo : NPCInfo
    {
        private readonly string[] SurpriseTriggers = new string[]
        {
            "Anim_Surprise"
        };
        private readonly string[] LaughTriggers = new string[]
        {
            "Anim_Laugh"
        };
        private readonly string[] BadassTriggers = new string[]
        {
            "Anim_Baddass"
        };
        private readonly string[] DenyTriggers = new string[]
        {
            "Anim_Denial"
        };
        private readonly string[] BuriedTriggers = new string[]
        {
            "Anim_Buried"
        };
        private readonly string WalkingBool = "IsWalking";
        private readonly string PrevailBool = "Anim_Prevail";

        public override void StartAnimation(string animation)
        {
            switch (animation)
            {
                case "Surprise":
                    SetRandomTrigger(SurpriseTriggers);
                    break;
                case "Laugh":
                    SetRandomTrigger(LaughTriggers);
                    break;
                case "Badass":
                    SetRandomTrigger(BadassTriggers);
                    break;
                case "Deny":
                    SetRandomTrigger(DenyTriggers);
                    break;
                case "Buried":
                    SetRandomTrigger(BuriedTriggers);
                    break;
                case "Walk":
                    SetBool(WalkingBool, true);
                    break;
                case "Prevail":
                    SetBool(PrevailBool, true);
                    break;
                default:
                    break;
            }
        }
    }
}