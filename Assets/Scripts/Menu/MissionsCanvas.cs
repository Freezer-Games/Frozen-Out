using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsCanvas : MonoBehaviour
{
    public Canvas MisionsCanvas;
    public GameObject MisionsPanel;
    public GameObject DescriptionsPanel;
    public Dictionary<string, Mision> Missions = new Dictionary<string, Mision>();
    //public List<Mision> Missions = new List<Mision>();
    // Start is called before the first frame update
    void Start()
    {
        MisionsCanvas.enabled = false;
    }


    void CloseOpenMenu()
    {
        if (MisionsCanvas.enabled == false && !GameManager.instance.menuopen) { GameManager.instance.menuopen = true; MisionsCanvas.enabled = true; Time.timeScale = 0; Cursor.visible = true; Cursor.lockState = CursorLockMode.None; }
        else if (MisionsCanvas.enabled == true && GameManager.instance.menuopen) { Time.timeScale = 1; Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked; MisionsCanvas.enabled = false; GameManager.instance.menuopen = false; }
        //if (MisionsCanvas.enabled == true) { Time.timeScale = 0; Cursor.visible = true; Cursor.lockState = CursorLockMode.None; } else { Time.timeScale = 1; Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked; } //ActualizarMisiones(); }

    }

    public void ActualizarMisiones()
    {
        VaciarMisiones();
        int i = 0;
        foreach (string mision in Missions.Keys)
        {            
            //Añadir Mision
            GameObject misiontitulo = new GameObject("myTextGO");
            misiontitulo.transform.SetParent(MisionsPanel.transform);
            misiontitulo.AddComponent<RectTransform>();
            misiontitulo.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 80);
            misiontitulo.transform.position = new Vector3(180, 850 - 70 * i, 0);
            misiontitulo.AddComponent<Button>().onClick.AddListener(() => { SeleccionarMision(misiontitulo, mision); });

            Text myText = misiontitulo.AddComponent<Text>();
            myText.supportRichText = true;
            myText.color = new Color(255f, 255f, 255f);
            myText.text = Missions[mision].nombre;
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


    void SeleccionarMision(GameObject misiontitulo, string mision)
    {
        LimpiarDescripcion();
        Text tittext =  misiontitulo.GetComponent<Text>();
        tittext.text = "<color=blue>" + tittext.text + "</color>";
        int k = 0;
        foreach (string desctiption in Missions[mision].descriptions) {
            GameObject misiondescription = new GameObject("myTextGO");
            misiondescription.transform.SetParent(DescriptionsPanel.transform);
            misiondescription.AddComponent<RectTransform>();
            misiondescription.GetComponent<RectTransform>().sizeDelta = new Vector2(1100, 100);
            misiondescription.transform.position = new Vector3(1070, 850 - k*60, 0);

            Text myText = misiondescription.AddComponent<Text>();
            myText.color = new Color(255f, 255f, 255f);
            myText.text = Missions[mision].descriptions[k];
            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            myText.font = ArialFont;
            myText.fontSize = 25;
            myText.alignment = TextAnchor.MiddleLeft;
            k++;
        }
    }

    void LimpiarDescripcion()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in MisionsPanel.transform)
        {
            children.Add(child);
        }
        int i = 0;
        foreach (KeyValuePair<string, Mision> entry in Missions)
        {
            children[i].gameObject.GetComponent<Text>().text = entry.Value.nombre;
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
            LimpiarDescripcion();
        }
        //Debug.Log(Missions.Count);
    }
}
