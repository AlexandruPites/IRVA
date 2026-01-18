using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class ThirdPuzzleManager : MonoBehaviour
{
    [SerializeField] private List<LockController> locks;
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private Tablet tablet;
    [SerializeField] private string spawnPointTag = "puzzle_3";
    [SerializeField] private List<Color> allColors;
    [SerializeField] private Transform endKeySpawn;

    private Dictionary<string, Color> colorDict = new();
    private List<string> lockTags = new();

    private int locksUnlockedCount = 0;
    private int maxCount;

    private void Start()
    {
        EventBus.Instance.Subscribe<LockUnlocked>(OnUnlock);
        maxCount = locks.Count;

        foreach (var color in allColors)
        {
            colorDict.Add(color.ToString(), color);
        }
        
        GeneratePuzzle();
    }

    private void GeneratePuzzle()
    {
        Dictionary<string, string> colorMatches = new Dictionary<string, string>();
        List<string> initialColors = new List<string>(colorDict.Keys);
        var solutions = new List<string>(colorDict.Keys);
        solutions = solutions.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < initialColors.Count; i++)
        {
            colorMatches.Add(initialColors[i], solutions[i]);
        }
        List<Color> temp_locks = new List<Color>();
        List<Color> temp_keys = new List<Color>();

        for (int i = 0; i < locks.Count; i++)
        {
            List<string> colorMatchesKeys = new List<string>(colorMatches.Keys);
            string randomKey = colorMatchesKeys.GetRandomElement();
            string randomValue = colorMatches[randomKey];

            colorMatches.Remove(randomKey);
            
            Color m1 = colorDict[randomKey];
            Color m2 = colorDict[randomValue];

            GameObject key = SpawnerManager.Instance.YeetItem(keyPrefab, spawnPointTag);
            Vector3 parentScale = key.transform.parent.lossyScale;
            Vector3 newLocalScale = new Vector3(
                1f / parentScale.x,
                1f / parentScale.y,
                1f / parentScale.z
                );
            key.transform.localScale = newLocalScale;
            key.TryGetComponent(out Key component);
            component.SetUpKey(randomKey, m2);
            
            locks[i].SetUpLock(randomKey, m1);
            lockTags.Add(randomKey + "_puzzle_3");
            
            temp_locks.Add(m1);
            temp_keys.Add(m2);
        }

        foreach (var match in colorMatches)
        {
            Color m2 = colorDict[match.Value];
            GameObject key = SpawnerManager.Instance.YeetItem(keyPrefab, spawnPointTag);
            Vector3 parentScale = key.transform.parent.lossyScale;
            Vector3 newLocalScale = new Vector3(
                1f / parentScale.x,
                1f / parentScale.y,
                1f / parentScale.z
            );
            key.transform.localScale = newLocalScale;
            key.TryGetComponent(out Key component);
            component.SetUpKey("", m2);
        }
        
        tablet.SetUpTablet(temp_locks, temp_keys);

    }

    private void OnUnlock(LockUnlocked obj)
    {
        print(string.Join(',', lockTags));
        if (lockTags.Contains(obj.lockTag))
        {
            locksUnlockedCount++;

            lockTags.Remove(obj.lockTag);
            
            print($"Puzzle 3 Progress: {locksUnlockedCount} / {maxCount}");

            if (locksUnlockedCount >= maxCount)
            {
                CorrectSolution();
            }
        }
    }

    public void CorrectSolution()
    {
        EventBus.Instance.Broadcast(new PuzzleFinished(3, endKeySpawn.transform.position));
    }
}
