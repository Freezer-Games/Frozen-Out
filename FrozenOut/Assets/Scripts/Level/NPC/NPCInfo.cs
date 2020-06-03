using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level.Dialogue;
using System;

namespace Scripts.Level.NPC
{
    [RequireComponent(typeof(Animator))]
    public class NPCInfo : MonoBehaviour
    {
        public string Name;

        protected Animator Animator;

        void Start()
        {
            Animator = GetComponent<Animator>();
        }

        public virtual void StartAnimation(string animationTrigger)
        {
            Animator.SetTrigger(animationTrigger);
        }

        public virtual void StopAnimation()
        {
            Animator.enabled = false;
        }
    }
}