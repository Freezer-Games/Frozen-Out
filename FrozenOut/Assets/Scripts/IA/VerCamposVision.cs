using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if (UNITY_EDITOR)
[CustomEditor(typeof(Vision))]
public class VerCamposVision : Editor
{
    //Dibujado de circulos y flechas para representar vision y objetos visibles
    private void OnSceneGUI()
    {
        Vision vision = (Vision)target; //target coge el objeto seleccionado
        Handles.color = Color.white;
        Handles.DrawWireArc(vision.transform.position, Vector3.up, Vector3.forward, 360, vision.RadioVista);
        Handles.DrawWireArc(vision.transform.position, Vector3.up, Vector3.forward, 360, vision.trueSightRadius);
        Handles.DrawWireArc(vision.transform.position, Vector3.up, Vector3.forward, 360, vision.RadioCercanos);
        Vector3 viewAngleA = vision.DirFromAngle(-vision.viewAngle / 2, false);
        Vector3 viewAngleB = vision.DirFromAngle(vision.viewAngle / 2, false);

        Handles.DrawLine(vision.transform.position, vision.transform.position + viewAngleA * vision.RadioVista);
        Handles.DrawLine(vision.transform.position, vision.transform.position + viewAngleB * vision.RadioVista);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in vision.ObjetosVistos)
        {
            Handles.DrawLine(vision.transform.position, visibleTarget.position);
        }
        Handles.color = Color.yellow;
        foreach (Transform closeTarget in vision.ObjetosCercanos)
        {
            Handles.DrawLine(vision.transform.position, closeTarget.position);
        }
    }

}
#endif
