using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public Canvas LoadCanvas;
    [SerializeField]
    private Text loadingText;
    // Start is called before the first frame update
    void Start()
    {
        LoadCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

    }
}