using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class LeverLiftItem : ItemUser
    {
        [SerializeField] float animDelay;
        public GameObject Lever;
        public Animator LiftAnimator;

        public override void OnPlayerCol() {}

        public override void OnPlayerExitCol() {}

        public override void OnUse()
        {
            Lever.SetActive(true);
            LiftAnimator.SetTrigger("active");
            GetComponent<TriggerUseItem>().enabled = false;
        }
    }
}

