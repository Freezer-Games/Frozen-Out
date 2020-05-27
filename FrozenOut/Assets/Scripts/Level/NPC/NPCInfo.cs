using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level.Dialogue;

namespace Scripts.Level.NPC
{
    [RequireComponent(typeof(Animator))]
    public class NPCInfo : MonoBehaviour
    {
        public string Name;

        private Animator Animator;

        void Start()
        {
            Animator = GetComponent<Animator>();
        }

        public void StartAnimation(string animationTrigger)
        {
            Animator.SetTrigger(animationTrigger);
        }    
    }
}