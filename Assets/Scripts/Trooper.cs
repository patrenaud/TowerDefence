using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trooper : MonoBehaviour
{
    private List<Vector3> m_Path = new List<Vector3>();

    private float m_Speed = 0.0f;

    private float m_Timer = 0.0f;
    private float m_LerpValue = 0.0f;
    private int m_PathIndex = 0;

    private void Start ()
    {
        m_Path = PathManager.Instance.Path;
        AjustSpeed(m_PathIndex);
        LookForward(m_PathIndex);
    }
		
	private void Update ()
    {
        m_LerpValue += (Time.deltaTime / m_Speed);
        Move(m_LerpValue);        
	}

    private void Move(float lerp)
    {
        if(lerp < 1.0f)
        {
            if(m_PathIndex < m_Path.Count - 1)
            {
                transform.position = Vector3.Lerp(m_Path[m_PathIndex], m_Path[m_PathIndex + 1], lerp);
            }

        }
        else
        {
            m_LerpValue = 0.0f;

            if(m_PathIndex < m_Path.Count - 1)
            {
                m_PathIndex += 1;
            }
            else
            {
                TrooperManager.Instance.KillTroop(gameObject);
                Debug.Log("You make 1 point !!!");
            }

            if(m_PathIndex < m_Path.Count - 1)
            {
                AjustSpeed(m_PathIndex);
                LookForward(m_PathIndex);
            }
        }
    }

    private void AjustSpeed(int A_index)
    {
        m_Speed = Mathf.Abs(Vector2.Distance(m_Path[A_index + 1], m_Path[A_index])) / TrooperManager.Instance.MoveSpeed;
    }

    private void LookForward(int A_index)
    {
        transform.right = m_Path[A_index + 1] - m_Path[A_index];
    }
}
