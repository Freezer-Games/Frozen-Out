using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Level.Item
{
    public class StickItem : ItemUser
    {   
        [SerializeField] float DropForce;
        public Rigidbody Rigidbody;
        public Collider Collider;
        public MeshRenderer Renderer;
        public Transform Player;

        public UnityEvent PlayerNormal;

        void Start() 
        {
            Renderer.enabled = false;
            Collider.enabled = false;
            Rigidbody.useGravity = false;
            Rigidbody.isKinematic = true;

            if (PlayerNormal == null)
            {
                PlayerNormal = new UnityEvent();
            }
        }

        public override void OnPlayerAway()
        {
            SeparateFromPlayer();
        }

        public override void OnPlayerClose()
        {
            
        }

        public override void OnPlayerExitCol()
        {
            
        }

        public override void OnPlayerCol()
        {
            //Se ilumina o algo
        }

        public override void OnUse()
        {
            StartCoroutine(WaitingForPlayer());
        }

        void SeparateFromPlayer()
        {
            Debug.Log("Separando de jugador");
            Collider.isTrigger = false;
            Rigidbody.useGravity = true;
            Rigidbody.isKinematic = false;

            Rigidbody.AddForce(-Vector3.forward * DropForce, ForceMode.Force);
        }

        public void Melting()
        {
            transform.SetParent(null);
            Collider.enabled = true;
            Collider.isTrigger = true;
            Renderer.enabled = true;
        }

        public void Recovery() 
        {
            Renderer.enabled = false;
            Rigidbody.useGravity = false;
            Rigidbody.isKinematic = true;
            Collider.enabled = false;
            Collider.isTrigger = true;

            transform.SetParent(Player);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            PlayerNormal.Invoke();
        }

        IEnumerator WaitingForPlayer()
        {
            while (Vector3.Distance(GetInteractionPoint().position, Player.position) > 0.01)
            {
                //No hace nada
                yield return null;
            }
            Recovery();
            
            yield return null;
        }
    }
}
