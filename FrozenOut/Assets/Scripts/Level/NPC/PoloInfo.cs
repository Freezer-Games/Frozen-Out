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
        private readonly string[] DanceBools = new string[]
        {
            "Anim_Dance"
        };
        private readonly string[] TiredBools = new string[]
        {
            "Anim_Tired"
        };

        public override void StartAnimation(string animation)
        {
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
                    SetRandomBool(DanceBools);
                    break;
                case "Tired":
                    SetRandomBool(TiredBools);
                    break;
                default:
                    break;
            }
        }
    }
}