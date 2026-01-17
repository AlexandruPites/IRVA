using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cabinet : MonoBehaviour, IContainer
{
    [SerializeField] private float scaleModifier = 0.5f;
    public List<Drawer> drawers;
    private bool occupation;
    private int remainingCapacity;

    private List<int> drawerCapacities = new();


    private void Awake()
    {
        foreach (var drawer in drawers)
        {
            drawerCapacities.Add(drawer.capacity);
        }

        remainingCapacity = CalculateRemainingCapacity();
    }

    public int Capacity
    {
        get { return remainingCapacity; }
    }
    
    public float ScaleModifier
    {
        get {return scaleModifier; }
    }
    public Transform GetSpawnPoint()
    {
        var validIndices = drawerCapacities
            .Select((cap, index) => new { Index = index, Capacity = cap }) 
            .Where(item => item.Capacity > 0)
            .Select(item => item.Index)
            .ToList();
        
        if (validIndices.Count == 0)
        {
            return null;
        }
        
        int chosenIndex = validIndices.GetRandomElement();
        drawerCapacities[chosenIndex]--;
        remainingCapacity = CalculateRemainingCapacity();
        return drawers[chosenIndex].spawnRoot;
    }

    private int CalculateRemainingCapacity()
    {
        int sum = 0;
        foreach (var _capacity in drawerCapacities)
        {
            sum += _capacity;
        }

        return sum;
    }
}
