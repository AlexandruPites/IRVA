using NaughtyAttributes;
using UnityEngine;

namespace L4_VR_MetaXR_Basics.Scripts
{
    /// <summary>
    /// Script used to play visual and audio effects when the gun is fired.
    /// </summary>
    public class GunFireEffectsController : MonoBehaviour
    {
        [SerializeField]
        [BoxGroup("Gun components")]
        private GunFireController gunFireController;
        
        [SerializeField]
        [BoxGroup("Effects")]
        private ParticleSystem muzzleFlash;
        
        [SerializeField]
        [BoxGroup("Effects")]
        private ParticleSystem smokeEffect;
        
        [SerializeField]
        [BoxGroup("Effects")]
        private AudioClip fireSound;

        private void OnEnable() => gunFireController.OnGunFired += PlayFireEffects;

        private void OnDisable() => gunFireController.OnGunFired -= PlayFireEffects;

        private void PlayFireEffects()
        {
            if (muzzleFlash)
            {
                muzzleFlash.Play();
            }
            if (smokeEffect && muzzleFlash)
            {
                Instantiate(smokeEffect, muzzleFlash.transform.position, muzzleFlash.transform.rotation); // Will be destroyed automatically after its duration.
            }
            if (fireSound)
            {
                AudioSource.PlayClipAtPoint(fireSound, transform.position);
            }
        }
    }
}
