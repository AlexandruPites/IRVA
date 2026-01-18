using System;
using TMPro;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] public string puzzleTag;
    [SerializeField] private MeshRenderer renderer;

    public void SetUpKey(string key, Color color)
    {
        puzzleTag = key + "_puzzle_3";
        renderer.material.color = color;
    }
    
    public void SetUpKeyP4(string key)
    {
        puzzleTag = key + "_puzzle_4";
    }
}
