using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathManager : Singleton<PathManager>
{
    private const float PATH_STEP = 0.1f;
    private const float MAX_STEP_LENGHT = 0.6f;
    private const float MAX_PATH_POINTS = 1000; // Approx 2.5x la cironference de la map.

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
    private RaycastHit2D m_MouseHitInfo;
    private Vector2 m_HitPos;

    private bool m_InDrawingPhase = true; //Is it the time for the player to draw the path?
    private bool m_IsPathConfirmed = false; //Has the player confirmed that the drawn path his the path he really wants?

    [SerializeField]
    private GameObject m_PathPointPrefab;
    private List<GameObject> m_Trace = new List<GameObject>();

    [SerializeField]
    private GameObject m_DialogueBoxPrefab; //A text box that is moving in the screen.
    private List<DialogueBox> m_DialogueBoxs = new List<DialogueBox>();

    private void Start()
    {
        if(DialogueBoxManager.Instance)
        {
            DialogueBoxManager.Instance.InstantiateDialogueBox(GetInstanceID(), Vector2.zero);
        }
    }

    private void Update()
    {
        if (m_InDrawingPhase)
        {
            DrawingUpdate();
        }
        else if(!m_IsPathConfirmed && m_Path.Count != 0)
        {
            AskForPathConfirmation();
        }
        else
        {
            //The path is drawn and confirmed, Release the KRAKEN (troops).
            // WARNING !!!      DOES NOT WORK IF ACTIVATION IS PUT HERE!!
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            DialogueBoxManager.Instance.SetScale(GetInstanceID(), 0, 500, 500);
            DialogueBoxManager.Instance.SetPosition(GetInstanceID(), 0, new Vector2(200, 400));
        }
    }

    private void DrawingUpdate()
    {
        if (m_Path.Count == 0)
        {
            // TODO: Show UI asking the player to clic on the entrance to start drawing his path HERE.
            if (Input.GetMouseButtonDown(0) && MouseRaycast("Entrance"))
            {
                AddPathPoint(m_MouseHitInfo.transform.position);
                AddPathPoint(m_HitPos);
            }
        }
        else
        {
            // TODO: Show UI asking the player to hold his clic down to draw a path to the exit HERE.
            if (Input.GetMouseButtonDown(0) && MouseRaycast("Entrance"))
            {
                AddPathPoint(m_HitPos);
            }
            else if (Input.GetMouseButton(0) && MouseRaycast("Exit"))
            {
                AddPathPoint(m_HitPos);
                AddPathPoint(m_MouseHitInfo.transform.position);
                m_InDrawingPhase = false;
                SetTraceColor(Color.green);

                // Sets active troops !!!!!!!!!!!!!! KRAKEN RELEASING AND STUFF
                TrooperManager.Instance.ActivateTroppers();
            }
            else if (Input.GetMouseButton(0) && MouseRaycast("Obstacle"))
            {
                //TODO: Show UI telling the player that making a path through a obstacle is impossible.
            }
            else if (Input.GetMouseButton(0) && (MouseRaycast("Ground") || MouseRaycast("Entrance")))
            {
                DrawPath();
            }
            else
            {
                ResetPath();
            }
        }
    }

    private void DrawPath()
    {
        float dist = Vector3.Distance(m_Path[m_Path.Count - 1], m_HitPos);
        if (dist >= PATH_STEP)
        {
            if (dist >= MAX_STEP_LENGHT)
            {
                Debug.Log("You Are Drawing The Path Too Fast.");
                ResetPath();
            }
            else
            {
                AddPathPoint(m_HitPos);
            }
        }

        if(m_Path.Count >= MAX_PATH_POINTS)
        {
            Debug.Log("Too many path points.");
            ResetPath();
        }
    }

    private void AddPathPoint(Vector3 a_PathPoint)
    {
        m_Trace.Add(Instantiate(m_PathPointPrefab, a_PathPoint, Quaternion.identity));
        m_Path.Add(a_PathPoint);
    }

    public void ResetPath()
    {
        for(int i = 0; i < m_Trace.Count; i++)
        {
            Destroy(m_Trace[i]);
        }

        m_Trace = new List<GameObject>();
        m_Path = new List<Vector3>();

        m_InDrawingPhase = true;
    }

    private bool MouseRaycast(string a_LayerName)
    {
        SetMouseHitInfo(a_LayerName);

        if (m_MouseHitInfo.collider != null)
        {
            m_HitPos = m_MouseHitInfo.point;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetMouseHitInfo(string a_SortingLayer)
    {
        m_MouseHitInfo = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), 1000f, LayerMask.GetMask(a_SortingLayer));
    }

    private void AskForPathConfirmation()
    {

    }

    private void SetTraceColor(Color a_Color)
    {
        for(int i = 0; i < m_Trace.Count; i++)
        {
            m_Trace[i].GetComponent<Renderer>().material.color = a_Color;
        }
    }

}
