using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> keyholes;

    [SerializeField] private Material finished;

    [SerializeField] private int maxCounter = 6;

    private List<bool> isFinished = new List<bool>(new bool[6]);
    private int counter = 0;
    
    
    
    private void OnEnable()
    {
        EventBus.Instance.Subscribe<FirstPuzzleFinished>(OnFirst);
        EventBus.Instance.Subscribe<SecondPuzzleFinished>(OnSecond);
        EventBus.Instance.Subscribe<ThirdPuzzleFinished>(OnThird);
        EventBus.Instance.Subscribe<FourthPuzzleFinished>(OnFourth);
        EventBus.Instance.Subscribe<FifthPuzzleFinished>(OnFifth);
        EventBus.Instance.Subscribe<SixthPuzzleFinished>(OnSixth);
    }

    private void OnDisable()
    {
        EventBus.Instance.Unsubscribe<FirstPuzzleFinished>(OnFirst);
        EventBus.Instance.Unsubscribe<SecondPuzzleFinished>(OnSecond);
        EventBus.Instance.Unsubscribe<ThirdPuzzleFinished>(OnThird);
        EventBus.Instance.Unsubscribe<FourthPuzzleFinished>(OnFourth);
        EventBus.Instance.Unsubscribe<FifthPuzzleFinished>(OnFifth);
        EventBus.Instance.Unsubscribe<SixthPuzzleFinished>(OnSixth);
    }

    private void OnFirst(FirstPuzzleFinished e)
    {
        if (!isFinished[0])
        {
            CheckCounter();
        }

        isFinished[0] = true;
        keyholes[0].material = finished;
    }

    private void OnSecond(SecondPuzzleFinished e)
    {
        if (!isFinished[1])
        {
            CheckCounter();
        }
        
        isFinished[1] = true;
        keyholes[1].material = finished;
    }

    private void OnThird(ThirdPuzzleFinished e)
    {
        if (!isFinished[2])
        {
            CheckCounter();
        }
        
        isFinished[2] = true;
        keyholes[2].material = finished;
    }

    private void OnFourth(FourthPuzzleFinished e)
    {
        if (!isFinished[3])
        {
            CheckCounter();
        }
        
        isFinished[3] = true;
        keyholes[3].material = finished;
    }

    private void OnFifth(FifthPuzzleFinished e)
    {
        if (!isFinished[4])
        {
            CheckCounter();
        }
        
        isFinished[4] = true;
        keyholes[4].material = finished;
    }

    private void OnSixth(SixthPuzzleFinished e)
    {
        if (!isFinished[5])
        {
            CheckCounter();
        }
        
        isFinished[5] = true;
        keyholes[5].material = finished;
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
