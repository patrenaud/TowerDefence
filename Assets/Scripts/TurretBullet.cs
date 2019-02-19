using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D m_Rigidbody;

    private float m_Speed;

    [SerializeField]
    private int m_TurretDamage = 1;

    private Vector3 m_Dir = new Vector3();

    public void InitSpeed(float aSpeed, Vector3 aDir)
    {
        m_Speed = aSpeed;
        m_Dir = aDir;
    }

	private void FixedUpdate ()
    {
        m_Rigidbody.velocity = m_Dir * m_Speed;
    }

    private void OnTriggerEnter2D(Collider2D aOther)
    {
        aOther.gameObject.GetComponent<Trooper>().GetHit(m_TurretDamage);
        Destroy(gameObject);
    }
}
