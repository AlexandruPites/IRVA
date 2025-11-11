using UnityEngine;

namespace L4_VR_MetaXR_Basics.AssetStore.Bottle.Scripts
{
    /// <summary>
    /// NOTE: Modified from original asset store script.
    /// </summary>
    public class Bottle : MonoBehaviour
    {
        [SerializeField]
        private AudioClip shatterSound;
        
        [SerializeField] 
        private BrokenBottle brokenBottlePrefab;

        public void Shatter()
        {
            var brokenBottle = Instantiate(brokenBottlePrefab, transform.position, Quaternion.identity);
            brokenBottle.RandomVelocities();
            if (shatterSound)
            {
                AudioSource.PlayClipAtPoint(shatterSound, transform.position);
            }
            Destroy(gameObject);
        }
    }
}
