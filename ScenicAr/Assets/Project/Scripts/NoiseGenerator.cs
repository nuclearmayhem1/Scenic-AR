using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{


    public static float[,] GenerateNoisemapWithPerlin(int width, int height, float scale)
    {
        float[,] noisemap = new float[width, height];

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sampleX = (float)x / scale;
                float sampleY = (float)y / scale;
                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                noisemap[x, y] = perlinValue;
            }
        }
        return noisemap;
    }


}
