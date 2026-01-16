using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class FourthPuzzleManager : MonoBehaviour
{
    public List<Key> keys;
    public LockController lok;
    public List<TMP_Text> hintTexts;
    public TMP_Text rulesText;
    private List<List<string>> _hints;
    private List<int> _solutions;
    private string _rules = "There will always be at least one tablet which displays only true statements.\n \n There will always be at least one tablet which displays only false statements.\n \n Only one tablet has the color of the correct key. The other 2 keys don't open the lock..";

    private void Start()
    {
        EventBus.Instance.Subscribe<LockUnlocked>(CorrectSolution);
        rulesText.text = _rules;
        
        _hints = new List<List<string>>();
        _solutions = new List<int>();
        
        _hints.Add(new List<string>
        {
            "A tablet next to this tablet has the correct color",
            "Both tablets next to this tablet have the correct color",
            "A tablet next to this tablet tells the truth"
        });
        _solutions.Add(1);
        
        _hints.Add(new List<string>
        {
            "A tablet with a false statement has the correct color",
            "The statement on the blue tablet is true",
            "The blue tablet has the correct color"
        });
        _solutions.Add(2);
        
        _hints.Add(new List<string>
        {
            "All three statements are false",
            "Two statements are false",
            "The correct color is on a tablet with a false statement"
        });
        _solutions.Add(1);
        
        _hints.Add(new List<string>
        {
            "The tablet next to this tablet is false",
            "A tablet next to this tablet is true",
            "The tablet next to this tablet has the correct color"
        });
        _solutions.Add(1);
        
        int solution_index = Random.Range(0, _solutions.Count);
        
        lok.SetUpLock("solution");
        keys[_solutions[solution_index]].SetUpKey("solution");

        for (int i = 0; i < _hints[solution_index].Count; i++)
        {
            hintTexts[i].text = _hints[solution_index][i];
        }
    }

    public void CorrectSolution(LockUnlocked obj)
    {
        if (lok.puzzleTag.Equals("solution_puzzle_4"))
        {
            EventBus.Instance.Broadcast(new FourthPuzzleFinished());
        }
    }
}
