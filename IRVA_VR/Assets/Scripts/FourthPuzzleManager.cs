using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class FourthPuzzleManager : MonoBehaviour
{
    public void CorrectSolution()
    {
        EventBus.Instance.Broadcast(new FifthPuzzleFinished());
    }
}
