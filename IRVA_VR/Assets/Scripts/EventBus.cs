using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance { get; private set; }

    private readonly Dictionary<Type, List<object>> subscribers = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Subscribe<T>(Action<T> listener) where T : class
    {
        var eventType = typeof(T);
        if (!subscribers.ContainsKey(eventType))
        {
            subscribers[eventType] = new List<object>();
        }
        subscribers[eventType].Add(listener);
    }

    public void Unsubscribe<T>(Action<T> listener) where T : class
    {
        var eventType = typeof(T);
        if (subscribers.TryGetValue(eventType, out var subscriberList))
        {
            subscriberList.Remove(listener);
        }
    }

    public void Broadcast<T>(T eventData) where T : class
    {
        var eventType = typeof(T);
        if (!this.subscribers.TryGetValue(eventType, out var subscribersOut)) return;
        foreach (var subscriber in new List<object>(subscribersOut))
        {
            (subscriber as Action<T>)?.Invoke(eventData);
        }
    }
}