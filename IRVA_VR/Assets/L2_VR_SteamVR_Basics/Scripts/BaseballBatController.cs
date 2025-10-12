using System;
using UnityEngine;

public class BaseballBatController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [SerializeField] private AudioClip hitSound;

    private void OnCollisionEnter(Collision other)
    {
        var strength = other.relativeVelocity.magnitude;
        var hitLocation = other.contacts[0].point;
        AudioSource.PlayClipAtPoint(hitSound, hitLocation, strength);
    }
}
