using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class StickController : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPositionHorizontal;
    public Transform endPositionVertical;
    public LinearMapping linearMappingHorizontal;
    public LinearMapping linearMappingVertical;
    public GameObject goal;
    public Material normalMat;
    public Material correctMat;
    public List<GameObject> progressBar;
    
    [Header("Timer Settings")]
    public float timeLimit = 10f; 
    private float currentTime;
    private bool isTimerRunning = false;
    
    // To track the starting state to detect movement
    private float initialH;
    private float initialV;

    private float horizontalGoal;
    private float verticalGoal;
    private int progress;
    private int progressGoal;
    private float hGoal;
    private float vGoal;

    void Start()
    {
        progressGoal = 5;
        ResetPuzzle(); 
    }

    void ResetPuzzle()
    {
        progress = 0;
        isTimerRunning = false;
        currentTime = timeLimit;
        
        // Capture initial positions to detect when the player starts moving them
        initialH = linearMappingHorizontal.value;
        initialV = linearMappingVertical.value;

        UpdateProgressBar(); // Clear the visual bar
        GenerateSolution();
        Debug.Log("Puzzle Reset! Waiting for interaction...");
    }

    void GenerateSolution()
    {
        // We reset the time but keep isTimerRunning based on whether it was already active
        currentTime = timeLimit;

        horizontalGoal = Random.Range(0.0f, 1.0f);
        verticalGoal = Random.Range(0.0f, 1.0f);
        var pos = goal.transform.position;
        var hLerp = Vector3.Lerp(startPosition.position, endPositionHorizontal.position, horizontalGoal);
        var vLerp = Vector3.Lerp(startPosition.position, endPositionVertical.position, verticalGoal);

        pos.z = hLerp.z;
        pos.x = vLerp.x;
        goal.transform.position = pos;

        hGoal = horizontalGoal;
        vGoal = verticalGoal;
    }

    void Update()
    {
        // 1. Detection Logic: Start timer if the mapping values change from their initial state
        if (!isTimerRunning)
        {
            if (Mathf.Abs(linearMappingHorizontal.value - initialH) > 0.001f || 
                Mathf.Abs(linearMappingVertical.value - initialV) > 0.001f)
            {
                isTimerRunning = true;
            }
        }

        // 2. Timer Logic
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                ResetPuzzle();
                return; 
            }
        }

        // 3. Movement Logic
        var horizontalLerp = Vector3.Lerp(startPosition.position, endPositionHorizontal.position, linearMappingHorizontal.value);
        var verticalLerp = Vector3.Lerp(startPosition.position, endPositionVertical.position, linearMappingVertical.value);
        horizontalLerp.x = verticalLerp.x;
        transform.position = horizontalLerp;

        // 4. Win Condition
        if (Mathf.Abs(linearMappingHorizontal.value - hGoal) < 0.015f &&
            Mathf.Abs(linearMappingVertical.value - vGoal) < 0.015f)
        {
            SolutionCorrect();
        }
    }

    void SolutionCorrect()
    {
        progress++;
        UpdateProgressBar();
        
        if (progress >= progressGoal)
        {
            PuzzleComplete();
        }
        else
        {
            // Optional: If you want the timer to pause again until the next move, 
            // set isTimerRunning = false; and re-capture initialH/V here.
            GenerateSolution();
        }
    }

    void PuzzleComplete()
    {
        EventBus.Instance.Broadcast(new FourthPuzzleFinished());
        goal.SetActive(false);
        this.enabled = false;
    }

    private void UpdateProgressBar()
    {
        // Set all to normal first
        foreach (var segment in progressBar)
        {
            ChangeMaterial(segment, normalMat);
        }

        // Set the completed ones to correct
        for (int i = 0; i < progress; i++)
        {
            if(i < progressBar.Count) // Safety check for list bounds
                ChangeMaterial(progressBar[i], correctMat);
        }
    }
    
    private void ChangeMaterial(GameObject obj, Material material)
    {
        if (obj == null) return;
        var renderer = obj.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }
}