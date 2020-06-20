using Scripts.Level.Sound;
using System.Collections;
using UnityEngine;

namespace Scripts.Level.NPC
{
    public class PoloWorkerTiredInfo : PoloInfo
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
                    WorkTiredIntervals();
                    break;
                default:
                    break;
            }
        }

        private void WorkTiredIntervals()
        {
            StartCoroutine(DoWorkTiredInterval(0.5f, 2.0f));
        }

        private IEnumerator DoWorkTiredInterval(float minDelay, float maxDelay)
        {
            yield return new WaitForSeconds(maxDelay);

            while (true)
            {
                SetRandomTrigger(WorkTriggers);
                SoundController.PlayRandomClip(SoundController.Ores);

                float randomDelay = Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(randomDelay);

                SetBool(TiredBool, true);

                randomDelay = Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(randomDelay);
                SetBool(TiredBool, false);
            }
        }
    }
}