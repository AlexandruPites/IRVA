using System;
using TMPro;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] public string puzzleTag;
    [SerializeField] private Material initialMaterial;
    [SerializeField] private MeshRenderer renderer;

    private void Start()
    {
        renderer.material = initialMaterial;
    }

    public void SetUpKey(string key, Material material)
    {
        puzzleTag = key + "_puzzle_3";
        renderer.material = material;
    }
}
