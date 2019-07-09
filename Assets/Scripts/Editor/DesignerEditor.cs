using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Designer))]
public class DesignerEditor : Editor
{
    Designer designer;

    private void OnEnable()
    {
        designer = (Designer)target;
        designer.PopulatePrefabs();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //if(GUILayout.Button("Draw Grid"))
        //{
        //    designer.DrawBackground();
        //}

        if(GUILayout.Button("Load Level"))
        {
            designer.LoadLevel();
        }

        if(GUILayout.Button("Save Level"))
        {
            designer.SaveLevel();
        }

        if (GUILayout.Button("Clear Level"))
        {
            designer.ClearLevel();
        }
    }
}
