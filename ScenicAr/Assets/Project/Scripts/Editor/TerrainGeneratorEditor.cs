using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        TerrainGenerator terrainGenerator = (TerrainGenerator)target;

        GUILayout.BeginHorizontal();
        GUI.DrawTexture(GUILayoutUtility.GetRect(100, 100, GUILayout.ExpandWidth(false)), terrainGenerator.noisemapPreview);
        GUI.DrawTexture(GUILayoutUtility.GetRect(100, 100, GUILayout.ExpandWidth(false)), terrainGenerator.colormap);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Noisemap") == true)
        {
            terrainGenerator.GenerateNoisemap();
        }

        if (GUILayout.Button("Generate Colormap") == true)
        {
            terrainGenerator.GenerateColormap();
        }

        if (GUILayout.Button("Generate Mesh") == true)
        {
            terrainGenerator.GenerateMesh();
        }
        GUILayout.EndHorizontal();
    }

}
