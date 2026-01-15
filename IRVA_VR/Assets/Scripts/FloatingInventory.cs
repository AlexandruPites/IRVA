using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FloatingInventory : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float radius = 1.0f;
    [SerializeField] private float heightOffset = -0.3f; 
    [SerializeField] private float followSmoothness = 5f;
    
    [Header("Animation")]
    [Tooltip("How fast the items orbit around you (Degrees per second)")]
    [SerializeField] private float orbitSpeed = 80f; 
    private float currentOrbitAngle = 0f;
    
    [Header("Data")]
    [SerializeField] private List<Transform> inventoryItems = new();

    private void Update()
    {
        if (target == null)
        {
            return;
        }
        
        currentOrbitAngle += orbitSpeed * Time.deltaTime;
        if (currentOrbitAngle >= 360f)
        {
            currentOrbitAngle -= 360f;
        }
        
        HandleRingMovement();
        ArrangeItems();
    }

    private void HandleRingMovement()
    {
        Vector3 targetPos = target.position;
        targetPos.y += heightOffset;
        transform.position = targetPos;//Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSmoothness);
        transform.rotation = Quaternion.identity;
    }

    private void ArrangeItems()
    {
        if (inventoryItems.Count == 0)
        {
            return;
        }

        float angleStep = 360f / inventoryItems.Count;

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            Transform item = inventoryItems[i];
            
            float currentAngleDegrees = (i * angleStep) + currentOrbitAngle;
            
            float currentAngleRadians = currentAngleDegrees * Mathf.Deg2Rad;
            
            Vector3 xOffset = Vector3.right * (Mathf.Cos(currentAngleRadians) * radius);
            Vector3 zOffset = Vector3.forward * (Mathf.Sin(currentAngleRadians) * radius);
            
            item.position = transform.position + xOffset + zOffset;
            
            Vector3 directionToPlayer = target.position - item.position;
            directionToPlayer.y = 0;
            
            if (directionToPlayer != Vector3.zero)
            {
                item.rotation = Quaternion.LookRotation(directionToPlayer);
            }
        }
    }
    
    public void AddItem(GameObject item)
    {
        if (inventoryItems.Contains(item.transform))
        {
            return;
        }

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Pickupable listener = item.GetComponent<Pickupable>();
        listener.activeInventory = this;

        item.transform.SetParent(this.transform);
        
        item.transform.localScale = Vector3.one * listener.scaleMultiplier;

        inventoryItems.Add(item.transform);
    }

    public void RemoveItem(GameObject item)
    {
        if (inventoryItems.Contains(item.transform))
        {
            inventoryItems.Remove(item.transform);
            
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            
            Pickupable listener = item.GetComponent<Pickupable>();
            
            item.transform.SetParent(listener.savedParent);
            
            item.transform.localScale = listener.savedScale;
            
            
        }
    }
}
