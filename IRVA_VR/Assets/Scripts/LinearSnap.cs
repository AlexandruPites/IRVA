using Unity.Mathematics.Geometry;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LinearSnap : MonoBehaviour
{
    public LinearMapping linearMapping;
    public int segmentCount;
    public Transform startPosition;
    public Transform endPosition;

    protected virtual void OnDetachedFromHand(Hand hand)
    {
        float step = 1f / (float)(segmentCount - 1);
        float pos = 0;
        float min_dist = Mathf.Abs(linearMapping.value - pos);
        int closest = 0;
        
        for (int i = 1; i < segmentCount; i++)
        {
            pos += step;
            float current_distance = Mathf.Abs(linearMapping.value - pos);
            if (current_distance < min_dist)
            {
                min_dist = current_distance;
                closest = i;
            }
        }

        linearMapping.value = (float)closest * step;
        transform.position = Vector3.Lerp( startPosition.position, endPosition.position, linearMapping.value );
    }

}
