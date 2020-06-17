namespace Scripts.Level.NPC
{
    public class PalanquillaInfo : PoloInfo
    {
        private bool InPalanca = false;

        void Start()
        {
            StartAnimation("DenyIntervals");
        }

        public override void StartAnimation(string animation)
        {
            if (!InPalanca)
            {
                base.StartAnimation(animation);

                switch (animation)
                {
                    case "DenyIntervals":
                        DenyIntervals();
                        break;
                    case "Palanca":
                        InPalanca = true;
                        SetRandomTrigger(PalancaTriggers);
                        break;
                    default:
                        break;
                }
            }
        }

        private void DenyIntervals()
        {
            StartCoroutine(DoTriggerInterval(DenyTriggers, 3.0f, 6.0f));
        }
    }
}