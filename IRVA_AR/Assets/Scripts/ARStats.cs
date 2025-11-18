using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARStats : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text infoText;

    [Header("AR Managers")]
    public ARPlaneManager planeManager;
    public ARPointCloudManager pointCloudManager;

    void Update()
    {
        UpdateStats();
    }

    void UpdateStats()
    {
        int planeCount = planeManager.trackables.count;

        int pointCount = 0;
        foreach (var cloud in pointCloudManager.trackables)
        {
            if (cloud.positions.HasValue) 
            {
                pointCount += cloud.positions.Value.Length;
            }
        }
        
        Vector3 camPos = Camera.main.transform.position;
        Vector3 camRot = Camera.main.transform.rotation.eulerAngles;

        // 4. Afisare
        string stats = $"Plane: {planeCount}\n" +
                       $"Puncte: {pointCount}\n" +
                       $"Pozitie: {camPos:F2}\n" +
                       $"Rotatie: {camRot:F0}";
        
        infoText.text = stats;
    }
}