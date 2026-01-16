using System.Collections.Generic;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> locks;
    [SerializeField] private List<MeshRenderer> keys;

    public void SetUpTablet(List<Material> lockMaterials, List<Material> keysMaterial)
    {
        for (int i = 0; i < locks.Count; i++)
        {
            locks[i].material = lockMaterials[i];
            keys[i].material = keysMaterial[i];
        }
    }
}
