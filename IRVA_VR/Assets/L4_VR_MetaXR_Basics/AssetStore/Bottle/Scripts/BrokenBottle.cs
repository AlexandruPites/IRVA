using UnityEngine;

namespace L4_VR_MetaXR_Basics.AssetStore.Bottle.Scripts
{
    /// <summary>
    /// NOTE: Modified from original asset store script.
    /// </summary>
    public class BrokenBottle : MonoBehaviour
    {
        [SerializeField] 
        private GameObject[] pieces;
        
        [SerializeField] 
        private float velocityMultiplier = 2f;
        
        [SerializeField] 
        private float timeBeforeDestroying = 60f;

        private void Start() => Destroy(gameObject, timeBeforeDestroying);

        public void RandomVelocities()
        {
            for(var i = 0; i <= pieces.Length - 1; i++)
            {
                var vel = new Vector3(
                    velocityMultiplier * Random.Range(-1f, 1f), 
                    velocityMultiplier * Random.Range(-1f, 1f), 
                    velocityMultiplier * Random.Range(-1f, 1f));
                pieces[i].GetComponent<Rigidbody>().linearVelocity = vel;
            }
        }
    }
}