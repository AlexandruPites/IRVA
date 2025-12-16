using UnityEngine;
using Valve.VR.InteractionSystem;

public class StickController : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPositionHorizontal;
    public Transform endPositionVertical;
    public LinearMapping linearMappingHorizontal;
    public GameObject goal;
    private float horizontalGoal;
    private float verticalGoal;
    private int progress;
    private int progressGoal;
    private float hGoal;
    private float vGoal;
    
    public LinearMapping linearMappingVertical;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateSolution();
        progress = 0;
        progressGoal = 5;
    }

    void GenerateSolution()
    {
        horizontalGoal = Random.Range(0.0f, 1.0f);
        verticalGoal = Random.Range(0.0f, 1.0f);
        var pos = goal.transform.position;
        var hLerp = Vector3.Lerp( startPosition.position, endPositionHorizontal.position, horizontalGoal );
        var vLerp = Vector3.Lerp( startPosition.position, endPositionVertical.position, verticalGoal );

        pos.z = hLerp.z;
        pos.x = vLerp.x;
        goal.transform.position = pos;

        hGoal = horizontalGoal;
        vGoal = verticalGoal;
    }
    // Update is called once per frame
    void Update()
    {
        var horizontalLerp = Vector3.Lerp( startPosition.position, endPositionHorizontal.position, linearMappingHorizontal.value );
        var verticalLerp = Vector3.Lerp( startPosition.position, endPositionVertical.position, linearMappingVertical.value );
        horizontalLerp.x = verticalLerp.x;
        transform.position = horizontalLerp;

        // Debug.Log($"HGoal - {hGoal}, HCurrent - {linearMappingHorizontal.value}, Diff - {Mathf.Abs(linearMappingHorizontal.value - hGoal)}");
        // Debug.Log($"VGoal - {vGoal}, VCurrent - {linearMappingVertical.value}, Diff - {Mathf.Abs(linearMappingVertical.value - vGoal)}");
        if (Mathf.Abs(linearMappingHorizontal.value - hGoal) < 0.015f &&
            Mathf.Abs(linearMappingVertical.value - vGoal) < 0.015f)
        {
            SolutionCorrect();
        }
    }

    void SolutionCorrect()
    {
        GenerateSolution();
        progress++;
        if (progress == progressGoal)
        {
            PuzzleComplete();
        }
    }

    void PuzzleComplete()
    {
        EventBus.Instance.Broadcast(new SixthPuzzleFinished());
        goal.SetActive(false);
    }
}
