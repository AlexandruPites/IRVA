using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class ThirdPuzzleManager : MonoBehaviour
{
    [SerializeField] private List<LockController> locks;
    [SerializeField] private List<Key> puzzleKeys;
    [SerializeField] private Tablet tablet;
    [SerializeField] private List<Cabinet> cabinets;
    
    [Header("Dictionary Emulation :( 'cause Unity doesn't support Serializable Dicts in inspector")]
    [SerializeField] private List<string> dictKeys;
    [SerializeField] private List<Material> dictValues;

    private Dictionary<string, Material> colorDict = new();
    private List<string> lockTags = new();

    private int locksUnlockedCount = 0;
    private int maxCount;

    private void Start()
    {
        EventBus.Instance.Subscribe<LockUnlocked>(OnUnlock);
        foreach (var myLock in locks)
        {
            lockTags.Add(myLock.puzzleTag);
        }
        maxCount = lockTags.Count;

        if (dictKeys.Count == dictValues.Count)
        {
            for (int i = 0; i < dictKeys.Count; i++)
            {
                colorDict.Add(dictKeys[i], dictValues[i]);
            }
        }
        else
        {
            Debug.LogError("ColorDictInspectorEmulationListsSizeMismatch");
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
        
        List<Material> temp_locks = new List<Material>();
        List<Material> temp_keys = new List<Material>();
        int index = 0;
        foreach (var match in colorMatches)
        {
            Material m1 = colorDict[match.Key];
            Material m2 = colorDict[match.Value];
            
            locks[index].SetUpLock(match.Key, m1);
            puzzleKeys[index].SetUpKey(match.Key, m2);
            
            temp_locks.Add(m1);
            temp_keys.Add(m2);
            index++;
        }
        
        tablet.SetUpTablet(temp_locks, temp_keys);

        for (int i = 0; i < puzzleKeys.Count; i++)
        {
            int x = Random.Range(0, puzzleKeys.Count);
            int y = Random.Range(0, cabinets[i].drawers.Count);

            puzzleKeys[i].transform.position = cabinets[x].drawers[y].root.position;
        }

    }

    private void OnUnlock(LockUnlocked obj)
    {
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
        EventBus.Instance.Broadcast(new ThirdPuzzleFinished());
    }
}
