using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Level.NPC
{
    public abstract class NPCInfo : MonoBehaviour
    {
        public string Name;

        public Animator Animator;

        public abstract void StartAnimation(string animation);

        public virtual void StopAnimation()
        {
            Animator.enabled = false;
        }

        protected T RandomElement<T>(ICollection<T> collection)
        {
            T randomElement;
            if(collection.Count() > 1)
            {
                int randomIndex = Random.Range(0, collection.Count());
                randomElement = collection.ElementAt(randomIndex);
            }
            else
            {
                randomElement = collection.FirstOrDefault();
            }

            return randomElement;
        }

        protected void SetRandomTrigger(ICollection<string> triggers)
        {
            string triggerName = RandomElement(triggers);

            Animator.SetTrigger(triggerName);
        }
        
        protected void SetRandomBool(ICollection<string> bools)
        {
            string boolName = RandomElement(bools);

            Animator.SetBool(boolName, true);
        }
    }
}