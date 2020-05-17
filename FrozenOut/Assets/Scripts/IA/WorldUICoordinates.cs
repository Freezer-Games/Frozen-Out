using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUICoordinates : MonoBehaviour
{
    public Vector2 GetCoordinates()
    {
        return RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z));
    }
}
