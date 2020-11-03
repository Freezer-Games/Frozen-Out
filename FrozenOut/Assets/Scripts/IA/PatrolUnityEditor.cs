using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Patrulla)), CanEditMultipleObjects]
public class PatrolUnityEditor : Editor
{
    MonoScript Script;
    SerializedProperty Type;
    SerializedProperty Destinies;
    SerializedProperty PatrolPosition;
    SerializedProperty Look;
    SerializedProperty NextPoint;
    SerializedProperty HasPauses;
    SerializedProperty Pauses;
    SerializedProperty LookPauses;
    SerializedProperty TimesPauses;
    bool ShowFoldout1 = false;
    int size;

    void OnEnable()
    {
        Script = MonoScript.FromMonoBehaviour((Patrulla)target);
        Type = serializedObject.FindProperty("Tipo");
        Destinies = serializedObject.FindProperty("Destinos");
        PatrolPosition = serializedObject.FindProperty("PosicionPatrulla");
        Look = serializedObject.FindProperty("Mira");
        NextPoint = serializedObject.FindProperty("SiguientePunto");
        HasPauses = serializedObject.FindProperty("TienePausas");
        Pauses = serializedObject.FindProperty("Pausas");
        LookPauses = serializedObject.FindProperty("MiraPausas");
        TimesPauses = serializedObject.FindProperty("TiemposPausas");
        size = Pauses.arraySize;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Script = EditorGUILayout.ObjectField("Script:", Script, typeof(MonoScript), false) as MonoScript;
        EditorGUILayout.PropertyField(Type);
        if (Type.enumValueIndex == 2)
        {
            EditorGUILayout.PropertyField(PatrolPosition);
            EditorGUILayout.PropertyField(Look);
            Destinies.ClearArray();

        }
        else
        {
            EditorGUILayout.PropertyField(Destinies);
            EditorGUILayout.PropertyField(HasPauses);
        }
        if (HasPauses.boolValue)
        {

            EditorStyles.label.fontStyle = FontStyle.Bold;
            size = EditorGUILayout.IntField("Number of Pauses", size);

            bool PausesBool = EditorGUILayout.PropertyField(Pauses, false);

            Pauses.arraySize = size;

            if (PausesBool)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < size; i++)
                {
                    EditorGUILayout.PropertyField(Pauses.GetArrayElementAtIndex(i), new GUIContent("Element " + i));
                }
                EditorGUI.indentLevel--;
            }

            LookPauses.arraySize = size;

            bool LookPausesBool = EditorGUILayout.PropertyField(LookPauses, false);

            if (LookPausesBool)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < size; i++)
                {
                    EditorGUILayout.PropertyField(LookPauses.GetArrayElementAtIndex(i), new GUIContent("Element " + i));
                }
                EditorGUI.indentLevel--;
            }

            TimesPauses.arraySize = size;
            bool TimesPausesBool = EditorGUILayout.PropertyField(TimesPauses, false);

            if (TimesPausesBool)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < size; i++)
                {
                    EditorGUILayout.PropertyField(TimesPauses.GetArrayElementAtIndex(i), new GUIContent("Element " + i));
                }
                EditorGUI.indentLevel--;
            }
        }
        else
        {
            Pauses.ClearArray();
            LookPauses.ClearArray();
            TimesPauses.ClearArray();
        }
        serializedObject.ApplyModifiedProperties();
    }

}
