using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameCanvas : MonoBehaviour
{
    public Dropdown DropdownLanguage;
    public Slider sizeSlider;
    public Text sizeText;

    // Start is called before the first frame update
    void Start()
    {
        float size = PlayerPrefs.GetFloat("TextSize", 14);
        sizeSlider.value = size;
        if (PlayerPrefs.HasKey("Language"))
        {
            if (PlayerPrefs.GetString("Language") == "Es") { DropdownLanguage.value = 1; }

        }else if (Application.systemLanguage == SystemLanguage.Spanish) { DropdownLanguage.value = 1; }
        DropdownLanguage.onValueChanged.AddListener(ChangeLanguageF);
    }

    void ChangeLanguageF(int selection)
    {

        if (selection == 0) {
            File.WriteAllBytes("Assets/StreamingAssets/Menu_Default.json", File.ReadAllBytes("Assets/StreamingAssets/Menu_En.json"));
            File.WriteAllBytes("Assets/StreamingAssets/Trial_level_Default.json", File.ReadAllBytes("Assets/StreamingAssets/Trial_level_En.json"));
            File.WriteAllBytes("Assets/StreamingAssets/Menu_pausa_Default.json", File.ReadAllBytes("Assets/StreamingAssets/Menu_pausa_En.json"));
            PlayerPrefs.SetString("Language", "En");
        }else if (selection == 1) {
            File.WriteAllBytes("Assets/StreamingAssets/Menu_Default.json", File.ReadAllBytes("Assets/StreamingAssets/Menu_Es.json"));
            File.WriteAllBytes("Assets/StreamingAssets/Trial_level_Default.json", File.ReadAllBytes("Assets/StreamingAssets/Trial_level_Es.json"));
            File.WriteAllBytes("Assets/StreamingAssets/Menu_pausa_Default.json", File.ReadAllBytes("Assets/StreamingAssets/Menu_pausa_Es.json"));
            PlayerPrefs.SetString("Language", "Es");
        }
        LocalizationManager.instance.LoadLocalizedText("Menu_Default.json");
    }

    // Update is called once per frame
    void Update()
    {
        sizeText.text = sizeSlider.value.ToString();
        PlayerPrefs.SetFloat("TextSize", sizeSlider.value);
        GameManager.instance.TextSize = sizeSlider.value;
    }
}
