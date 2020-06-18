using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    [RequireComponent(typeof(DialogueIndicator))]
    public class DialogueTalker : DialogueActer
    {
        protected DialogueIndicator Indicator;
        private Quaternion InitialRotation;
        private const float RotationSpeed = 1.0f;

        protected ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;

        void Start()
        {
            Indicator = GetComponent<DialogueIndicator>();
            SetBlocking();
            SetNonAutomatic();

            InitialRotation = transform.rotation;
        }

        public override void OnStartTalk()
        {
            Indicator.HideIndicator();

            StopAllCoroutines();

            Vector3 playerPosition = LevelManager.GetPlayerManager().Player.transform.position;
            Quaternion toPlayerRotation = Quaternion.LookRotation(playerPosition - transform.position);
            StartCoroutine(RotateTowards(toPlayerRotation));
        }

        public override void OnEndTalk()
        {
            Indicator.ShowIndicator();

            StopAllCoroutines();
            StartCoroutine(RotateTowards(InitialRotation));
        }

        public override void OnPlayerClose()
        {
            //Indicator.ShowIndicator();
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

        private IEnumerator RotateTowards(Quaternion rotation)
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