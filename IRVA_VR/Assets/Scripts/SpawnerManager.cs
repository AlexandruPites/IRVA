using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }
    
    [SerializeField] private List<GameObject> containerPrefabs;
    [SerializeField] private Transform lostAndFound;
    
    private List<SpawnPoint> spawnPoints = new();
    private Dictionary<GameObject, List<string>> containers = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        spawnPoints = new List<SpawnPoint>(FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None));
    }

    private void Start()
    {
        foreach (var point in spawnPoints)
        {
            GameObject prefab = null;
            if (point.spawnContainerType == SpawnPoint.SpawnContainerType.None)
            {
                prefab = containerPrefabs.GetRandomElement();
            }
            else
            {
                prefab = containerPrefabs[(int)point.spawnContainerType - 1];
            }
            GameObject spawned = Instantiate(prefab, point.gameObject.transform);
            IContainer cont = spawned.GetComponent<IContainer>();
            spawned.transform.localScale = Vector3.one * cont.ScaleModifier * point.spawnPointScaleMultiplier;
            spawned.transform.position += point.spawnPointPositionOffset;
            containers.Add(spawned, point.tags);
        }
    }

    public T YeetItem<T>(T prefab, string tag = null) where T : UnityEngine.Object
    {
        Transform spawnTransform = RequestSpawnPoint(tag);
        T newInstance = Instantiate(prefab, spawnTransform);
        return newInstance;
    }

    public Transform RequestSpawnPoint(string tag = null)
    {
        bool found = false;

        var eligibleEntries = containers.Where(kvp => 
                kvp.Key != null &&
                (string.IsNullOrEmpty(tag) || kvp.Value.Contains(tag))
        );
        
        List<IContainer> conts = eligibleEntries
            .Select(kvp => kvp.Key.GetComponent<IContainer>())
            .Where(c => c != null)
            .ToList();
        
        List<IContainer> validConts = conts
            .Where(c => c.Capacity > 0)
            .ToList();

        IContainer chosenContainer;

        if (conts.Count <= 0)
        {
            return lostAndFound;
        }

        if (validConts.Count > 0)
        {
            chosenContainer = validConts.GetRandomElement();
        }
        else
        {
            chosenContainer = conts.GetRandomElement();
        }
        
        print("Getting location");
        Transform result = chosenContainer.GetSpawnPoint();
        if (result)
        {
            return result;
        }

        return lostAndFound;
    }
}
