using System.Collections;
using System.Text;
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
    
    public IEnumerator DisplayDigitFeedback(string playerInput, string correctCode, float seconds)
    {
        isEnabled = false;

        StringBuilder coloredString = new StringBuilder();

        for (int i = 0; i < playerInput.Length; i++)
        {
            if (i < correctCode.Length)
            {
                if (playerInput[i] == correctCode[i])
                {
                    coloredString.Append($"<color=green>{playerInput[i]}</color>");
                }
                else
                {
                    coloredString.Append($"<color=red>{playerInput[i]}</color>");
                }
            }
        }

        for (int i = 0; i < 3; i++)
        {
            displayText.text = coloredString.ToString();
            yield return new WaitForSeconds(seconds);

            displayText.text = playerInput;
            displayText.color = Color.white; 
            yield return new WaitForSeconds(seconds);
        }

        currentDigits = 0;
        displayText.text = "";
        displayText.color = Color.white;
        isEnabled = true;
    }
}
