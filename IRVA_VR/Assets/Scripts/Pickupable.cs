using UnityEngine;
using Valve.VR.InteractionSystem;

public class Pickupable : MonoBehaviour
{
    public FloatingInventory activeInventory;
    public bool isPicked = false;
    public float scaleMultiplier;
    public Vector3 savedScale;
    public Transform savedParent;

    private void OnAttachedToHand(Hand hand)
    {
        if (activeInventory != null)
        {
            activeInventory.RemoveItem(gameObject);
            activeInventory = null;
            isPicked = false;
        }
    }
}
