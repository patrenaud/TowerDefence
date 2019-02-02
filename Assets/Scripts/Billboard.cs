using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform m_CameraTransform;

	void Start ()
    {
        m_CameraTransform = Camera.main.transform;
	}

    private void Update()
    {
        transform.forward = m_CameraTransform.forward;
    }
}
