using System;
using Unity.VisualScripting;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class InventoryDeposit : MonoBehaviour
{
    [SerializeField] private FloatingInventory inventory;
    
    private void OnTriggerEnter(Collider other)
    {
        HandleDeposit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        HandleDeposit(other);
    }

    private void OnTriggerExit(Collider other)
    {
       HandleDeposit(other);
    }

    private void HandleDeposit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Pickupable pick))
        {
            if (pick.isPicked)
            {
                return;
            }
            
            Interactable interactable = other.gameObject.GetComponent<Interactable>();
            if (interactable != null && interactable.attachedToHand != null)
            {
                return;
            }
            
            pick.isPicked = true;
            pick.savedScale = other.gameObject.transform.localScale;
            pick.savedParent = other.gameObject.transform.parent;
            print("Collided");
            inventory.AddItem(other.gameObject);
        }
    }
}
