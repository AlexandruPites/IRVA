using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeypadController : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;
    public int maxDigits = 6;
    private int currentDigits = 0;
    private bool isEnabled = true;
    
    public void InputValue(string value)
    {
        if (!isEnabled)
        {
            return;
        }
        
        if (currentDigits < maxDigits)
        {
            displayText.text += value;
            currentDigits++;
        }
        
    }
    public void ResetValue()
    {
        if (!isEnabled)
        {
            return;
        }
        
        displayText.text = "";
        currentDigits = 0;
    }

    public void Submit()
    {
        if (!isEnabled)
        {
            return;
        }
        
        EventBus.Instance.Broadcast(new SecondPuzzleCodeEntered(displayText.text));
    }
    
    public IEnumerator DisplayColor(Color color, float seconds)
    {
        isEnabled = false;
        for (int i = 0; i < 3; i++)
        {
            displayText.color = color;
            yield return new WaitForSeconds(seconds);
            displayText.color = Color.white;
            yield return new WaitForSeconds(seconds);
        }

        displayText.color = color;
        currentDigits = 0;
        displayText.text = "";
        displayText.color = Color.white;
        isEnabled = true;

    }
}
