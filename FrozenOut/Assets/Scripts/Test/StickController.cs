using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    MeshCollider m_Collider;
    MeshRenderer m_Renderer;

    [SerializeField] float GoodByeForce;
    public Transform Player;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<MeshCollider>();
        m_Renderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        m_Renderer.enabled = false;
        m_Collider.enabled = false;
        m_Rigidbody.useGravity = false;
        m_Rigidbody.isKinematic = true;
    }


    void OnTriggerExit(Collider other)
    {
        m_Collider.isTrigger = false;
        m_Rigidbody.useGravity = true;
        m_Rigidbody.isKinematic = false;

        m_Rigidbody.AddForce(-Vector3.forward * GoodByeForce, ForceMode.Force);
    }

    public void Melting()
    {
        transform.SetParent(null);
        m_Collider.enabled = true;
        m_Collider.isTrigger = true;
        m_Renderer.enabled = true;
        Debug.Log("no formo parte de pol");
    }

    public void Recovery()
    {
        m_Renderer.enabled = false;
        m_Rigidbody.useGravity = false;
        m_Rigidbody.isKinematic = true;
        m_Collider.enabled = false;
        m_Collider.isTrigger = true;

        transform.SetParent(Player);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Player.rotation;

        Debug.Log("formo parte de pol");
    }
}
