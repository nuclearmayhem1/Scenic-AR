using System.Collections.Generic;
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
    public class PlaceOnPlane : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_PlacedPrefab;
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
                        spawnedObject = Instantiate(m_PlacedPrefab, s_Hits[0].pose.position, s_Hits[0].pose.rotation);
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
    }
}
