using NaughtyAttributes;
using UnityEngine;

namespace L4_VR_MetaXR_Basics.Scripts
{
    /// <summary>
    /// Scripts which manages the gun trigger curl based on input values.
    /// </summary>
    public class GunTriggerCurlController : MonoBehaviour
    {
        [SerializeField]
        [BoxGroup("Gun components")]
        private Transform triggerTransform;
        
        [SerializeField]
        [MinMaxSlider(0f, 40f)]
        [InfoBox("Angles in degrees for the X rotation axis for minimum and maximum curl.")]
        [BoxGroup("Settings")]
        private Vector2 curlRange = new(0f, 40f);
        
        /// <summary>
        /// Sets the curl amount of the gun trigger.
        /// </summary>
        /// <param name="curlAmount">A value between 0 (no curl) and 1 (full curl).</param>
        public void SetCurl(float curlAmount)
        {
            var clampedCurl = Mathf.Clamp01(curlAmount);
            var targetAngle = clampedCurl.Remap(0f, 1f, curlRange.x, curlRange.y);
            var currentEulerAngles = triggerTransform.localEulerAngles;
            triggerTransform.localEulerAngles = new Vector3(targetAngle, currentEulerAngles.y, currentEulerAngles.z);
        }
        
        [Button("Testing | Set no curl")]
        private void SetNoCurl() => SetCurl(0f);
        
        [Button("Testing | Set full curl")]
        private void SetFullCurl() => SetCurl(1f);
    }
}
