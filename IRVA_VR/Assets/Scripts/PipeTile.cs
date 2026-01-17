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
        transform.localRotation *= Quaternion.Euler(0f, times * 90f, 0f);
        // Debug.Log($"({directions[0]}, {directions[1]})");
    }

    public void initializeTile(int a, int b)
    {
        directions = new List<int> { a, b };
        
    }

    public void SpawnChild(List<GameObject> prefabs, int type, int rotation)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        transform.localRotation = Quaternion.identity;
        GameObject pipe;
        PipeTile pt;

        if (type == 0)
        {
            pipe = Instantiate(prefabs[type], transform, false);
            initializeTile(1, 3);
        }
        else
        {
            pipe = Instantiate(prefabs[type], transform, false);
            initializeTile(0, 1);
        }
        pipe.transform.localPosition = Vector3.zero;
        pipe.transform.localRotation = Quaternion.identity;
        pipe.transform.localScale = Vector3.one;
        rotateTile(rotation);
    }
    
    public void SpawnChildToDirection(List<GameObject> prefabs, int direction1, int direction2)
    {
        int type = 0;
        if (Mathf.Abs(direction1 - direction2) == 2)
        {
            type = 0;
        }
        
        if (Mathf.Abs(direction1 - direction2) == 1)
        {
            type = 1;
        }
        
        if (Mathf.Abs(direction1 - direction2) == 3)
        {
            type = 1;
        }
        
        SpawnChild(prefabs, type, 0);

        bool cantRotate = true;
        for (int count = 0; count < 4; count++)
        {
            bool isValidRotation = true;
            for (int i = 0; i < directions.Count; i++)
            {
                if (directions[i] != direction1 && directions[i] != direction2)
                {
                    isValidRotation = false;
                    
                    break;
                }
            }

            if (isValidRotation)
            {
                cantRotate = false;
                break;
            }
            
            rotateTile(1);
        }

        if (cantRotate)
        {
            Debug.Log($"Cant rotate {directions[0]}, {directions[1]} to {direction1}, {direction2}");
        }
    }

    public int GetTypeFromDesiredDirection(int direction1, int direction2)
    {
        int type = 0;
        if (Mathf.Abs(direction1 - direction2) == 2)
        {
            type = 0;
        }
        
        if (Mathf.Abs(direction1 - direction2) == 1)
        {
            type = 1;
        }
        
        if (Mathf.Abs(direction1 - direction2) == 3)
        {
            type = 1;
        }

        return type;

    }
}
