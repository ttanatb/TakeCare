#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlagManager))]
public class FlagManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        FlagManager flagManager = (FlagManager)target;
        if (GUILayout.Button("Turn ON Flag"))
        {
            flagManager.TurnInspectorFlagOn();
        }

        if (GUILayout.Button("Turn OFF Flag"))
        {
            flagManager.TurnInspectorFlagOff();
        }
    }
}
#endif