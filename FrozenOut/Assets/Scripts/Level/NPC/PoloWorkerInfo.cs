namespace Scripts.Level.NPC
{
    public class PoloWorkerInfo : PoloInfo
    {
        void Start()
        {
            StartAnimation("WorkIntervals");
        }

        public override void StartAnimation(string animation)
        {
            base.StartAnimation(animation);

            switch (animation)
            {
                case "WorkIntervals":
                    WorkIntervals();
                    break;
                default:
                    break;
            }
        }

        private void WorkIntervals()
        {
            StartCoroutine(DoTriggerInterval(WorkTriggers, 0.5f, 2.0f, () => {
                SoundController.PlayRandomClip(SoundController.Ores);
            }));
        }
    }
}