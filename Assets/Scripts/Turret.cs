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
    private float m_FireRate = 1.0f;
    [SerializeField]
    private float m_BulletSpeed = 3.0f;
    [SerializeField]
    private GameObject m_BulletPrefab;
	
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
        if (m_TargetList != null)
        {
            for (int i = 0; i < m_TargetList.Count; i++)
            {
                if(m_TargetList[i])
                {
                    m_TurretAngle = m_TargetList[i].transform.position - transform.position;
                    break;
                }
            }
        }
    }

    private void SetTarget()
    {
        if(m_TargetList != null)
        {
            for(int i = 0; i < m_TargetList.Count; i++)
            {
                if(m_TargetList[i])
                {
                    m_Target = m_TargetList[i];
                    break;
                }
            }
        }
    }

    private void Fire()
    {
        // Instantiate bullet
        if(m_Target != null)
        {
            GameObject m_BulletInstance = Instantiate(m_BulletPrefab, transform.position, Quaternion.identity);
            TurretBullet script = m_BulletInstance.GetComponent<TurretBullet>();

            // Set Destination and Velocity (In front of first troop of list)
            Vector3 m_BulletTarget = (m_Target.transform.position - transform.position).normalized;
            script.InitSpeed(m_BulletSpeed, m_BulletTarget);
        }        
    }


    private void OnTriggerEnter2D(Collider2D aOther)
    {
        Debug.Log("Target Sighted");
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
