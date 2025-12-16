using UnityEngine;

public class FourthPuzzleManager : MonoBehaviour
{
    public void CorrectSolution()
    {
        EventBus.Instance.Broadcast(new FourthPuzzleFinished());
    }
}
