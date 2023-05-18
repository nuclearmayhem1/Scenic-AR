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


        GUI.DrawTexture(GUILayoutUtility.GetRect(100, 100, GUILayout.ExpandWidth(false)), terrainGenerator.noiseMapPreview);

        if (GUILayout.Button("Generate Noisemap") == true)
        {
            terrainGenerator.GenerateNoisemap();
        }



        if (GUILayout.Button("Generate Mesh") == true)
        {
            terrainGenerator.GenerateMesh();
        }

    }

}
