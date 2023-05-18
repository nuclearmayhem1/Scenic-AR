using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Texture2D heightMapTexture;
    public MeshFilter target;
    public Material targetMaterial;
    public float heightMultiplier = 1;

    public int width = 128;
    public int height = 128;
    public float noiseScale = 1;
    public int octaves = 1;
    [Range(0, 1)] public float persistance = 1;
    public float lacunarity = 1;
    public int seed = 0;
    public Vector2 offset = Vector2.zero;

    public float[,] noiseMap = null;

    public Region[] regions;

    public bool autoGenerate = false;

    [HideInInspector] public Texture2D noisemapPreview = null;
    [HideInInspector] public Texture2D heightmapPreview = null;
    [HideInInspector] public Texture2D colormap = null;

    private void OnValidate()
    {
        if (octaves < 1)
        {
            octaves = 1;
        }
        if (height < 1)
        {
            height = 1;
        }
        if (width < 1)
        {
            width = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (autoGenerate)
        {
            GenerateNoisemap();
            GenerateColormap();
            GenerateMesh();
        }
    }

    public void GenerateMesh()
    {
        MeshData meshData = MeshGenerator.GenerateMeshFromHeightMap(noiseMap, heightMultiplier);
        target.mesh = meshData.CreateMesh();
        targetMaterial.SetTexture("_MainTex", colormap);
    }

    public void GenerateNoisemap()
    {
        noiseMap = NoiseGenerator.GenerateNoisemapWithPerlin(width, height, noiseScale, seed, octaves, persistance, lacunarity, offset);
        noisemapPreview = TextureUtility.TextureFromArray(noiseMap, TextureUtility.DrawGrayscale);
    }

    public void GenerateColormap()
    {
        colormap = TextureGenerator.ColormapFromHeightmap(noiseMap, regions);
    }
}
