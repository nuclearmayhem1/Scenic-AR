using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingFieldManager : MonoBehaviour
{
    public Transform[] flags = new Transform[3];
    public Transform quad;
    public Transform rotationPivot;

    private void Update()
    {
        quad.position = flags[0].position;
        rotationPivot.rotation = Quaternion.LookRotation(flags[1].position - flags[0].position, Vector3.up);

        Vector3 scale = Vector3.zero;

        scale.y = 1;
        scale.z = Vector3.Distance(flags[0].position, flags[1].position);
        scale.x = Vector3.Distance(flags[0].position, flags[2].position);

        quad.localScale = scale;
    }


}
