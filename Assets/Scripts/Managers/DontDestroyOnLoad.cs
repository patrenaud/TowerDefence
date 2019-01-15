using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
	virtual protected void Awake ()
    {
        DontDestroyOnLoad(gameObject);
	}
}
