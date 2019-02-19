﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    private Dictionary<EventID, Action<object>> m_Events;

    protected override void Awake()
    {
        base.Awake();

        //Create The Dictionnary of the Actions. (Key) enum ActionsID, (value) Actions.
        m_Events = new Dictionary<EventID, Action<object>>();
    }

    //Add a Function (Value) into a Action (Key).
    //Action<object> is a way of saying that the function added to the Action may take ANY parameter. Works Like <T> but less overkill.
    public void RegisterEvent(EventID a_EventID, Action<object> a_CallBack)
    {
        if (m_Events.ContainsKey(a_EventID))
        {
            m_Events[a_EventID] += a_CallBack;
        }
        else
        {
            m_Events.Add(a_EventID, a_CallBack);
        }
    }

    //Remove a Function (Value) into a Action (Key).
    public void UnregisterEvent(EventID a_EventID, Action<object> a_CallBack)
    {
        if (m_Events.ContainsKey(a_EventID))
        {
            m_Events[a_EventID] -= a_CallBack;
        }
        else
        {
            Debug.LogWarning("The Event you are Unregistering doesnt exist. Mathf");
        }
    }

    //Call all the Functions in the Action Once.
    public void DispatchEvent(EventID a_EventID, object a_Param = null)
    {
        if (m_Events.ContainsKey(a_EventID))
        {
            m_Events[a_EventID](a_Param);
        }
        else
        {
            Debug.LogWarning("The thread you are Dispatching doesnt exist. Mathf");
        }
    }
}