using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Vector3Variable), editorForChildClasses: true)]
public class Vector3VariableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        Vector3Variable e = target as Vector3Variable;
    
        if (GUILayout.Button("Clear value")) {
            e.CurrentValue = null;
        }
    }
}
