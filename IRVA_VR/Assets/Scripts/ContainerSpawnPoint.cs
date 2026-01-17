using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    
    public enum SpawnContainerType
    {
        None,
        Cabinet,
        Chest
    }
    
    [SerializeField] public List<string> tags;
    [SerializeField] public float spawnPointScaleMultiplier = 1.0f;
    [SerializeField] public Vector3 spawnPointPositionOffset = Vector3.zero;
    [SerializeField] public SpawnContainerType spawnContainerType = SpawnContainerType.None;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.matrix = transform.localToWorldMatrix;
        
        Vector3 drawSize = new Vector3(1, 0.5f, 1);
        Vector3 drawCenter = new Vector3(0, drawSize.y * 0.5f, 0);
        Gizmos.DrawCube(drawCenter, drawSize);
    
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(drawCenter, drawSize);
    }
}
