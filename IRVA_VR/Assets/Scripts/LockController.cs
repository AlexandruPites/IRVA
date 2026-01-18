using System;
using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LockController : MonoBehaviour
{
    [SerializeField] private string collision_tag = "Key";
    [SerializeField] private bool destroyOnUse = false;
    [SerializeField] private float turnDuration = 0.5f;
    [SerializeField] public string puzzleTag;

    [SerializeField] private Collider collider;
    [SerializeField] private Transform attachPosition;
    [SerializeField] private MeshRenderer colorCube;
    [SerializeField] private MeshRenderer correctDisplayCube;

    public void SetUpLock(string key, Color color)
    {
        puzzleTag = key + "_puzzle_3";
        colorCube.material.color = color;
    }
    
    public void SetUpLockP4(string key)
    {
        puzzleTag = key + "_puzzle_4";
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Collided smth");
        if (other.CompareTag(collision_tag))
        {
            print("Collided with a key");
            if (other.gameObject.GetComponent<Key>().puzzleTag.Equals(puzzleTag))
            {
                print("Collided with a correct key");
                Unlock(other.gameObject);
            }
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

        key.transform.SetParent(attachPosition, true);
        
        var scale = key.transform.localScale;
        scale.z = key.transform.localScale.x;
        key.transform.localScale = scale;
        key.transform.position = attachPosition.position;
        key.transform.rotation = attachPosition.rotation;
        key.transform.localRotation = Quaternion.Euler(0, 90, -90);

        collider.enabled = false;
        
        StartCoroutine(AnimateKeyTurn(key));
    }

    IEnumerator AnimateKeyTurn(GameObject key)
    {
        Quaternion startRot = key.transform.localRotation;
        
        Quaternion rotationAmount = Quaternion.Euler(0, -180, 0);

        Quaternion endRot = startRot * rotationAmount;

        float elapsed = 0;

        while (elapsed < turnDuration)
        {
            elapsed += Time.deltaTime;

            float percent = elapsed / turnDuration;

            percent = Mathf.SmoothStep(0f, 1f, percent);

            key.transform.localRotation = Quaternion.Slerp(startRot, endRot, percent);
            yield return null;
        }

        key.transform.localRotation = endRot;
        
        EventBus.Instance.Broadcast(new LockUnlocked(puzzleTag));

        correctDisplayCube.material = colorCube.material;

        if (destroyOnUse)
        {
            Destroy(key, 2.0f);
        }
    }
}
