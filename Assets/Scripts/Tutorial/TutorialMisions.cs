using UnityEngine;
using Yarn.Unity;

public class TutorialMisions : MonoBehaviour
{
    VariableStorageBehaviour variableStorageYarn;
    MissionsCanvas misiones;
    private bool mision1 = false;
    // Start is called before the first frame update
    void Start()
    {
        variableStorageYarn = FindObjectOfType<VariableStorageBehaviour>();
        misiones = GameObject.FindObjectOfType<MissionsCanvas>();
    }

    void Añadir_Mision(int i)
    {
        if (i == 1) {
            Mision misiontut = new Mision();
            misiontut.nombre = "Ir a trabajar";
            misiontut.description = "Ir a la mina para trabajar";
            misiones.Missions.Add(misiontut);
            misiones.ActualizarMisiones();
        }

    }

    private void Update()
    {
        if (variableStorageYarn.GetValue("$tutorial_mission") != Yarn.Value.NULL && !mision1) { Añadir_Mision(1); mision1 = true; }
    }

}
