using System;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPuzzleManager : MonoBehaviour
{
    [SerializeField] private List<string> lockTags;
    
    [Header("Dictionary Emulation :( 'cause Unity doesn't support Serializable Dicts in inspector")]
    [SerializeField] private List<string> keys;
    [SerializeField] private List<Material> values;

    private Dictionary<string, Material> colorDict;

    private int locksUnlockedCount = 0;
    private int maxCount;

    private void Start()
    {
        EventBus.Instance.Subscribe<LockUnlocked>(OnUnlock);
        maxCount = lockTags.Count;

        if (keys.Count == values.Count)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                colorDict.Add(keys[i], values[i]);
            }
        }
        else
        {
            Debug.LogError("ColorDictInspectorEmulationListsSizeMismatch");
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
