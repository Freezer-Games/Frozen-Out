using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLanguage : MonoBehaviour
{
    public Dropdown DropdownLanguage;
    // Start is called before the first frame update
    void Start()
    {
        DropdownLanguage.onValueChanged.AddListener(ChangeLanguageF);
    }

    void ChangeLanguageF(int selection)
    {

        if (selection == 0) { LocalizationManager.instance.LoadLocalizedText("Menu_En.json"); }
        else if (selection == 1) { LocalizationManager.instance.LoadLocalizedText("Menu_Es.json"); }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
