using TMPro;
using UnityEngine;

public class MathShapes : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private MeshRenderer renderer;

    public void SetUpShape(string value, Material mat)
    {
        displayText.text = value;
        renderer.material = mat;
    }
}
