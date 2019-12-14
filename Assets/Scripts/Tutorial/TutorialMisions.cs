using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TutorialMisions : MonoBehaviour
{
    VariableStorageBehaviour variableStorageYarn;
    MissionsCanvas misiones;
    private bool mision1 = false;
    private bool mision1p2 = false;
    private Mision misiontut;
    // Start is called before the first frame update
    void Start()
    {
        variableStorageYarn = FindObjectOfType<VariableStorageBehaviour>();
        misiones = GameObject.FindObjectOfType<MissionsCanvas>();
    }

    void Añadir_Mision(int mision, int seccion)
    {
        if (mision == 1) {
            List<string> Missiondescriptions = new List<string>();
            misiontut = new Mision();
            misiontut.nombre = "Ir a trabajar";
            Missiondescriptions.Add("Ir a la mina para trabajar");
            if (misiones.Missions.ContainsKey("mision1tutorial")) { misiones.Missions.Remove("mision1tutorial"); }
            if (seccion == 1){} else if (seccion == 2)
            {
                Missiondescriptions.Remove("Ir a la mina para trabajar");
                Missiondescriptions.Add(StrikeThrough("Ir a la mina para trabajar"));
                Missiondescriptions.Add("Bajar a la mina a extraer hielo");
            }
            misiontut.descriptions = Missiondescriptions;
            misiones.Missions.Add("mision1tutorial",misiontut);
            misiones.ActualizarMisiones();
        }

    }

    public string StrikeThrough(string s)
    {
        string strikethrough = "";
        foreach (char c in s)
        {
            strikethrough = strikethrough + c + '\u0336';
        }
        return strikethrough;
    }

    private void Update()
    {
        if (variableStorageYarn.GetValue("$tutorial_mission") != Yarn.Value.NULL && !mision1) { Añadir_Mision(1,1); mision1 = true; }
        if (variableStorageYarn.GetValue("$tutorial_missionp2") != Yarn.Value.NULL && !mision1p2) { Añadir_Mision(1, 2); mision1p2 = true; }
    }

}
