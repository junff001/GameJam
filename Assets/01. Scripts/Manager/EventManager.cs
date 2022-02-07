using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager
{
    private static Hashtable eventHashtable = new Hashtable();

    public static void AddEvent(string eventName, Action addEvent)
    {
        if (eventHashtable.ContainsKey(eventName))
        {
            eventHashtable[eventName] = addEvent;
        }
        else
        {
            eventHashtable.Add(eventName, addEvent);
        }
    }
    public static void AddEvent(string eventName, Action<GameObject> addEvent)
    {
        if (eventHashtable.ContainsKey(eventName))
        {
            eventHashtable[eventName] = addEvent;
        }
        else
        {
            eventHashtable.Add(eventName, addEvent);
        }
    }
    public static void AddEvent(string eventName, Action<float, float> addEvent)
    {
        if (eventHashtable.ContainsKey(eventName))
        {
            eventHashtable[eventName] = addEvent;
        }
        else
        {
            eventHashtable.Add(eventName, addEvent);
        }
    }
    public static void AddEvent(string eventName, Action<Action> addEvent)
    {
        if (eventHashtable.ContainsKey(eventName))
        {
            eventHashtable[eventName] = addEvent;
        }
        else
        {
            eventHashtable.Add(eventName, addEvent);
        }
    }

    public static void RemoveEvent(string eventName)
    {
        if (eventHashtable.ContainsKey(eventName))
        {
            eventHashtable.Remove(eventName);
        }
        else
        {
            Debug.Log("제거할 함수가 없습니다.");
        }
    }

    public static void TriggerEvent(string eventName)
    {
        Action action;

        if (eventHashtable.ContainsKey(eventName))
        {
            if (eventHashtable[eventName] is Action)
            {
                action = (Action)eventHashtable[eventName];
                action?.Invoke();
            }
        }
    }
    public static void TriggerEvent(string eventName, GameObject obj)
    {
        Action<GameObject> action;

        if (eventHashtable.ContainsKey(eventName))
        {
            if (eventHashtable[eventName] is Action<GameObject>)
            {
                action = (Action<GameObject>)eventHashtable[eventName]; // 언박싱
                action?.Invoke(obj);
            }
        }
    }
    public static void TriggerEvent(string eventName, float moveX, float moveY)
    {
        Action<float, float> action;

        if (eventHashtable.ContainsKey(eventName))
        {
            if (eventHashtable[eventName] is Action<float, float>)
            {
                action = (Action<float, float>)eventHashtable[eventName]; // 언박싱
                action?.Invoke(moveX, moveY);
            }
        }
    }
}
