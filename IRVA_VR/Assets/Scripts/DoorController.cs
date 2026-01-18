using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private int maxCounter = 6;

    private List<bool> isFinished = new List<bool>(new bool[6]);
    private int counter = 0;

    [SerializeField] private List<Key> endKeys;
    
    
    private void OnEnable()
    {
        EventBus.Instance.Subscribe<LockUnlocked>(OnLockUnlocked);
        EventBus.Instance.Subscribe<PuzzleFinished>(OnPuzzleFinished);
    }

    private void OnDisable()
    {
        EventBus.Instance.Unsubscribe<LockUnlocked>(OnLockUnlocked);
        EventBus.Instance.Unsubscribe<PuzzleFinished>(OnPuzzleFinished);
    }
    
    private void OnLockUnlocked(LockUnlocked obj)
    {
        if (obj.lockTag.Equals("endFinalDoor"))
        {
            CheckCounter();
        }
    }
    
    private void OnPuzzleFinished(PuzzleFinished obj)
    {
        endKeys[obj.number - 1].transform.position = obj.position;
    }

    private void CheckCounter()
    {
        counter++;
        if (counter >= maxCounter)
        {
            gameObject.SetActive(false);
        }
    }
}
