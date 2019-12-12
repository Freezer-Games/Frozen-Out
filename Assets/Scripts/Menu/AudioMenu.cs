using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    public Slider audioslider;
    public Text volumelevel;
    // Start is called before the first frame update
    void Start()
    {
        float volumen = PlayerPrefs.GetFloat("volume", 100);
        audioslider.value = volumen;
    }

    void changevolume(float newVolume)
    {
        PlayerPrefs.SetFloat("volume", newVolume);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        volumelevel.text = audioslider.value.ToString();
        GameManager.instance.volume = newVolume;
    }

    // Update is called once per frame
    void Update()
    {
        changevolume(audioslider.value);
    }
}
