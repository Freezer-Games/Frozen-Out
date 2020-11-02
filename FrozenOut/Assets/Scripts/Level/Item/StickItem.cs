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
            base.OnPlayerAway();
            SeparateFromPlayer();
        }

        public override void OnPlayerExitCol()
        {
            
        }

        public override void OnPlayerCol()
        {
            Debug.Log("esto es el palo");
        }

        public override void OnUse()
        {
            Recovery();
        }

        public override void OnUnableUse()
        {

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
    }
}
