using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ModeVisuals : MonoBehaviour
{
    [Header("Components to Control")]
    [SerializeField]
    private TextMeshPro buttonText;
    
    [SerializeField]
    private RoundedBoxProperties buttonRenderer;

    [Header("Event Source")]
    [SerializeField]
    private InteractableUnityEventWrapper pokeInteractable;

    [Header("Visual States")]
    [SerializeField]
    private Color stateOffColor = new (0, 1, 0, 1);
    [SerializeField]
    private string stateOffText = "SEMI";
    
    [SerializeField]
    private Color stateOnColor = new (1, 0, 0, 1);
    [SerializeField]
    private string stateOnText = "AUTO";

    private bool _isToggled = false;

    private void Start()
    {
        UpdateVisuals();
    }

    private void OnEnable()
    {
        if (pokeInteractable != null)
        {
            pokeInteractable.WhenSelect.AddListener(HandlePoke);
        }
    }

    private void OnDisable()
    {
        if (pokeInteractable != null)
        {
            pokeInteractable.WhenSelect.RemoveListener(HandlePoke);
        }
    }
    
    private void HandlePoke()
    {
        _isToggled = !_isToggled;
        UpdateVisuals();
    }
    
    private void UpdateVisuals()
    {
        if (_isToggled)
        {
            buttonText.text = stateOnText;
            buttonRenderer.Color = stateOnColor;
        }
        else
        {
            buttonText.text = stateOffText;
            buttonRenderer.Color = stateOffColor;
        }
    }
}
