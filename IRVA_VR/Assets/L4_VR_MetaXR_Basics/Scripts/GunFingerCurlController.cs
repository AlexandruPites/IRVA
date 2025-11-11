using System;
using NaughtyAttributes;
using Oculus.Interaction.PoseDetection;
using Oculus.Interaction.Input;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace L4_VR_MetaXR_Basics.Scripts
{
    /// <summary>
    /// Script used to read index finger curl and control gun trigger accordingly.
    /// </summary>
    public class GunFingerCurlController : MonoBehaviour
    {
        [SerializeField]
        [BoxGroup("Gun components")]
        private GunHandGrabController gunHandGrabController;
        
        [SerializeField]
        [BoxGroup("Gun components")]
        private GunTriggerCurlController gunTriggerCurlController;
        
        [SerializeField]
        [MinMaxSlider(0f, 300f)]
        [InfoBox("Set min and max experimentally — test where finger curl starts touching the trigger and where it is fully pressed.")]
        [BoxGroup("Settings")]
        private Vector2 usableFingerCurlRange = new(0f, 300f);
        
        [SerializeField]
        [Range(0f, 1f)]
        [InfoBox("Threshold value for considering the trigger as pressed (gun fired).")]
        [BoxGroup("Settings")]
        private float triggerThreshold = 0.5f;

        public event Action OnTriggerPressed;
        public event Action OnTriggerReleased;
        
        private bool _isTriggerPressed = false;
        
        private void Update()
        {
            // Abort if gun is not grabbed.
            if (!gunHandGrabController.IsGunGrabbed())
            {
                return;
            }
            gunHandGrabController.GetCurrentFingerStateProvider(out var provider);
            // Abort if no provider found.
            if (provider == null)
            {
                return;
            }
            
            // TODO: → Get curl value from index finger.
            //       → Use 'provider.GetFeatureValue' method with relevant parameters.
            //       → Print the value.
            //       → Use value to calibrate the 'usableFingerCurlRange' in the inspector.
            
            var curlValue = provider.GetFeatureValue(HandFinger.Index, FingerFeature.Curl);
            var curlFloat = curlValue ?? 0f;

            // TODO: → Remap curl value to 0-1 based on your usable range.
            //       → Note, you can use the 'Remap' extension method from the 'Utils' class.
            var normCurl = 0f;
            
            // TODO: → Set the curl on the gun trigger curl controller.
            normCurl = Utils.Remap(curlFloat, usableFingerCurlRange.x, usableFingerCurlRange.y, 0f, 1f);
            gunTriggerCurlController.SetCurl(normCurl);
            
            // Trigger logic.
            if (!_isTriggerPressed && normCurl >= triggerThreshold)
            {
                _isTriggerPressed = true;
                OnTriggerPressed?.Invoke();
                Debug.Log("[GunFingerCurlController] Trigger pressed.");
            }
            else if (_isTriggerPressed && normCurl < triggerThreshold)
            {
                _isTriggerPressed = false;
                OnTriggerReleased?.Invoke();
                Debug.Log("[GunFingerCurlController] Trigger released.");
            }
        }

        public void ReleaseStop()
        {
            _isTriggerPressed = false;
            OnTriggerReleased?.Invoke();
            gunTriggerCurlController.SetCurl(0f);
        }
    }
}
