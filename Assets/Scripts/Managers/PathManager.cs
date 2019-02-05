using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathManager : Singleton<PathManager>
{
    private const float PATH_STEP = 0.1f;       // Min distance between each path points.
    private const float MAX_STEP_LENGHT = 0.8f; // Max distance between each path points.
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

   // private bool m_InDrawingPhase = true; //Is it the time for the player to draw the path?
    //private bool m_IsPathConfirmed = false; //Has the player confirmed that the drawn path his the path he really wants?

    [SerializeField]
    private GameObject m_PathPointPrefab;
    private List<GameObject> m_Trace = new List<GameObject>();

    private int m_DialogueBoxesCount = 0;
    private Vector2 m_DialogueBoxesBasePos = new Vector2((-Screen.width / 2) + 210, (-Screen.height / 2) + 135); //Left corner of the screen

    private EDrawStates m_State = EDrawStates.Off;
    private delegate void VoidDelegate();
    private VoidDelegate m_CurrentUpdate; //delegate utilisé pour alléger l'update. Change dans la fct (ChangeState)

    private void Start()
    {
        m_CurrentUpdate = OffUpdate;
        ChangeState(EDrawStates.Standby);
    }

    private void Update()
    {
        m_CurrentUpdate();
    }

    //Draw a path where the mouse cursor goes.
    private void DrawPath()
    {
        float dist = Vector3.Distance(m_Path[m_Path.Count - 1], m_HitPos);
        if (dist >= PATH_STEP)
        {
            if (dist >= MAX_STEP_LENGHT)
            {
                StartCoroutine(ShowErroMessage("You are drawing the path too fast, try drawing slowly.", m_DialogueBoxesBasePos + new Vector2(0f, 250f), 4f));
                ResetPath();
                ChangeState(EDrawStates.Standby);
            }
            else
            {
                AddPathPoint(m_HitPos);
            }
        }

        if(m_Path.Count >= MAX_PATH_POINTS)
        {
            StartCoroutine(ShowErroMessage("The path is too long, try drawing a shorter one.", m_DialogueBoxesBasePos + new Vector2(0f, 250f), 4f));
            ResetPath();
            ChangeState(EDrawStates.Standby);
        }
    }

    //Create a path point.
    private void AddPathPoint(Vector3 a_PathPoint)
    {
        m_Trace.Add(Instantiate(m_PathPointPrefab, a_PathPoint, Quaternion.identity));
        m_Path.Add(a_PathPoint);
    }

    //Delete the path.
    public void ResetPath()
    {
        for(int i = 0; i < m_Trace.Count; i++)
        {
            Destroy(m_Trace[i]);
        }

        m_Trace = new List<GameObject>();
        m_Path = new List<Vector3>();
    }

    //Raycast for a specific layer setting the raycasthit infos and hitposition.
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

    //Set the raycasthit info for a specific layer.
    private void SetMouseHitInfo(string a_SortingLayer)
    {
        m_MouseHitInfo = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), 1000f, LayerMask.GetMask(a_SortingLayer));
    }

    //Show a dialogue box with a Yes/No answers to confirm the users path.
    private void AskForPathConfirmation()
    {
        if(DialogueBoxManager.Instance)
        {
            DialogueBoxManager.Instance.SetText(GetInstanceID(), 0, "Do you confirm your path?");
            DialogueBoxManager.Instance.SetPositiveButton(GetInstanceID(), 0, "Yes", ConfirmPath);
            DialogueBoxManager.Instance.SetNegativeButton(GetInstanceID(), 0, "No", CancelPath);
        }
    }

    //Callback for the positive confirmation button.
    public void ConfirmPath()
    {
        TrooperManager.Instance.ActivateTroppers();
        ChangeState(EDrawStates.Off);
    }

    //Callback for the negative confirmation button.
    public void CancelPath()
    {
        ResetPath();
        ChangeState(EDrawStates.Standby);
    }

    //Change the drawn trace color.
    private void SetTraceColor(Color a_Color)
    {
        for(int i = 0; i < m_Trace.Count; i++)
        {
            m_Trace[i].GetComponent<Renderer>().material.color = a_Color;
        }
    }

    //Show a red dialogue box for X seconds.
    private IEnumerator ShowErroMessage(string a_Message, Vector2 a_Position, float a_Duration)
    {
        if (DialogueBoxManager.Instance)
        {
            DialogueBoxManager.Instance.InstantiateDialogueBox(GetInstanceID(), a_Position);
            m_DialogueBoxesCount++;

            DialogueBoxManager.Instance.SetPannelColor(GetInstanceID(), m_DialogueBoxesCount - 1, Color.red);
            DialogueBoxManager.Instance.SetTextColor(GetInstanceID(), m_DialogueBoxesCount - 1, Color.black);
            DialogueBoxManager.Instance.SetText(GetInstanceID(), m_DialogueBoxesCount - 1, a_Message);

            yield return new WaitForSeconds(a_Duration);

            if (m_DialogueBoxesCount > 0)
            {
                DialogueBoxManager.Instance.DeleteDialogueBox(GetInstanceID(), m_DialogueBoxesCount - 1);
                m_DialogueBoxesCount--;
            }
        }
        else
        {
            yield return null;
        }
    }

    //Set the current PathManager state.
    private void ChangeState(EDrawStates a_NewState)
    {
        if(m_State == a_NewState)
        {
            return;
        }

        switch (m_State)
        {
            case EDrawStates.Off:
                {
                    OnOffExit();
                    break;
                }
            case EDrawStates.Standby:
                {
                    OnStandbyExit();
                    break;
                }
            case EDrawStates.Drawing:
                {
                    OnDrawingExit();
                    break;
                }
            case EDrawStates.Confirmation:
                {
                    OnConfirmationExit();
                    break;
                }
        }

        switch (a_NewState)
        {
            case EDrawStates.Off:
                {
                    OnOffEnter();
                    m_CurrentUpdate = OffUpdate;
                    break;
                }
            case EDrawStates.Standby:
                {
                    OnStandbyEnter();
                    m_CurrentUpdate = StandbyUpdate;
                    break;
                }
            case EDrawStates.Drawing:
                {
                    OnDrawingEnter();
                    m_CurrentUpdate = DrawingUpdate;
                    break;
                }
            case EDrawStates.Confirmation:
                {
                    OnConfirmationEnter();
                    m_CurrentUpdate = ConfirmationUpdate;
                    break;
                }
        }
        m_State = a_NewState;
    }

    //-------------State machine On______Enters--------------

    private void OnOffEnter()
    {
        if (DialogueBoxManager.Instance && m_DialogueBoxesCount > 0)
        {
            while (m_DialogueBoxesCount > 0)
            {
                DialogueBoxManager.Instance.DeleteDialogueBox(GetInstanceID(), m_DialogueBoxesCount - 1);
                m_DialogueBoxesCount--;
            }
        }
    }

    private void OnStandbyEnter()
    {
        if (DialogueBoxManager.Instance)
        {
            DialogueBoxManager.Instance.SetText(GetInstanceID(), 0, "Clic on the ENTRANCE (Green) to start drawing the path and hold the clic to draw.");
        }
    }

    private void OnDrawingEnter()
    {
        if (DialogueBoxManager.Instance)
        {
            DialogueBoxManager.Instance.SetText(GetInstanceID(), 0, "HOLD the clic to draw from the ENTRANCE (Green) to the EXIT (red).");
        }
    }

    private void OnConfirmationEnter()
    {
        AskForPathConfirmation();
    }

    //-----------------State machine Updates---------------

    private void OffUpdate()
    {

    }

    private void StandbyUpdate()
    {
        if (Input.GetMouseButtonDown(0) && MouseRaycast("Entrance"))
        {
            AddPathPoint(m_MouseHitInfo.transform.position);
            AddPathPoint(m_HitPos);
            ChangeState(EDrawStates.Drawing);
        }
    }

    private void DrawingUpdate()
    {
        if (Input.GetMouseButton(0) && MouseRaycast("Exit"))
        {
            AddPathPoint(m_HitPos);
            AddPathPoint(m_MouseHitInfo.transform.position);
            SetTraceColor(Color.green);
            ChangeState(EDrawStates.Confirmation);
        }
        else if (Input.GetMouseButton(0) && MouseRaycast("Obstacle"))
        {
            StartCoroutine(ShowErroMessage("You must draw on the ground and avoid all obstacles.", m_DialogueBoxesBasePos + new Vector2(0f, 250f), 4f));
            ResetPath();
            ChangeState(EDrawStates.Standby);
        }
        else if (Input.GetMouseButton(0) && (MouseRaycast("Ground") || MouseRaycast("Entrance")))
        {
            DrawPath();
        }
        else
        {
            StartCoroutine(ShowErroMessage("You must draw on the ground and avoid all obstacles.", m_DialogueBoxesBasePos + new Vector2(0f, 250f), 4f));
            ResetPath();
            ChangeState(EDrawStates.Standby);
        }
    }

    private void ConfirmationUpdate()
    {

    }

    //-------------State machine On____Exits--------------

    private void OnOffExit()
    {
        if (DialogueBoxManager.Instance && m_DialogueBoxesCount <= 0)
        {
            DialogueBoxManager.Instance.InstantiateDialogueBox(GetInstanceID(), m_DialogueBoxesBasePos);
            m_DialogueBoxesCount++;
        }
    }

    private void OnStandbyExit()
    {

    }

    private void OnDrawingExit()
    {

    }

    private void OnConfirmationExit()
    {
        if(DialogueBoxManager.Instance)
        {
            DialogueBoxManager.Instance.DeleteButtons(GetInstanceID(), 0);
        }
    }
}
