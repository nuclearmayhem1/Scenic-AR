using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    private static FlagManager instance;
    public static FlagManager Instance { get { return instance; } }

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Transform quadPosition;

    private void Awake()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        }
    }

    public List<Transform> flags = new List<Transform>();

    public Rect boundry = new Rect();

    public void AddPoint(Transform transform)
    {
        flags.Add(transform);
        TryRebuild();
    }

    public void RemovePoint(Transform transform)
    {
        flags.Remove(transform);
    }

    public void TryRebuild()
    {
        if (flags.Count > 1)
        {
            boundry = BuildRect(flags);
            TryBuildMesh();
        }
    }

    public Rect BuildRect(List<Transform> points)
    {
        Vector2 minxy = Vector2.one * float.MaxValue;
        Vector2 maxxy = Vector2.one * float.MinValue;


        foreach (Transform point in points)
        {
            if (point.position.x < minxy.x)
            {
                minxy.x = point.position.x;
            }
            if (point.position.z < minxy.y)
            {
                minxy.y = point.position.z;
            }
        }

        foreach (Transform point in points)
        {
            if (point.position.x > maxxy.x)
            {
                maxxy.x = point.position.x;
            }
            if (point.position.z > maxxy.y)
            {
                maxxy.y = point.position.z;
            }
        }

        return new Rect(minxy, maxxy - minxy);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(boundry.center.x, 0, boundry.center.y), new Vector3(boundry.size.x, 0, boundry.size.y));
    }

    public void TryBuildMesh()
    {
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(boundry.size.x, 0, 0),
            new Vector3(0, 0, boundry.size.y),
            new Vector3(boundry.size.x, 0, boundry.size.y)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
        PositionQuad();
    }

    public void PositionQuad()
    {
        quadPosition.position = new Vector3(boundry.position.x, 0, boundry.position.y);
    }

}
