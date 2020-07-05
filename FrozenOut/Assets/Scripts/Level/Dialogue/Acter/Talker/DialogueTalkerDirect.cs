using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    [RequireComponent(typeof(DialogueIndicator))]
    public class DialogueTalkerDirect : DialogueTalker
    {
        public bool FacePlayer = false;

        protected DialogueIndicator Indicator;

        private Quaternion InitialRotation;
        private const float RotationSpeed = 1.0f;

        void Start()
        {
            InitialRotation = transform.rotation;
            Indicator = GetComponent<DialogueIndicator>();
        }

        public override void OnStartTalk()
        {
            Indicator.HideIndicator();

            if (FacePlayer)
            {
                RotateFacePlayer();
            }
        }

        public override void OnEndTalk()
        {
            Indicator.ShowIndicator();

            if (FacePlayer)
            {
                RotateFaceBack();
            }
        }

        public override void OnPlayerAway()
        {
            Indicator.HideIndicator();
        }

        public override void OnSelected()
        {
            Indicator.ShowIndicator();
        }

        public override void OnDeselected()
        {
            Indicator.HideIndicator();
        }

        protected void RotateFacePlayer()
        {
            StopAllCoroutines();

            Vector3 playerPosition = LevelManager.GetPlayerManager().Player.transform.position;
            Quaternion toPlayerRotation = Quaternion.LookRotation(playerPosition - transform.position);
            StartCoroutine(DoRotateTowards(toPlayerRotation));
        }

        protected void RotateFaceBack()
        {
            StopAllCoroutines();

            StartCoroutine(DoRotateTowards(InitialRotation));
        }

        private IEnumerator DoRotateTowards(Quaternion rotation)
        {
            rotation.x = 0.0f;
            rotation.z = 0.0f;

            while (transform.rotation != rotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}