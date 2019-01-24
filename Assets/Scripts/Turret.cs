using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private List<GameObject> m_TargetList = new List<GameObject>();
    private GameObject m_Target;

    private bool m_TargetsInRange = false;
    private float m_Timer = 0;

    private Vector2 m_TurretAngle = new Vector2();

    [SerializeField]
    private float m_FireRate = 1;
	
	private void Start ()
    {

	}

    private void Update()
    {
        // Fire at targets if detected
        if(m_TargetsInRange)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= m_FireRate)
            {
                SetTarget();
                Fire();
                m_Timer = 0.0f;
            }
        }

        // Update sprite to aim at Target closest to Exit
        if (m_TargetsInRange)
        {
            if (m_Target != null)
            {
                SetTurretAngle();
                transform.right = -m_TurretAngle;
            }
        }
    }

    private void SetTurretAngle()
    {
        m_TurretAngle = m_TargetList[0].transform.position - transform.position;
    }

    private void SetTarget()
    {
        if(m_TargetList != null)
        {
            m_Target = m_TargetList[0];
        }
    }

    private void Fire()
    {
        // Instantiate bullet
        // Set Destination and Velocity (In front of first troop of list)
        Debug.Log("Fire a bullet");
    }


    private void OnTriggerEnter2D(Collider2D aOther)
    {
        m_Target = aOther.gameObject;
        m_TargetList.Add(m_Target);
        m_TargetsInRange = true;
    }

    private void OnTriggerExit2D(Collider2D aOther)
    {
        m_TargetList.Remove(aOther.gameObject);

        if(m_TargetList.Count <= 0)
        {
            m_TargetsInRange = false;
        }
    }
}
