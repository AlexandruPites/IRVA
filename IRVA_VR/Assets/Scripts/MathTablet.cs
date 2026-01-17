using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MathTablet : MonoBehaviour
{
    [SerializeField] public int rows = 4;
    
    [Header("Positions")]
    [SerializeField] private List<Transform> leftPositions;
    [SerializeField] private List<Transform> symbolPositions;
    [SerializeField] private List<Transform> rightPositions;
    
    [Header("Symbols")]
    [SerializeField] public List<string> symbolNames;
    [SerializeField] private List<GameObject> symbolPrefabs;
    
    [Header("Shapes")]
    [SerializeField] public List<string> shapeNames;
    [SerializeField] private List<GameObject> shapePrefabs;
    
    [Header("Colors")]
    [SerializeField] public List<string> colorNames;
    [SerializeField] private List<Material> colorMaterials;
    
    private Dictionary<string, GameObject> shapeDict = new();
    private Dictionary<string, GameObject> symbolDict = new();
    private Dictionary<string, Material> colorDict = new();

    private List<GameObject> objects = new();

    private void Awake()
    {
        for (int i = 0; i < symbolNames.Count; i++)
        {
            symbolDict.Add(symbolNames[i], symbolPrefabs[i]);
        }
        
        for (int i = 0; i < shapeNames.Count; i++)
        {
            shapeDict.Add(shapeNames[i], shapePrefabs[i]);
        }
        
        for (int i = 0; i < colorNames.Count; i++)
        {
            colorDict.Add(colorNames[i], colorMaterials[i]);
        }
    }

    public void DestroyAllObjects()
    {
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
        
        objects.Clear();
    }

    public void SetUpMathTablet(List<RowData> data)
    {
        for (int i = 0; i < rows; i++)
        {
            GameObject left = Instantiate(shapeDict[data[i].leftShape], leftPositions[i]);
            left.transform.localScale = Vector3.one * 0.05f;
            left.GetComponentInChildren<MeshRenderer>().material = colorDict[data[i].leftColor];
            
            GameObject sym = Instantiate(symbolDict[data[i].symbol], symbolPositions[i]);
            sym.transform.localScale = Vector3.one * 0.05f;
            
            GameObject right = Instantiate(shapeDict[data[i].rightShape], rightPositions[i]);
            right.transform.localScale = Vector3.one * 0.05f;
            right.GetComponentInChildren<MeshRenderer>().material = colorDict[data[i].rightColor];
            
            objects.Add(left);
            objects.Add(sym);
            objects.Add(right);
        }
    }
}
