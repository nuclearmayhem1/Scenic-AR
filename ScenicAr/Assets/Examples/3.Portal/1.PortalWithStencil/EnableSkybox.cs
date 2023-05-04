using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSkybox : MonoBehaviour
{

    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        UIDebug.Instance.Append("Before: " + camera.clearFlags.ToString() + "\n");
        camera.clearFlags = CameraClearFlags.Skybox;
        UIDebug.Instance.Append("After: " + camera.clearFlags.ToString() + "\n");
    }
}
