using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{

    public static Texture2D ColormapFromHeightmap(float[,] heightmap, Region[] regions)
    {
        int width = heightmap.GetLength(0);
        int height = heightmap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        Color[] colors = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int i = 0; i < regions.Length; i++)
                {
                    Region region = regions[i];
                    float pointHeight = heightmap[x, y];

                    if (region.height < pointHeight)
                    {
                        if (i + 1 >= regions.Length)
                        {
                            colors[(y * width) + x] = region.color;
                        }
                        else
                        {
                            if (regions[i + 1].height < pointHeight)
                            {
                                continue;
                            }
                            else
                            {
                                float time = Mathf.InverseLerp(region.height, regions[i + 1].height, pointHeight);
                                colors[(y * width) + x] = Color.Lerp(region.color, regions[i + 1].color, time);
                            }
                        }
                    }

                }
            }
        }
        
        texture.SetPixels(colors);
        texture.Apply();

        return texture;
    }

}

[System.Serializable]
public struct Region
{
    public string name;
    [Range(0, 1)] public float height;
    public Color color;
}
