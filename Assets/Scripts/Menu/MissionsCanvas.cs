using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsCanvas : MonoBehaviour
{
    public Canvas MisionsCanvas;
    public GameObject MisionsPanel;
    public GameObject DescriptionsPanel;
    public List<Mision> Missions = new List<Mision>();
    // Start is called before the first frame update
    void Start()
    {
        MisionsCanvas.enabled = false;
    }


    void CloseOpenMenu()
    {
        MisionsCanvas.enabled = !MisionsCanvas.enabled;
        if (MisionsCanvas.enabled == true) { Time.timeScale = 0; Cursor.visible = true; Cursor.lockState = CursorLockMode.None; } else { Time.timeScale = 1; Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked; } //ActualizarMisiones(); }

    }

    public void ActualizarMisiones()
    {
        VaciarMisiones();
        for (int i = 0; i < Missions.Count; i++)
        {
            //Añadir Mision
            GameObject misiontitulo = new GameObject("myTextGO");
            misiontitulo.transform.SetParent(MisionsPanel.transform);
            misiontitulo.AddComponent<RectTransform>();
            misiontitulo.GetComponent<RectTransform>().sizeDelta = new Vector2(300,80);
            misiontitulo.transform.position = new Vector3(180, 850 - 70*i, 0);
            int j = i;
            misiontitulo.AddComponent<Button>().onClick.AddListener(() => { SeleccionarMision(misiontitulo, j); });

            Text myText = misiontitulo.AddComponent<Text>();
            myText.supportRichText = true;
            myText.color = new Color(255f, 255f, 255f);
            myText.text = Missions[i].nombre;
            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            myText.font = ArialFont;
            myText.fontSize = 30;
            myText.material = ArialFont.material;
            myText.alignment = TextAnchor.MiddleLeft;
        }
    }

    void VaciarMisiones()
    {
        foreach (Transform child in MisionsPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }


    void SeleccionarMision(GameObject misiontitulo, int i)
    {
        LimpiarDescripcion();
        Text tittext =  misiontitulo.GetComponent<Text>();
        tittext.text = "<color=blue>" + tittext.text + "</color>";
        GameObject misiondescription = new GameObject("myTextGO");
        misiondescription.transform.SetParent(DescriptionsPanel.transform);
        misiondescription.AddComponent<RectTransform>();
        misiondescription.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 2000);
        misiondescription.transform.position = new Vector3(770, 850, 0);

        Text myText = misiondescription.AddComponent<Text>();
        myText.color = new Color(255f, 255f, 255f);
        myText.text = Missions[i].description;
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        myText.font = ArialFont;
        myText.fontSize = 25;
        myText.alignment = TextAnchor.MiddleLeft;

    }

    void LimpiarDescripcion()
    {
        int i = 0;
        foreach (Transform child in MisionsPanel.transform)
        {
            child.gameObject.GetComponent<Text>().text = Missions[i].nombre;
            i++;
        }

        foreach (Transform child in DescriptionsPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CloseOpenMenu();
        }
    }
}
