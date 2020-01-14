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
        DropdownLanguage.onValueChanged.AddListener(ChangeLanguageF);
        float size = PlayerPrefs.GetFloat("TextSize", 20);
        sizeSlider.value = size;
        if (PlayerPrefs.HasKey("Language"))
        {
            if (PlayerPrefs.GetString("Language") == "Es") { DropdownLanguage.value = 1; ChangeLanguageF(1); }

        }else if (Application.systemLanguage == SystemLanguage.Spanish) { DropdownLanguage.value = 1; ChangeLanguageF(1); }
        else { DropdownLanguage.value = 0; ChangeLanguageF(0); }

    }

    void ChangeLanguageF(int selection)
    {
        print(Application.streamingAssetsPath + "Menu_Default.json");
        if (selection == 0) {
            File.WriteAllBytes(Application.streamingAssetsPath + "/Menu_Default.json", File.ReadAllBytes(Application.streamingAssetsPath + "/Menu_En.json"));
            File.WriteAllBytes(Application.streamingAssetsPath + "/Trial_level_Default.json", File.ReadAllBytes(Application.streamingAssetsPath + "/Trial_level_En.json"));
            File.WriteAllBytes(Application.streamingAssetsPath + "/Menu_pausa_Default.json", File.ReadAllBytes(Application.streamingAssetsPath + "/Menu_pausa_En.json"));
            PlayerPrefs.SetString("Language", "En");
        }else if (selection == 1) {
            File.WriteAllBytes(Application.streamingAssetsPath + "/Menu_Default.json", File.ReadAllBytes(Application.streamingAssetsPath + "/Menu_Es.json"));
            File.WriteAllBytes(Application.streamingAssetsPath + "/Trial_level_Default.json", File.ReadAllBytes(Application.streamingAssetsPath + "/Trial_level_Es.json"));
            File.WriteAllBytes(Application.streamingAssetsPath + "/Menu_pausa_Default.json", File.ReadAllBytes(Application.streamingAssetsPath + "/Menu_pausa_Es.json"));
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
