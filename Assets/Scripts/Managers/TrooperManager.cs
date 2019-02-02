using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrooperManager : Singleton<TrooperManager>
{
    // To access Troopers in map
    private List<Trooper> m_Troops = new List<Trooper>();
    public List<Trooper> Troops
    {
        get { return m_Troops; }
    } 

    // Activation of troopers in map
    private bool m_TriggerSpawn = false;

    [SerializeField]
    private float m_MoveSpeed = 5.0f;
    public float MoveSpeed
    {
        get { return m_MoveSpeed; }
    }

    [SerializeField]
    private int m_TotalTroopers = 10;
    private int m_TroopCounter = 0;

    [SerializeField]
    private float m_SpawnCooldown = 2.0f;
    private float m_Timer = 0.0f;

    // Unit to be instantiated
    [SerializeField]
    private GameObject m_Trooper;

    [SerializeField]
    private int m_Health = 3;
    public int Health
    {
        get { return m_Health; }
    }

	
	private void Update ()
    {
		if(m_TriggerSpawn)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer > m_SpawnCooldown && m_TroopCounter < m_TotalTroopers)
            {
                CreateTrooper(m_Timer);
            }
        }
	}

    private void CreateTrooper(float timer)
    {
        Instantiate(m_Trooper, PathManager.Instance.Path[0], Quaternion.identity);
        m_Timer = 0.0f;
        m_TroopCounter += 1;
        m_Troops.Add(m_Trooper.GetComponent<Trooper>());
    }

    public void ActivateTroppers()
    {
        m_TriggerSpawn = true;
    }

    public void KillTroop(GameObject trooper)
    {
        m_Troops.Remove(trooper.GetComponent<Trooper>());
        Destroy(trooper);
    }
}
