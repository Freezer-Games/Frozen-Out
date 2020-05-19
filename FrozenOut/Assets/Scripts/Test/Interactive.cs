using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive : MonoBehaviour
{
    [SerializeField] Transform InteractionPoint;

    public abstract void Execute();

    public abstract void Advise();

    public Transform GetInteractPoint()
    {
        return InteractionPoint;
    }
}
