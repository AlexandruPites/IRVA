using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Random = UnityEngine.Random;

public class PipeGridManager : MonoBehaviour
{
    public List<GameObject> tiles;
    public LinearMapping linearMappingAngular;
    public LinearMapping linearMappingVeritcal;
    public LinearMapping linearMappingHorizontal;
    private int _selectedRow;
    private int _selectedColumn;
    public Hand dummyHand; // dont ask
    private Quaternion _selectedBaseRotation;
    private float _baseValveRotation;
    public GameObject junctionPipe;
    public GameObject straightPipe;
    
    private void Awake()
    {
        _selectedRow = 2;
        _selectedColumn = 2;
        _baseValveRotation = 0f;
    }

    private void Start()
    {
        GenerateGrid();
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
            _selectedBaseRotation = selectedTile.transform.localRotation;
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
            //Debug.Log("Rot trigger");
            GameObject selectedTile = tiles[5 * _selectedRow + _selectedColumn];
            var newRotation = Quaternion.Euler(0f, rotationIndex * 90f, 0f) * _selectedBaseRotation;
            _selectedBaseRotation = newRotation;
            
            var pt = selectedTile.GetComponent<PipeTile>();
            pt.rotateTile(rotationIndex);
            
            selectedTile.transform.localRotation = newRotation;
            _baseValveRotation = linearMappingAngular.value;
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

    private void GenerateGrid()
    {
        foreach (var tile in tiles)
        {
            // Debug.Log($"{tile.transform.childCount}");
            for (int i = tile.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(tile.transform.GetChild(i).gameObject);
            }

            int type = Random.Range(0, 2);
            int rotation = Random.Range(0, 4);
            GameObject pipe;
            PipeTile pt;

            if (type == 0)
            {
                pipe = Instantiate(straightPipe, tile.transform, false);
                pt = tile.GetComponent<PipeTile>();
                pt.initializeTile(1, 3);
            }
            else
            {
                pipe = Instantiate(junctionPipe, tile.transform, false);
                pt = tile.GetComponent<PipeTile>();
                pt.initializeTile(0, 1);
            }
            pipe.transform.localPosition = Vector3.zero;
            pipe.transform.localRotation = Quaternion.Euler(0f, 90f * rotation, 0f);
            pipe.transform.localScale = Vector3.one;
            pt.rotateTile(rotation);
            
        }
        
    }
}
