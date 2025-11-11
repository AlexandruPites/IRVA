using System.Collections.Generic;
using L4_VR_MetaXR_Basics.AssetStore.Bottle.Scripts;
using NaughtyAttributes;
using UnityEngine;

namespace L4_VR_MetaXR_Basics.Scripts
{
    public class BottlesManager : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> bottleSpawnTransforms = new();
        
        [SerializeField]
        private Bottle bottlePrefab;
        
        private readonly List<Bottle> bottles = new();

        private void Start() => SpawnBottles();

        [Button]
        public void SpawnBottles()
        {
            // Do not do this in edit mode :p.
            if (!Application.isPlaying) return;
            
            // Clear existing bottles.
            bottles.ForEach(bottle =>
            {
                if (bottle) Destroy(bottle.gameObject);
            });
            bottles.Clear();
            
            // Spawn new bottles.
            bottleSpawnTransforms.ForEach(spawnTransform =>
            {
                var bottle = Instantiate(
                    bottlePrefab, 
                    spawnTransform.position, 
                    spawnTransform.rotation);
                bottles.Add(bottle);
            });
        }
    }
}
