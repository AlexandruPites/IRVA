using System;
using TMPro;
using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private string value;
    [SerializeField] private KeypadController controller;

    private void Start()
    {
        text.text = value;
    }

    public void Press()
    {
        controller.InputValue(value);
    }

    public void Ok()
    {
        controller.Submit();
    }

    public void X()
    {
        controller.ResetValue();
    }
}
