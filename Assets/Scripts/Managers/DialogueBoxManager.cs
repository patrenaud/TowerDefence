using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This Manager Must Be A Canvas.
 * 
 * IMPORTANT: 
 * The anchors of the Dialogue box will change its relative position. 
 * So be sure to set the anchors you want on the dialogue box prefab.
 * This manager doesnt know what index each dialoge box is. 
 * The indexes may change if a box is deleted, so be sure that the class that will use many box know what is the index his boxes at all time.
 * 
 * Mathf.
 */

[RequireComponent(typeof(Canvas))]
public class DialogueBoxManager : Singleton<DialogueBoxManager>
{
    [SerializeField]
    private GameObject m_DialogueBoxPrefab;
    private Dictionary<int, List<DialogueBox>> m_DialogueBoxes = new Dictionary<int, List<DialogueBox>>();

    public void InstantiateDialogueBox(int a_OwnerID, Vector2 a_Position)
    {
        if(!m_DialogueBoxes.ContainsKey(a_OwnerID))
        {
            m_DialogueBoxes.Add(a_OwnerID, new List<DialogueBox>());
        }

        m_DialogueBoxes[a_OwnerID].Add(Instantiate(m_DialogueBoxPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<DialogueBox>());
        m_DialogueBoxes[a_OwnerID][m_DialogueBoxes[a_OwnerID].Count - 1].SetPosition(a_Position);
    }

    public void DeleteDialogueBox(int a_OwnerID, int a_Index)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            Destroy(m_DialogueBoxes[a_OwnerID][a_Index].gameObject);
            m_DialogueBoxes[a_OwnerID].RemoveAt(a_Index);

            if (m_DialogueBoxes[a_OwnerID].Count <= 0)
            {
                m_DialogueBoxes.Remove(a_OwnerID);
            }
        }
    }

    public void SetScale(int a_OwnerID, int a_Index, float a_Width, float a_Height)
    {
        if(m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].SetScale(a_Width, a_Height);
        }
    }

    public void SetPosition(int a_OwnerID, int a_Index, Vector2 a_Coordinates)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].SetPosition(a_Coordinates);
        }
    }
}
