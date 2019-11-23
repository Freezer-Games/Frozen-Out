using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GraphicsMenu : MonoBehaviour
{
    public Dropdown resolutiondropdown;
    public Dropdown proportiondropdown;
    public Dropdown screentypedropdown;
    public Button LowResButton;
    public Button MediumResButton;
    public Button HighResButton;
    public Button applybutton;




    // Start is called before the first frame update
    void Start()
    {
        applybutton.onClick.AddListener(apply_settings);
        proportiondropdown.onValueChanged.AddListener(delegate {
            proportionchange(proportiondropdown, resolutiondropdown);
        });
    }

    void apply_settings()
    {
        string a =resolutiondropdown.options[resolutiondropdown.value].text;
        char[] separator = { 'x'};
        string[] strlist = a.Split(separator);
        int res1 = Int32.Parse(strlist[0]);
        int res2 = Int32.Parse(strlist[1]);
        if (screentypedropdown.value == 0) { Screen.SetResolution(res1, res2, true); }
        else if (screentypedropdown.value == 1) { Screen.SetResolution(res1, res2, false); }
        if (HighResButton.IsInteractable() == false) { QualitySettings.masterTextureLimit = 0; }
        else if (MediumResButton.IsInteractable() == false) { QualitySettings.masterTextureLimit = 4; }
        else if (LowResButton.IsInteractable() == false) { QualitySettings.masterTextureLimit = 8; }

    }

    void proportionchange(Dropdown propdrop , Dropdown resdrop)
    {

        if (propdrop.value == 0)
        {
            resdrop.options.Clear();
            resdrop.options.Add(new Dropdown.OptionData() {text = "1920x1080" });
            resdrop.options.Add(new Dropdown.OptionData() {text = "1280x720" });
            resdrop.options.Add(new Dropdown.OptionData() {text = "2560x1440" });
            resdrop.value = 0;
        }
        else if (propdrop.value == 1)
        {
            resdrop.options.Clear();
            resdrop.options.Add(new Dropdown.OptionData() { text = "640x480" });
            resdrop.options.Add(new Dropdown.OptionData() { text = "800x600" });
            resdrop.options.Add(new Dropdown.OptionData() { text = "960x720" });
            resdrop.options.Add(new Dropdown.OptionData() { text = "1024x768" });
            resdrop.options.Add(new Dropdown.OptionData() { text = "1280x960" });
            resdrop.options.Add(new Dropdown.OptionData() { text = "1400x1050" });
            resdrop.options.Add(new Dropdown.OptionData() { text = "1440x1080" });
            resdrop.value = 0;
        }
        else if (propdrop.value == 2)
        {
            resdrop.options.Clear();
            resdrop.options.Add(new Dropdown.OptionData() { text = "600x600" });
            resdrop.options.Add(new Dropdown.OptionData() { text = "720x720" });
            resdrop.options.Add(new Dropdown.OptionData() { text = "800x800" });
            resdrop.options.Add(new Dropdown.OptionData() { text = "1080x1080" });
            resdrop.options.Add(new Dropdown.OptionData() { text = "1440x1440" });
            resdrop.value = 0;
        }
        else if (propdrop.value == 3)
        {
            resdrop.options.Clear();
            resdrop.options.Add(new Dropdown.OptionData() { text = "3840x1080" });
            resdrop.options.Add(new Dropdown.OptionData() { text = "5120x1440" });
            resdrop.value = 0;
        }

    }


    // Update is called once per frame
    void Update()
    {

    }
}
