using System;
using NaughtyAttributes;
using UnityEngine;
using static L4_VR_MetaXR_Basics.Scripts.Utils;

namespace L4_VR_MetaXR_Basics.Scripts
{
    /// <summary>
    /// Script used to control gun firing based on trigger presses.
    /// </summary>
    public class GunFireController : MonoBehaviour
    {
        [SerializeField]
        [BoxGroup("Gun components")]
        private GunFingerCurlController gunFingerCurlController;

        [SerializeField]
        [BoxGroup("Settings")]
        private GunFireMode gunFireMode = GunFireMode.SemiAutomatic;
        
        [SerializeField] private float fireRate = 10f;

        // In semi-automatic mode, this flag ensures the gun can only fire once per trigger press.
        private bool _canFire = true;
        private bool isTriggerHeld = false;
        private float nextFireTime = 0f;

        public event Action OnGunFired;

        private void Update()
        {
            if (gunFireMode == GunFireMode.Automatic && isTriggerHeld && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + (1f / fireRate);
                FireGun();
            }
        }

        private void OnEnable()
        {
            gunFingerCurlController.OnTriggerPressed  += HandleTriggerPressed;
            gunFingerCurlController.OnTriggerReleased += HandleTriggerReleased;
        }

        private void OnDisable()
        {
            gunFingerCurlController.OnTriggerPressed  -= HandleTriggerPressed;
            gunFingerCurlController.OnTriggerReleased -= HandleTriggerReleased;
        }

        private void HandleTriggerPressed()
        {
            isTriggerHeld = true;
            if (gunFireMode == GunFireMode.SemiAutomatic)
            {
                if (_canFire)
                {
                    FireGun();
                    _canFire = false;
                }
            }
        }

        private void HandleTriggerReleased()
        {
            isTriggerHeld = false;
            if (gunFireMode == GunFireMode.SemiAutomatic)
            {
                _canFire = true;
            }
        }

        private void FireGun()
        {
            Debug.Log("[GunFireController] Gun fired!");
            OnGunFired?.Invoke();
        }

        public void ChangeMode()
        {
            gunFireMode = (gunFireMode == GunFireMode.SemiAutomatic) ? GunFireMode.Automatic : GunFireMode.SemiAutomatic;
            _canFire = true;
            print($"mode changed to {gunFireMode}");
        }
        
        public GunFireMode GetCurrentMode()
        {
            return gunFireMode;
        }
    }
}
