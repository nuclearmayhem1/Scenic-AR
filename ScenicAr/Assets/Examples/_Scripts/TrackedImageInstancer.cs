using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrackedImageInstancer : MonoBehaviour
{
    [System.Serializable]
    public struct ImagePrefab { public string imageName; public GameObject prefab; }

    // List of prefabs used to augment each image target
    public List<ImagePrefab> prefabs;

    private ARTrackedImage trackedImage;
    private GameObject target;

    void Start()
    {
        trackedImage = GetComponent<ARTrackedImage>();
        var prefab = prefabs.Find(p => p.imageName == trackedImage.referenceImage.name);
        target = Instantiate(prefab.prefab, this.transform);
    }

    void Update()
    {
        target.SetActive(trackedImage.trackingState != UnityEngine.XR.ARSubsystems.TrackingState.None);
    }
}
