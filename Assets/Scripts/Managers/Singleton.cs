using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : DontDestroyOnLoad where T : Singleton<T>
{
    private static T m_Instance;
    public static T Instance
    {
        get { return m_Instance; }
    }

    protected override void Awake()
    {
        if(m_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            m_Instance = (T)this;
        }

        base.Awake();
    }
}
