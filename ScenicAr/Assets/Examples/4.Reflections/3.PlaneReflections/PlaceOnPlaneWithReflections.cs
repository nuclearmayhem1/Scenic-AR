using System.Collections.Generic;
using System.Linq;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Listens for touch events and performs an AR raycast from the screen touch point.
    /// AR raycasts will only hit detected trackables like feature points and planes.
    ///
    /// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
    /// and moved to the hit position.
    /// </summary>
    [RequireComponent(typeof(ARRaycastManager))]
    public class PlaceOnPlaneWithReflections : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_PlacedPrefab;
        [SerializeField]
        [Tooltip("After touching a plane, this material is added to the plane.")]
        Material m_StencilAllowerMaterial;
        [SerializeField]
        [Tooltip("After creating the mirror version, this material is set to the object.")]
        Material m_ReflectionMaterial;

        [SerializeField]
        int reflectionLayer;
        [SerializeField]
        LayerMask lightMask;

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
        ARRaycastManager m_RaycastManager;

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject placedPrefab
        {
            get { return m_PlacedPrefab; }
            set { m_PlacedPrefab = value; }
        }

        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        public GameObject spawnedObject { get; private set; }

        protected void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
        }

        void Update()
        {

            if (Input.touchCount == 0)
                return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (m_RaycastManager.Raycast(Input.GetTouch(0).position, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null)
                {
                    if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Spawnable")
                    {
                        spawnedObject = hit.collider.gameObject;
                    }
                    else
                    {
                        // Add the reflection enabler to the plane
                        var planeRenderer = s_Hits[0].trackable.transform.GetComponent<Renderer>();
                        if (planeRenderer)
                        {
                            AddReflectionEnabler(planeRenderer);
                        }

                        // Create the object and the mirror version
                        spawnedObject = Instantiate(m_PlacedPrefab, s_Hits[0].pose.position, s_Hits[0].pose.rotation);
                        var mirrorObject = Instantiate(m_PlacedPrefab);

                        // Replace the materials in the reflection
                        ReplaceMaterials(mirrorObject, m_ReflectionMaterial);

                        // Create an aux object to mirror
                        var mirrorAuxObject = new GameObject("Mirror Aux").transform;
                        mirrorAuxObject.parent = spawnedObject.transform;
                        ResetTRS(mirrorAuxObject);
                        // Add the reflection to the aux object
                        mirrorObject.transform.parent = mirrorAuxObject;
                        ResetTRS(mirrorObject.transform);

                        // Add the lights to the aux object
                        foreach (var light in FindObjectsOfType<Light>())
                        {
                            var mirrorLight = Instantiate(light.gameObject); // Clone light
                            mirrorLight.transform.parent = mirrorAuxObject;
                            mirrorLight.GetComponent<Light>().cullingMask = (int)lightMask;
                        }
                        // Flip the aux object
                        mirrorAuxObject.localScale = new Vector3(1, -1, 1);
                    }
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
                {
                    spawnedObject.transform.position = s_Hits[0].pose.position;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    spawnedObject = null;
                }
            }
        }

        private void ReplaceMaterials(GameObject mirrorObject, Material newMaterial)
        {
            foreach (var renderer in mirrorObject.GetComponentsInChildren<Renderer>())
            {
                renderer.gameObject.layer = reflectionLayer;
                renderer.material = newMaterial;
                renderer.receiveShadows = false;
                renderer.shadowCastingMode = Rendering.ShadowCastingMode.Off;
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    renderer.materials[i] = newMaterial;
                }
            }
        }

        private void AddReflectionEnabler(Renderer planeRenderer)
        {
            var materialsList = planeRenderer.materials.ToList();
            if (!materialsList.Contains(m_StencilAllowerMaterial) && materialsList.Count < 4)
            {
                materialsList.Add(m_StencilAllowerMaterial);
                planeRenderer.materials = materialsList.ToArray();
            }
        }

        private static void ResetTRS(Transform mirrorAuxObject)
        {
            mirrorAuxObject.localPosition = Vector3.zero;
            mirrorAuxObject.localRotation = Quaternion.identity;
            mirrorAuxObject.localScale = Vector3.one;
        }
    }
}
