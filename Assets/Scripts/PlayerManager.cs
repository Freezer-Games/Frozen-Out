using System;
using UnityEngine;

[Serializable]
public class PlayerManager 
{
    public Transform m_SpawnPoint;
    public Transform m_checkPoint;
    [HideInInspector] public GameObject m_Instance;

    private PlayerController m_PlayerController;
    
    public void Setup() {
        m_PlayerController = m_Instance.GetComponent<PlayerController>();
    }

    public void DisableControl() 
    {
        m_PlayerController.enabled = false;

    }

    public void EnableControl() 
    {
        m_PlayerController.enabled = true;
    }

    public void Reset() 
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }

    public void ResetOnCheckpoint()
    {
        m_Instance.transform.position = m_checkPoint.position;
        m_Instance.transform.rotation = m_checkPoint.rotation;
    }
}
