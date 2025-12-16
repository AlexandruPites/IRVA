using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FirstPuzzleManager : MonoBehaviour
{
    [SerializeField] private List<FirstPuzzleButton> solutionButtons;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private float intervalDuration = 0.2f;
    
    public Material correct, wrong, simon, neutral, active;

    private List<int> solution;
    private int currentSize = 3;
    private int solutionCurrentIndex = 0;
    private bool finished = false;
    
    private void OnEnable()
    {
        EventBus.Instance.Subscribe<FirstPuzzleButtonPressed>(OnFirstPuzzleButtonPressed);
    }

    private void OnDisable()
    {
        EventBus.Instance.Unsubscribe<FirstPuzzleButtonPressed>(OnFirstPuzzleButtonPressed);
    }

    private void Start()
    {
        foreach (var btn in solutionButtons)
        {
            btn.ChangeMaterial(neutral);
        }
        solution = GenerateSolution(currentSize);
    }

    private List<int> GenerateSolution(int size)
    {
        int previous = -1, beforePrevious = -1;

        List<int> sol = new List<int>();

        for (int i = 0; i < size; i++)
        {
            int nr = Random.Range(0, 3);
            while (previous == beforePrevious && nr == previous)
            {
                nr = Random.Range(0, 3);
            }

            beforePrevious = previous;
            previous = nr;
            sol.Add(nr);
        }

        return sol;
    }
    
    private void OnFirstPuzzleButtonPressed(FirstPuzzleButtonPressed obj)
    {
        if (solutionCurrentIndex >= solution.Count)
        {
            return;
        }
        
        print(obj.Number + " " + solution[solutionCurrentIndex]);
        if (obj.Number == solution[solutionCurrentIndex])
        {
            solutionCurrentIndex++;

            if (solutionCurrentIndex == currentSize)
            {
                CorrectSolution();
            }
        }
        else
        {
            ResetSolution();
            StartCoroutine(BlinkFailSequence());
        }
        
    }

    private void CorrectSolution()
    {
        if (currentSize == 7)
        {
            EventBus.Instance.Broadcast(new FirstPuzzleFinished());
            finished = true;
        }

        StartCoroutine(CorrectPassSequence());
    }

    public void ShowSolution()
    {
        solutionCurrentIndex = 0;
        if (!finished)
        {
            

            StartCoroutine(ShowSequence());
        }
    }

    private void ResetSolution()
    {
        solutionCurrentIndex = 0;
        foreach (var btn in solutionButtons)
        {
            btn.isEnabled = false;
        }
    }
    
    private IEnumerator ShowSequence()
    {
        foreach (int buttonIndex in solution)
        {
            if (buttonIndex >= solutionButtons.Count) continue;
            
            solutionButtons[buttonIndex].ChangeMaterial(simon);

            yield return new WaitForSeconds(flashDuration);
            
            solutionButtons[buttonIndex].ChangeMaterial(neutral);
            
            yield return new WaitForSeconds(intervalDuration);

        }
        
        foreach (var btn in solutionButtons)
        {
            btn.isEnabled = true;
        }
    }
    
    private IEnumerator BlinkFailSequence()
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (var btn in solutionButtons)
            {
                btn.ChangeMaterial(wrong);
            }
        
            yield return new WaitForSeconds(0.25f);

            foreach (var btn in solutionButtons)
            {
                btn.ChangeMaterial(neutral);
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
    
    private IEnumerator CorrectPassSequence()
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (var btn in solutionButtons)
            {
                btn.ChangeMaterial(correct);
                btn.isEnabled = false;
            }
        
            yield return new WaitForSeconds(0.25f);

            foreach (var btn in solutionButtons)
            {
                btn.ChangeMaterial(neutral);
            }

            yield return new WaitForSeconds(0.25f);
        }
        
        solutionCurrentIndex = 0;
        currentSize += 2;
        solution = GenerateSolution(currentSize);
    }
    
}
