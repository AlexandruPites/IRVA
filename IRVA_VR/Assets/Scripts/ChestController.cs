using System;
using UnityEngine;

public class ChestController : MonoBehaviour, IContainer
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public int chestCapacity = 2;
    [SerializeField] private float scaleModifier = 1f;

    private int currentCapacity;

    private void Awake()
    {
        currentCapacity = chestCapacity;
    }

    public int Capacity
    {
        get { return currentCapacity; }
    }

    public float ScaleModifier
    {
        get { ;return scaleModifier; }
    }

    public Transform GetSpawnPoint()
    {
        currentCapacity--;
        return spawnPoint;
    }
}
