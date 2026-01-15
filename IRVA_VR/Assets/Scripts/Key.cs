using System;
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
}
