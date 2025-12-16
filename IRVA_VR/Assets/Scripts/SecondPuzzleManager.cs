using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SecondPuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject endButton;
    [SerializeField] private int codeLength = 4;
    [SerializeField] private TMP_Text solution;
    [SerializeField] private KeypadController keypad;
    
    private string code = "";

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
        if (obj.value.Equals(code))
        {
            StartCoroutine(keypad.DisplayColor(Color.green, 0.5f));
            endButton.SetActive(true);
        }
        else
        {
            StartCoroutine(keypad.DisplayColor(Color.red, 0.5f));
        }
    }

    private void Start()
    {
        endButton.SetActive(false);
        for (int i = 0; i < codeLength; i++)
        {
            code += Random.Range(0, 10).ToString();
        }

        solution.text = code;
    }

    public void CorrectSolution()
    {
        EventBus.Instance.Broadcast(new SecondPuzzleFinished());
    }
}
