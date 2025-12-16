using System;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LockController : MonoBehaviour
{
    [SerializeField] private string tag = "Key";
    [SerializeField] private bool destroyOnUse = true;

    [SerializeField] private Collider collider;
    [SerializeField] private Transform attachPosition;

    private void OnTriggerEnter(Collider other)
    {
        print("Collided smth");
        if (other.CompareTag(tag))
        {
            print("Collided with a key");
            Unlock(other.gameObject);
        }
    }

    void Unlock(GameObject key)
    {
        Rigidbody keyRb = key.GetComponent<Rigidbody>();
        if (keyRb) 
        {
            keyRb.isKinematic = true; 
            keyRb.useGravity = false;
        }
        
        Interactable interactable = key.GetComponent<Interactable>();
        if (interactable != null && interactable.attachedToHand != null)
        {
            interactable.attachedToHand.DetachObject(key);
        }
        key.transform.SetParent(attachPosition);
        key.transform.position = attachPosition.position;
        key.transform.rotation = attachPosition.rotation;
        
        if (destroyOnUse)
        {
            Destroy(key);
        }

        collider.enabled = false;
    }
}
