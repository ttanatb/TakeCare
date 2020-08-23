#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CsvToDialogueModelSO))]
public class CsvToDialogueModelSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CsvToDialogueModelSO convertor = (CsvToDialogueModelSO)target;
        if (GUILayout.Button("Convert"))
        {
            convertor.ReadCSV();
        }
    }
}

#endif