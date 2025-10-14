using System;
using System.Collections;
using L1_VR_GoogleCardboard.Scripts.Helpers;
using NaughtyAttributes;
using UnityEngine;

namespace L1_VR_GoogleCardboard.Scripts.BalloonGame
{
    /// <summary>
    /// Used to control the spawn behavior of balloons.
    /// </summary>
    public class BalloonSpawner : MonoBehaviour
    {
        // Check the comments in 'BalloonController' for more info about the attributes used below.
        
        [SerializeField] 
        [Range(0.1f, 5f)] 
        [BoxGroup("Settings")]
        private float spawnRate = 1.5f;
        
        [SerializeField] 
        [BoxGroup("External components")]
        [Required]
        private GameObject balloonPrefab;

        private BoxCollider _boxCollider;
        private Coroutine _spawnRoutine;

        private void Awake() => _boxCollider = GetComponent<BoxCollider>();

        private void Start() => _spawnRoutine = StartCoroutine(SpawnBalloonsRoutine());

        private void OnDisable()
        {
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
                _spawnRoutine = null;
            }
        }

        private IEnumerator SpawnBalloonsRoutine()
        {
            while (true)
            {
                // TODO 3.1 : Determine the spawning position for the balloon.
                //            Balloons should spawn at a random position inside the volume of the bounding box.
                //            Hint: Use the `BoxCollider` components (some attribute from it) & check the `Utils` class.
                var bounds = _boxCollider.bounds; 
                var spawnPos = Utils.GetRandomPointInBounds(bounds);
                
                if(balloonPrefab != null)
                {
                    // TODO 3.2 : Spawn the balloon at the previously determined position.
                    Instantiate(balloonPrefab, spawnPos, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("[BalloonSpawner] 'balloonPrefab' is NULL!");
                }
                
                yield return new WaitForSeconds(spawnRate);
            }
        }
    }
}