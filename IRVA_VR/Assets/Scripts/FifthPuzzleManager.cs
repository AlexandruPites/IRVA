using UnityEngine;

public class FifthPuzzleManager : MonoBehaviour
{
    public void CorrectSolution()
    {
        EventBus.Instance.Broadcast(new FifthPuzzleFinished());
    }
}
