using System.Collections.Generic;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> locks;
    [SerializeField] private List<MeshRenderer> keys;

    public void SetUpTablet(List<Color> lockMaterials, List<Color> keysMaterial)
    {
        for (int i = 0; i < locks.Count; i++)
        {
            locks[i].material.color = lockMaterials[i];
            keys[i].material.color = keysMaterial[i];
        }
    }
}
