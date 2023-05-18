using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Texture2D heightMapTexture;
    public MeshFilter target;
    public float heightMultiplier = 1;

    public int width = 128;
    public int height = 128;
    public float noiseScale = 1;

    public float[,] noiseMap = null;

    [HideInInspector] public Texture2D noiseMapPreview = null;
    [HideInInspector] public Texture2D heightMapPreview = null;


    public void GenerateMesh()
    {
        MeshData meshData = MeshGenerator.GenerateMeshFromHeightMap(noiseMap, heightMultiplier);
        target.mesh = meshData.CreateMesh();
    }


    public void GenerateNoisemap()
    {
        noiseMap = NoiseGenerator.GenerateNoisemapWithPerlin(width, height, noiseScale);
        noiseMapPreview = TextureUtility.TextureFromArray(noiseMap, TextureUtility.DrawGrayscale);
    }

}
