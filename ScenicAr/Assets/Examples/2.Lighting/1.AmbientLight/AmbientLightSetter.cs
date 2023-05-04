using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// A component that can be used to access the most recently received basic light estimation information
    /// for the physical environment as observed by an AR device.
    /// </summary>
    public class AmbientLightSetter : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The ARCameraManager which will produce frame events containing light estimation information.")]
        ARCameraManager m_CameraManager;

        /// <summary>
        /// Get or set the <c>ARCameraManager</c>.
        /// </summary>
        public ARCameraManager cameraManager
        {
            get { return m_CameraManager; }
            set
            {
                if (m_CameraManager == value)
                    return;

                if (m_CameraManager != null)
                    m_CameraManager.frameReceived -= FrameChanged;

                m_CameraManager = value;

                if (m_CameraManager != null & enabled)
                    m_CameraManager.frameReceived += FrameChanged;
            }
        }

        /// <summary>
        /// The estimated brightness of the physical environment, if available.
        /// </summary>
        public float? brightness { get; private set; }

        /// <summary>
        /// The estimated color temperature of the physical environment, if available.
        /// </summary>
        public float? colorTemperature { get; private set; }

        /// <summary>
        /// The estimated color correction value of the physical environment, if available.
        /// </summary>
        public Color? colorCorrection { get; private set; }

        void Awake()
        {
            m_Light = GetComponent<Light>();
        }

        void OnEnable()
        {
            if (m_CameraManager != null)
                m_CameraManager.frameReceived += FrameChanged;
        }

        void OnDisable()
        {
            if (m_CameraManager != null)
                m_CameraManager.frameReceived -= FrameChanged;
        }

        void FrameChanged(ARCameraFrameEventArgs args)
        {
            string lightInfo = "Light Info: \n";
            if (args.lightEstimation.averageBrightness.HasValue)
            {
                brightness = args.lightEstimation.averageBrightness.Value;
                RenderSettings.ambientIntensity = brightness.Value * 3; // Make it closer to 1
                lightInfo += "Ambient Intensity: " + (RenderSettings.ambientIntensity) + "\n";
            }
            else
            {
                brightness = null;
            }

            if (args.lightEstimation.colorCorrection.HasValue)
            {
                colorCorrection = args.lightEstimation.colorCorrection.Value;
                var colorCorrectionValue = colorCorrection.Value;// + new Color(0, 0, 0, 1 - colorCorrection.Value.a);
                colorCorrectionValue.a = 1f;
                RenderSettings.ambientLight = colorCorrectionValue;
                lightInfo += "Color Correction: " + (colorCorrectionValue) + "\n";
                //m_Light.color = colorCorrection.Value;
                lightInfo += "Ambient Light: " + (RenderSettings.ambientLight) + "\n";
            }
            else
            {
                colorCorrection = null;
            }

            if (args.lightEstimation.averageColorTemperature.HasValue)
            {
                colorTemperature = args.lightEstimation.averageColorTemperature.Value;
                lightInfo += "Color Temperature: " + (colorTemperature.Value) + "\n";
                RenderSettings.ambientLight = colorTemperature.Value * Mathf.CorrelatedColorTemperatureToRGB(colorTemperature.Value);
                lightInfo += "Ambient Light (AfterTemp): " + (RenderSettings.ambientLight) + "\n";
            }
            else
            {
                colorTemperature = null;
            }
            UIDebug.Instance.Set(lightInfo);
        }

        Light m_Light;
    }
}
