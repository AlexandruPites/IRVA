using L4_VR_MetaXR_Basics.AssetStore.Bottle.Scripts;
using NaughtyAttributes;
using UnityEngine;

namespace L4_VR_MetaXR_Basics.Scripts
{
    /// <summary>
    /// Script used to perform raycasts when the gun is fired.
    /// </summary>
    public class GunRaycastController : MonoBehaviour
    {
        [SerializeField]
        [BoxGroup("Gun components")]
        private GunFireController gunFireController;
        
        [SerializeField]
        [BoxGroup("Raycast settings")]
        private Transform raycastOrigin;
    
        [SerializeField]
        [BoxGroup("Raycast settings")]
        private float raycastDistance = 100f;
        
        private void OnEnable() => gunFireController.OnGunFired += PerformRaycast;

        private void OnDisable() => gunFireController.OnGunFired -= PerformRaycast;
        
        private void PerformRaycast()
        {
            // TODO: → Create a raycast from the gun's raycast origin 'raycastOrigin' in the forward direction.
            //       → Check if any objects which has the 'Bottle' script attached is hit -- if so, call its 'Shatter()' method.
            var ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
            
            if (Physics.Raycast(ray, out var hitInfo, raycastDistance))
            {
                if (hitInfo.collider.TryGetComponent<Bottle>(out var bottle))
                {
                    bottle.Shatter();
                }
            }
        }
    }
}
