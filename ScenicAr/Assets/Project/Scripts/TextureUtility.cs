using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureUtility
{

    public static Texture2D TextureFromArray(float[,] floatArray, Func<float, Color> drawFunction)
    {
        int width = floatArray.GetLength(0);
        int height = floatArray.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        Color[] colors = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colors[(y * width) + x] = drawFunction(floatArray[x, y]);
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        return texture;
    }


    public static Color DrawGrayscale(float value)
    {
        return Color.white * value;
    }


}
