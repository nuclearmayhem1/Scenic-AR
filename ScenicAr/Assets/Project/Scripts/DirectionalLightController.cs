using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightController : MonoBehaviour
{
    public Transform dirLight;

    public void MoveLeft()
    {
        dirLight.Rotate(0f, 20f, 0f, Space.World);
    }

    public void MoveRight()
    {
        dirLight.Rotate(0f, -20f, 0f, Space.World);
    }

    public void MoveDown()
    {
        dirLight.Rotate(-20f, 0f, 0f, Space.Self);
    }

    public void MoveUp()
    {
        dirLight.Rotate(20f, 0f, 0f, Space.Self);
    }
}
