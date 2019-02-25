using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trooper : MonoBehaviour
{
    private List<Vector3> m_Path = new List<Vector3>();

    private float m_Speed = 0.0f;

    private float m_Timer = 0.0f;
    private float m_LerpValue = 0.0f;
    private int m_PathIndex = 0;

    [SerializeField]
    private Slider m_HealthBar;
    [SerializeField]
    private Canvas m_Canvas;
    //private bool isHit = false;

    private int m_Health;

    private void Start ()
    {
        m_Path = PathManager.Instance.Path;
        AjustSpeed(m_PathIndex);
        LookForward(m_PathIndex);
        m_Health = TrooperManager.Instance.Health;
        m_HealthBar.value = 1;
        m_Canvas.transform.position = transform.position;
        m_Canvas.gameObject.SetActive(false);
    }

    private void Update()
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
                TrooperManager.Instance.TrooperSaved();
            }

            if(m_PathIndex < m_Path.Count - 1)
            {
                AjustSpeed(m_PathIndex);
                LookForward(m_PathIndex);
            }
        }
    }

    public void GetHit(int damage)
    {   
        m_Health -= damage;
        if(m_Health <= 0)
        {
            TrooperManager.Instance.KillTroop(gameObject);
        }

        UpdateUI(damage);
    }

    private void UpdateUI(int damage)
    {
        m_Canvas.gameObject.SetActive(true);

        m_HealthBar.value -= ((float)damage / (float)TrooperManager.Instance.Health);
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
