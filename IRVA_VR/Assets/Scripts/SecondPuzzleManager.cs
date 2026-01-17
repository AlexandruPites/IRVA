using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.XR.OpenVR;
using UnityEngine;
using Random = UnityEngine.Random;

public struct RowData
{
    public float valueLeft;
    public string leftShape, leftColor;
    public string symbol;
    public float valueRight;
    public string rightShape, rightColor;

    public RowData(float valueLeft, string leftShape, string leftColor, string symbol, float valueRight,string rightShape, string rightColor)
    {
        this.valueLeft = valueLeft;
        this.leftShape = leftShape;
        this.leftColor = leftColor;
        this.symbol = symbol;
        this.valueRight = valueRight;
        this.rightShape = rightShape;
        this.rightColor = rightColor;
    }
}
public class SecondPuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject endButton;
    [SerializeField] private int codeLength = 4;
    [SerializeField] private TMP_Text solution;
    [SerializeField] private KeypadController keypad;
    [SerializeField] private MathTablet mathTablet;
    [SerializeField] private Cabinet cabinet;
    [SerializeField] private int maxTries = 3;
    
    [Header("Shapes")]
    [SerializeField] private List<string> shapeNames;
    [SerializeField] private List<MathShapes> mathShapePrefabs;
    
    [Header("Colors")]
    [SerializeField] private List<string> colorNames;
    [SerializeField] private List<Material> colorMaterials;
    
    private Dictionary<string, MathShapes> shapeDict = new();
    private Dictionary<string, Material> colorDict = new();
    
    private List<string> shapes, symbols, colors;

    private Dictionary<(string shape, string color), bool> combinations = new();
    
    private string code = "";

    private List<MathShapes> currentShapes = new();

    private void Awake()
    {
        for (int i = 0; i < shapeNames.Count; i++)
        {
            shapeDict.Add(shapeNames[i], mathShapePrefabs[i]);
        }
        
        for (int i = 0; i < colorNames.Count; i++)
        {
            colorDict.Add(colorNames[i], colorMaterials[i]);
        }
    }

    private void OnEnable()
    {
        EventBus.Instance.Subscribe<SecondPuzzleCodeEntered>(OnCodeEntered);
    }

    private void OnDisable()
    {
        EventBus.Instance.Unsubscribe<SecondPuzzleCodeEntered>(OnCodeEntered);
    }

    private void OnCodeEntered(SecondPuzzleCodeEntered obj)
    {
        StartCoroutine(keypad.DisplayDigitFeedback(obj.value, code, 0.5f));
        
        if (obj.value.Equals(code))
        {
            endButton.SetActive(true);
        }
        else
        {
            maxTries--;

            if (maxTries <= 0)
            {
                GenerateCode();
            }
        }
    }

    private void Start()
    {
        print("Second puzzle Start");
        endButton.SetActive(false);
        
        shapes = mathTablet.shapeNames;
        symbols = mathTablet.symbolNames;
        colors = mathTablet.colorNames;
        
        code = GenerateCode();
        print($"Code is {code}");
        
        solution.text = code;
    }

    private string GenerateCode()
    {
        print(currentShapes.Count);
        foreach (var shape in currentShapes)
        {
            print($"Destroying {shape}");
            Destroy(shape.gameObject);
        }
        
        mathTablet.DestroyAllObjects();
        
        currentShapes.Clear();
        
        GenerateCombinations();
        string generatedCode = "";
        List<RowData> data = new List<RowData>();

        for (int i = 0; i < mathTablet.rows; i++)
        {
            string pickedSymbol = symbols.GetRandomElement();
            var (a, b, result) = getNumbers(pickedSymbol);

            var randomKey = GetRandomAndRemove();
            string leftColor = randomKey.color;
            string leftShape = randomKey.shape;

            randomKey = GetRandomAndRemove();
            string rightColor = randomKey.color;
            string rightShape = randomKey.shape;

            a = applyShapeAndColor(a, leftShape, leftColor);
            b = applyShapeAndColor(b, rightShape, rightColor);
            
            data.Add(new RowData(a, leftShape, leftColor, pickedSymbol, b, rightShape, rightColor));
            generatedCode += result.ToString();
        }
        
        mathTablet.SetUpMathTablet(data);
        SpawnShapes(data);
        
        return generatedCode;
    }

    void SpawnShapes(List<RowData> data)
    {
        foreach (var row in data)
        {
            Transform spawnPoint = cabinet.drawers[0].root;
            
            MathShapes left = Instantiate(shapeDict[row.leftShape], spawnPoint);
            left.SetUpShape(row.valueLeft.ToString(), colorDict[row.leftColor]);
            left.transform.localScale = Vector3.one * 0.1f;
            left.transform.position = spawnPoint.position;//+ (Random.insideUnitSphere * 0.3f);
            
            MathShapes right = Instantiate(shapeDict[row.rightShape], spawnPoint);
            right.SetUpShape(row.valueRight.ToString(), colorDict[row.rightColor]);
            right.transform.localScale = Vector3.one * 0.1f;
            right.transform.position = spawnPoint.position;//+ (Random.insideUnitSphere * 0.3f);
            
            currentShapes.Add(left);
            currentShapes.Add(right);
        }
    }

    void GenerateCombinations()
    {
        combinations.Clear();

        foreach (var shape in shapes)
        {
            foreach (var color in colors)
            {
                combinations.Add((shape, color), true);
            }
        }
    }

    (string shape, string color) GetRandomAndRemove()
    {
        var keyList = new List<(string shape, string color)>(combinations.Keys);
        var randomKey = keyList.GetRandomElement();

        combinations.Remove(randomKey);
        return randomKey;
    }

    private (float a, float b, float result) getNumbers(string symbol)
    {
        int a, b, result;
        switch (symbol)
        {
            case "plus":
                a = Random.Range(0, 10);
                b = Random.Range(0, 10 - a);
                result = a + b;
                return (a, b, result);
            case "minus":
                a = Random.Range(0, 10);
                b = Random.Range(0, a);
                result = a - b;
                return (a, b, result);
            case "multiply":
                a = Random.Range(1, 10);
                b = Random.Range(0, (10 / a) + 1);
                result = a * b;
                return (a, b, result);
            case "divide":
                a = Random.Range(1, 10);
                b = Random.Range(1, 10);
                int c = Random.Range(0, 100) % 2 == 0 ? a : b;
                result = a * b / c;
                return (a * b, c, result);
                
            default:
                Debug.LogError("Symbol name is wrong");
                return (-1, -1, -1);
        }
    }

    private float applyShapeAndColor(float num, string shape, string color)
    {
        float nr = num;
        
        switch (color)
        {
            case "red":
                nr = nr + 5;
                break;
            case "green":
                nr = nr + 10;
                break;
            case "blue":
                break;
            default:
                nr = -1;
                break;
        }
        
        switch (shape)
        {       
            case "cube":
                nr = nr * 2;
                break;
            case "sphere":
                nr = nr * nr;
                break;
            case "capsule":
                nr = nr / 2.0f;
                break;
            default:
                nr = -1;
                break;
        }

        return nr;
    }

    public void CorrectSolution()
    {
        EventBus.Instance.Broadcast(new SecondPuzzleFinished());
    }
    
    
}
