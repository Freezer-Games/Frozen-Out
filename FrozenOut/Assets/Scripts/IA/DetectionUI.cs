using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Slider slider = gameObject.GetComponentInChildren<Slider>();
        Debug.Log("detection = " + deteccion );
        slider.value = deteccion;

    }

    private void Update()
    {
        gameObject.transform.LookAt(camera.transform);
    }

}
