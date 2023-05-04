using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public Texture2D heightmap;
    public MeshFilter meshFilter;



    public void GenerateMeshFromHeightMap()
    {
        float height = heightmap.height;
        float width = heightmap.width;

        Mesh mesh = new Mesh();

        Color[] colorArray = heightmap.GetPixels();

        Vector3[] verticies = new Vector3[(int)(height * width)];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int iterator = (int)(y * width) + x;
                verticies[iterator].y = colorArray[(int)((x * y) + width)].grayscale;
                verticies[iterator].x = x / width;
                verticies[iterator].y = y / height;
            }
        }

        mesh.vertices = verticies;

        int[] triangles = new int[(int)width * 6];
        for (int ti = 0, vi = 0, x = 0; x < width; x++, ti += 6, vi++)
        {
            triangles[ti] = vi;
            triangles[ti + 3] = triangles[ti + 2] = vi + 1;
            triangles[ti + 4] = triangles[ti + 1] = vi + width + 1;
            triangles[ti + 5] = vi + width + 2;
        }
    }

}
