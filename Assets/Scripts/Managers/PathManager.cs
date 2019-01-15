using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : Singleton<PathManager>
{
    private const float PATH_STEP = 0.1f;

    private List<Vector3> m_Path = new List<Vector3>();
    public List<Vector3> Path
    {
        get { return m_Path; }
    }
    public Vector3 GetPathPoint(int a_Index)
    {
        if(m_Path.Count < a_Index)
        {
            return m_Path[a_Index];
        }
        else
        {
            Debug.LogWarning("The Move point index is out of range. Mathf");
        }

        return Vector3.zero;
    }
    private RaycastHit2D m_HitInfo;
    private Vector2 m_HitPos;

    private bool m_IsDrawingPath = false;

    public GameObject m_PathPointPrefab;

    private void Update()
    {
        if(!m_IsDrawingPath)
        {
            if(RaycastBeggining() && Input.GetMouseButtonDown(0))
            {
                m_IsDrawingPath = true;
                AddPathPoint(m_HitPos);
            }
        }
        else if(Input.GetMouseButton(0) && (RaycastGround() || RaycastBeggining()))
        {
            DrawPath();
        }
        else
        {
            m_IsDrawingPath = false;
            ResetPath();
        }
    }

    private void DrawPath()
    {
        if(Vector3.Distance(m_Path[m_Path.Count - 1], m_HitPos) <= PATH_STEP)
        {
            AddPathPoint(m_HitPos);
        }
    }

    private void AddPathPoint(Vector3 a_PathPoint)
    {
        m_Path.Add(a_PathPoint);
    }

    private void ResetPath()
    {
        m_Path = new List<Vector3>();
    }

    private bool RaycastBeggining()
    {
        m_HitInfo = Physics2D.Raycast(Input.mousePosition, Vector2.zero, LayerMask.GetMask("Beggining"));

        if (m_HitInfo.collider != null)
        {
            m_HitPos = m_HitInfo.point;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool RaycastGround()
    {
        m_HitInfo = Physics2D.Raycast(Input.mousePosition, Vector2.zero, LayerMask.GetMask("Ground"));

        if (m_HitInfo.collider != null)
        {
            m_HitPos = m_HitInfo.point;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool RaycastEnding()
    {
        m_HitInfo = Physics2D.Raycast(Input.mousePosition, Vector2.zero, LayerMask.GetMask("Ending"));

        if (m_HitInfo.collider != null)
        {
            m_HitPos = m_HitInfo.point;
            return true;
        }
        else
        {
            return false;
        }
    }
}
