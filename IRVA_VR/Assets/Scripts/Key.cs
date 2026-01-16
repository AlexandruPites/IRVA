using System;
using TMPro;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] public string puzzleTag;
    [SerializeField] private MeshRenderer renderer;

    public void SetUpKey(string key, Material material)
    {
        puzzleTag = key + "_puzzle_3";
        renderer.material = material;
    }
    
    public void SetUpKey(string key)
    {
        puzzleTag = key + "_puzzle_4";
    }
}
