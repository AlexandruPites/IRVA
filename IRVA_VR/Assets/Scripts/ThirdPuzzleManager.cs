using UnityEngine;

public class ThirdPuzzleManager : MonoBehaviour
{
    public void CorrectSolution()
    {
        EventBus.Instance.Broadcast(new ThirdPuzzleFinished());
    }
}
