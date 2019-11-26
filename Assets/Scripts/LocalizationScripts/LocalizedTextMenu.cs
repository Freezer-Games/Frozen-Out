using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedTextMenu : MonoBehaviour
{
    public string key;
    // Start is called before the first frame update
    void Update()
    {
        Text text = GetComponent<Text>();
        text.text = LocalizationManagerMenuPausa.instance.GetLocalizedValue(key);
    }

}

