using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T GetRandomElement<T>(this IList<T> list)
    {
        if (list == null)
        {
            Debug.LogError("GetRandomElement called on a null list!");
            return default;
        }
        
        if (list.Count == 0)
        {
            Debug.LogError("GetRandomElement called on an empty list!");
            return default;
        }
        
        return list[Random.Range(0, list.Count)];
    }
}