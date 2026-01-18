using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Random = UnityEngine.Random;

public class PipeGridManager : MonoBehaviour
{
    public List<GameObject> tiles;
    public List<GameObject> outerTiles;
    public LinearMapping linearMappingAngular;
    public LinearMapping linearMappingVeritcal;
    public LinearMapping linearMappingHorizontal;
    private int _selectedRow;
    private int _selectedColumn;
    public Hand dummyHand; // dont ask
    private float _baseValveRotation;
    public GameObject junctionPipe;
    public GameObject straightPipe;
    public Material NormalMaterial;
    public Material CorrectMaterial;
    private Dictionary<(int, int), GameObject> getOuterTile;
    private (int row, int column) startingPos;
    private (int row, int column) goalPos;
    private List<GameObject> pipePrefabs;
    public Material sourceMaterial;
    public Material goalMaterial;
    
    private void Awake()
    {
        getOuterTile = new Dictionary<(int, int), GameObject>();
        _selectedRow = 2;
        _selectedColumn = 2;
        _baseValveRotation = 0f;

        for (int i = 0; i < 5; i++)
        {
            getOuterTile.Add((i, -1), outerTiles[i]);
            getOuterTile.Add((i, 5), outerTiles[i + 5]);
            getOuterTile.Add((-1, i), outerTiles[i + 10]);
            getOuterTile.Add((5, i), outerTiles[i + 15]);
        }

        pipePrefabs = new List<GameObject> { straightPipe, junctionPipe };
    }

    private void Start()
    {
        GenerateRandomGrid();
        GenerateSolution();
        ShowOuterTile(startingPos, sourceMaterial);
        ShowOuterTile(goalPos, goalMaterial);
        VerifySolution(startingPos.row, startingPos.column + 1, 1, goalPos);
        
    }

    private void Update()
    {
        var row = GetClosestIndexInRange(5, linearMappingVeritcal.value);
        var column = GetClosestIndexInRange(5, linearMappingHorizontal.value);
        if (_selectedRow != row || _selectedColumn != column)
        {
            DeHighlightPipe();
            _selectedRow = row;
            _selectedColumn = column;
            HighlightPipe();
            
            //Debug.Log("Switch trigger");
            GameObject selectedTile = tiles[5 * _selectedRow + _selectedColumn];
            _baseValveRotation = linearMappingAngular.value;
        }

        var relative_rot = _baseValveRotation - linearMappingAngular.value;
        if (relative_rot < 0)
        {
            relative_rot += 1;
        }
        
        var rotationIndex = GetClosestIndexInRange(5, relative_rot);
        if (Mathf.Abs(relative_rot) > 0.25f && Mathf.Abs(relative_rot) < 0.75f)
        {
            GameObject selectedTile = tiles[5 * _selectedRow + _selectedColumn];
            var pt = selectedTile.GetComponent<PipeTile>();
            pt.rotateTile(rotationIndex);
            
            _baseValveRotation = linearMappingAngular.value;
            
            ClearColors();
            VerifySolution(startingPos.row, startingPos.column + 1, 1, goalPos);
        }
    }

    private void ClearColors()
    {
        foreach (var tile in tiles)
        {
            ChangeTileMaterial(tile, NormalMaterial);
        }
    }

    private int GetClosestIndexInRange(int segmentCount, float linearMapping)
    {
        float step = 1f / (float)(segmentCount - 1);
        float pos = 0;
        float min_dist = Mathf.Abs(linearMapping - pos);
        int closest = 0;
        
        for (int i = 1; i < segmentCount; i++)
        {
            pos += step;
            float current_distance = Mathf.Abs(linearMapping - pos);
            if (current_distance < min_dist)
            {
                min_dist = current_distance;
                closest = i;
            }
        }

        return closest;
    }
    

    void HighlightPipe()
    {
       GameObject selectedTile = tiles[5 * _selectedRow + _selectedColumn];
       var highlight = selectedTile.GetComponent<Interactable>();
       highlight.HighlightOverrride(dummyHand);
    }

    void DeHighlightPipe()
    {
        GameObject selectedTile = tiles[5 * _selectedRow + _selectedColumn];
        var highlight = selectedTile.GetComponent<Interactable>();
        highlight.DeHighlightOverrride(dummyHand);
    }

    private void GenerateRandomGrid()
    {
        foreach (var tile in tiles)
        {
            
            int type = Random.Range(0, 2);
            int rotation = Random.Range(0, 4);

            var pt = tile.GetComponent<PipeTile>();
            pt.SpawnChild(pipePrefabs, type, rotation);
        }
        
    }

    private bool VerifySolution(int row, int column, int entryDirection, (int x, int y) goal)
    {

        if (row == goal.x && column == goal.y)
        {
            EventBus.Instance.Broadcast(new SixthPuzzleFinished());
            return true;
        }

        if (row < 0 || row > 4 || column < 0 || column > 4)
        {
            return false;
        }
        
        var tile = tiles[5 * row + column];
        var pt = tile.GetComponent<PipeTile>();
        
        int requiredDirection = (entryDirection + 2) % 4;
        bool canEnter = false;
        
        foreach (var direction in pt.directions)
        {
            if (direction == requiredDirection)
            {
                canEnter = true;
            }
        }

        if (!canEnter)
        {
            return false;
        }
        
        ChangeTileMaterial(tile, CorrectMaterial);
        
        foreach (var direction in pt.directions)
        {
            if (direction != requiredDirection)
            {
                var offsets = PipeTile.Offsets[direction];
                int nextRow = row + offsets.x;
                int nextColumn = column + offsets.y;
                return VerifySolution(nextRow, nextColumn, direction, goal);
            }
        }

        return false;
    }

    private void ChangeTileMaterial(GameObject tile, Material material)
    {
        var existingRenderers = tile.GetComponentsInChildren<MeshRenderer>();
        
        foreach (var renderer in existingRenderers)
        {
            renderer.material = material;
        }
    }

    private void GenerateSolution()
    {
        List<(int, int)> visitedTiles = new List<(int, int)>();
        startingPos = (Random.Range(0, 5), -1);
        var currentPositon = startingPos;
        currentPositon.column++;
        int outgoingDirection = 1;
        int length = 0;
        while (true)
        {
            Debug.Log($"Position - {currentPositon.row}, {currentPositon.column}, Direction - {outgoingDirection}");
            if (currentPositon.row < 0 || currentPositon.row > 4 || currentPositon.column < 0 ||
                currentPositon.column > 4)
            {
                goalPos = currentPositon;
                break;
            }
            
            int incomingDirection = (outgoingDirection + 2) % 4;
            var availableDirections = new List<int> { 0, 1, 2, 3 };
            availableDirections.Remove(incomingDirection);

            for (int i = 0; i < availableDirections.Count; i++)
            {
                var pos = (currentPositon.row + PipeTile.Offsets[availableDirections[i]].x, currentPositon.column + PipeTile.Offsets[availableDirections[i]].y);

                if (visitedTiles.Contains(pos))

                {

                    availableDirections.RemoveAt(i);
                    i--;

                }
            }
            // Debug.Log("Directions: " + string.Join(", ", availableDirections));

            if (availableDirections.Count == 0)
            {
                GenerateSolution();
            }
            
            int newDirection = availableDirections.GetRandomElement();
            
            GameObject selectedTile = tiles[5 * currentPositon.row + currentPositon.column];
            var pt = selectedTile.GetComponent<PipeTile>();
            
            pt.SpawnChild(pipePrefabs, pt.GetTypeFromDesiredDirection(incomingDirection, newDirection), Random.Range(0, 4));
            
            // pt.SpawnChildToDirection(pipePrefabs, incomingDirection, newDirection);
            visitedTiles.Add(currentPositon);
            
            currentPositon = (currentPositon.row + PipeTile.Offsets[newDirection].x,
                currentPositon.column + PipeTile.Offsets[newDirection].y);
            
            outgoingDirection = newDirection;
            
            if (visitedTiles.Contains(currentPositon))
            {
                //GenerateSolution();
                break;
            }

            length++;
        }

        if (length < 10)
        {
            GenerateSolution();
        }
    }

    void ShowOuterTile((int row, int column) tileLocation, Material material)
    {
        var tile = getOuterTile[tileLocation];

        for (int i = 0; i < tile.transform.childCount; i++)
        {
            tile.transform.GetChild(i).gameObject.SetActive(true);
        }
        
        ChangeTileMaterial(tile, material);
    }
}
