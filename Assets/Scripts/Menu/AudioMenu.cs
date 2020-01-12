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
        ChangeVolume(volumen);

        audioslider.onValueChanged.AddListener(nuevoVolumen => ChangeVolume(nuevoVolumen));
    }

    private void ChangeVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("volume", newVolume);
        AudioListener.volume = Mathf.Clamp(PlayerPrefs.GetFloat("volume") / 100f, 0, 1);
        volumelevel.text = audioslider.value.ToString();
        GameManager.instance.volume = newVolume;
    }
}
