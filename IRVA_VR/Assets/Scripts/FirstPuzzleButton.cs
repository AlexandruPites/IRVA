using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FirstPuzzleButton : MonoBehaviour
{
    public int buttonNumber;

    public MeshRenderer visualComponent;
    
    public bool isEnabled = false;

    public Material touchMaterial, defaultMaterial;

    public void BroadcastTouch()
    {
        if (isEnabled)
        {
            visualComponent.material = touchMaterial;
            EventBus.Instance.Broadcast(new FirstPuzzleButtonPressed(buttonNumber));
        }
    }

    public void OnHandRemove()
    {
        if (isEnabled)
        {
            visualComponent.material = defaultMaterial;
        }
    }

    public void ChangeMaterial(Material material)
    {
        visualComponent.material = material;
    }
}