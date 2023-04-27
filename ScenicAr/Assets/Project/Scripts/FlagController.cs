using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour
{
    public float distanceMovedBeforeRefresh = 0.1f;
    private Vector3 lastPosition = Vector3.zero;

    private void Awake()
    {
        lastPosition = transform.position;
    }

    private void Start()
    {
        FlagManager.Instance.AddPoint(transform);
    }

    private void OnDestroy()
    {
        FlagManager.Instance.RemovePoint(transform);
    }

    private void Update()
    {
        if (Vector3.Distance(lastPosition, transform.position) > distanceMovedBeforeRefresh)
        {
            FlagManager.Instance.TryRebuild();
        }
    }

}
