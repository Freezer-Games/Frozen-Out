using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionUI : MonoBehaviour
{
    public GameObject camera;
    bool x = true;
    public void showUI()
    {
        Vector2 position = gameObject.GetComponentInChildren<WorldUICoordinates>().GetCoordinates();
        gameObject.transform.position = position;
        gameObject.transform.LookAt(camera.transform);
    }

    public void UpdateUI(int deteccion) {
        SpriteRenderer sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        Debug.Log("detection = " + deteccion );

        if (x)
        {
            sprite.color = new Color(sprite.color.r, 255 - 100, 255 - 100);
            x = false;
        }
    }

    private void Update()
    {
        gameObject.transform.LookAt(camera.transform);
    }

}
