using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private List<Troopers> m_Targets = new List<Troopers>();
    private bool m_TargetInRange = false;
    private float m_Timer = 0;

    [SerializeField]
    private float m_FireRate = 1;

    [SerializeField]
    private Transform m_BulletSpawnpoint;

	
	private void Start ()
    {
		
	}

    private void Update()
    {
        // update sprite to aim at m_Target

        // Fire at target
        m_Timer += Time.deltaTime;
        if (m_Timer >= m_FireRate)
        {
            Fire();
        }
	}

    private void Fire()
    {
        // Instantiate bullet
    }


    private void OnTriggerEnter2D(Collider2D aOther)
    {
        m_Targets.Add(aOther.gameObject.GetComponent<Troopers>());
        m_TargetInRange = true;
    }

    private void OnTriggerExit2D(Collider2D aOther)
    {
        m_Targets.Remove(aOther.gameObject.GetComponent<Troopers>());

        if(m_Targets.Count == 0)
        {
            m_TargetInRange = false;
        }
    }
}
