using System;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Drawer : MonoBehaviour
{
    [SerializeField] private LinearMapping linearMapping;
    [SerializeField] private Transform handleStart, handleEnd;
    [SerializeField] private Transform drawerStart;
    [SerializeField] private string drawerTag = "defaultTest";
    [SerializeField] public Transform root;
    [SerializeField] public int capacity = 2;
    [SerializeField] public Transform spawnRoot;

    private Vector3 drawerEnd;
    private bool isOpen = false;

    private void Start()
    {
        drawerEnd = drawerStart.position + (handleEnd.position - handleStart.position);
    }

    private void Update()
    {
        if (!isOpen && linearMapping.value > 0.1f) 
        {
            isOpen = true;
            EventBus.Instance.Broadcast(new DrawerOpened(drawerTag));
        }
        else if (isOpen && linearMapping.value < 0.05f)
        {
            isOpen = false;
            EventBus.Instance.Broadcast(new DrawerClosed(drawerTag));
        }

        var posLerp = Vector3.Lerp(drawerStart.position, drawerEnd, linearMapping.value);
        root.position = posLerp;
    }
}
