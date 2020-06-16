using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    [RequireComponent(typeof(DialogueIndicator))]
    public class DialogueTalker : DialogueActer
    {
        private DialogueIndicator Indicator;
        private Vector3 InitialPosition;
        private const float RotationSpeed = 1.0f;

        protected ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;

        void Start()
        {
            Indicator = GetComponent<DialogueIndicator>();
            SetBlocking();
            SetNonAutomatic();

            InitialPosition = transform.position;
        }

        public override void OnStartTalk()
        {
            Indicator.HideIndicator();

            StopAllCoroutines();
            StartCoroutine(RotateTowards(LevelManager.GetPlayerManager().Player.transform.position));
        }

        public override void OnEndTalk()
        {
            Indicator.ShowIndicator();

            StopAllCoroutines();
            StartCoroutine(RotateTowards(InitialPosition));
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

        private IEnumerator RotateTowards(Vector3 playerPosition)
        {
            Quaternion playerRotation = Quaternion.LookRotation(playerPosition - transform.position);
            playerRotation.x = 0.0f;
            playerRotation.z = 0.0f;

            while (transform.rotation != playerRotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, RotationSpeed * Time.deltaTime);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}