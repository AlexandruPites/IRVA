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
        // This should not be modified to preserve compatibility with Instantiate
        Transform spawnTransform = RequestSpawnPoint(tag);
        T newInstance = Instantiate(prefab, spawnTransform);
        return newInstance;
    }

    public Transform RequestSpawnPoint(string tag = null, bool debug = false)
    {
        if (debug) Debug.Log($"<b>[Spawner] Requesting Spawn for Tag: '{(string.IsNullOrEmpty(tag) ? "ANY" : tag)}'</b>");
        
        if (containers == null)
        {
            Debug.LogError("[Spawner] FATAL: The 'containers' dictionary is NULL.");
            return lostAndFound;
        }
        if (debug) print($"Containers size is {containers.Count}");
        var eligibleEntries = new List<KeyValuePair<GameObject, List<string>>>();

        foreach (var kvp in containers)
        {
            if (kvp.Key == null)
            {
                Debug.LogWarning("[Spawner] Found a destroyed/null GameObject in dictionary keys. Skipping.");
                continue;
            }
            
            if (string.IsNullOrEmpty(tag) || kvp.Value.Contains(tag))
            {
                eligibleEntries.Add(kvp);
            }
        }
        if (debug) Debug.Log($"[Spawner] Found {eligibleEntries.Count} objects matching tag {tag}.");
        List<IContainer> conts = new List<IContainer>();

        foreach (var kvp in eligibleEntries)
        {
            if (kvp.Key != null && kvp.Key.TryGetComponent(out IContainer container))
            {
                conts.Add(container);
            }
        }
        if (debug) Debug.Log($"[Spawner] Collected {conts.Count} valid IContainer scripts.");
        
        List<IContainer> validConts = new List<IContainer>();

        foreach (var c in conts)
        {
            if (c.Capacity > 0)
            {
                validConts.Add(c);
            }
        }

        IContainer chosenContainer;

        if (conts.Count <= 0)
        {
            Debug.LogError("Couldn't find spawn for request with tag -> " + tag);
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
        
        Transform result = chosenContainer.GetSpawnPoint();
        if (result)
        {
            return result;
        }

        return lostAndFound;
    }
}
