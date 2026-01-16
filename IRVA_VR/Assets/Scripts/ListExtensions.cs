using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T GetRandomElement<T>(this IList<T> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogError("GetRandomElement called on a null or empty list!");
            return default;
        }
        
        return list[Random.Range(0, list.Count)];
    }
}