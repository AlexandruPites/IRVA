using System.Collections.Generic;
using UnityEngine;


public class PipeTile : MonoBehaviour
{
    public List<int> directions;
    
    public static readonly List<(int x, int y)> Offsets =
        new()
        {
            (-1, 0), // UP
            (0, 1), // Right
            (1, 0), // Down
            (0, -1) // Left
        };

    public void rotateTile(int times)
    {
        for (int i = 0; i < directions.Count; i++)
        {
            directions[i] = (directions[i] + times) % 4;
        }
        // Debug.Log($"({directions[0]}, {directions[1]})");
    }

    public void initializeTile(int a, int b)
    {
        directions = new List<int> { a, b };
        
    }
}
