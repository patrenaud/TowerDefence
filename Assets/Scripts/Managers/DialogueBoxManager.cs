using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * This Manager Must Be A Canvas.
 * 
 * IMPORTANT: 
 * The anchors of the Dialogue box will change its relative position. 
 * So be sure to set the anchors you want on the dialogue box prefab.
 * 
 * This manager doesnt know what index each dialoge box is. 
 * The indexes may change if a box is deleted, so make sure that a class unsing many box know what is the index his boxes at all time.
 * 
 * Mathf.
 */

[RequireComponent(typeof(Canvas))]
public class DialogueBoxManager : Singleton<DialogueBoxManager>
{
    [SerializeField]
    private GameObject m_DialogueBoxPrefab;
    private Dictionary<int, List<DialogueBox>> m_DialogueBoxes = new Dictionary<int, List<DialogueBox>>();

    //Creates a dialogue box for a specific owner.
    public void InstantiateDialogueBox(int a_OwnerID, Vector2 a_Position)
    {
        if (!m_DialogueBoxes.ContainsKey(a_OwnerID))
        {
            m_DialogueBoxes.Add(a_OwnerID, new List<DialogueBox>());
        }

        m_DialogueBoxes[a_OwnerID].Add(Instantiate(m_DialogueBoxPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<DialogueBox>());
        m_DialogueBoxes[a_OwnerID][m_DialogueBoxes[a_OwnerID].Count - 1].SetPosition(a_Position);
    }

    //Delete a dialogue box for a specific owner.
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

    //Set the scale of a dialogue box.
    public void SetScale(int a_OwnerID, int a_Index, float a_Width, float a_Height)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].SetScale(a_Width, a_Height);
        }
    }

    //Set the position of a dialogue box.
    public void SetPosition(int a_OwnerID, int a_Index, Vector2 a_Coordinates)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].SetPosition(a_Coordinates);
        }
    }

    //Set the background color of a dialogue box.
    public void SetPannelColor(int a_OwnerID, int a_Index, Color a_Color)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].SetPannelColor(a_Color);
        }
    }

    //Set the background material of a dialogue box.
    public void SetPannelMaterial(int a_OwnerID, int a_Index, Material a_Material)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].SetPannelMaterial(a_Material);
        }
    }

    //Set the background texture of a dialogue box.
    public void SetPannelTexture(int a_OwnerID, int a_Index, Texture a_Texture)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].SetPannelTexture(a_Texture);
        }
    }

    //Set the text color of a dialogue box.
    public void SetTextColor(int a_OwnerID, int a_Index, Color a_Color)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].SetTextColor(a_Color);
        }
    }

    //Set the text/message of a dialogue box.
    public void SetText(int a_OwnerID, int a_Index, string a_Text)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].SetText(a_Text);
        }
    }

    //Create a button in the left bottom corner of the dialogue box.
    public void SetPositiveButton(int a_OwnerID, int a_Index, string a_Text, UnityAction a_CallBack)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].SetPositiveButton(a_Text, a_CallBack);
        }
    }

    //Create a button in the right bottom corner of the dialogue box.
    public void SetNegativeButton(int a_OwnerID, int a_Index, string a_Text, UnityAction a_CallBack)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].SetNegativeButton(a_Text, a_CallBack);
        }
    }

    //Delete all buttons of the dialogue box.
    public void DeleteButtons(int a_OwnerID, int a_Index)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].DeleteButtons();
        }
    }

    //Delete the positive button of the dialogue box.
    public void DeletePositiveButton(int a_OwnerID, int a_Index)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].DeletePositiveButton();
        }
    }

    //Delete the Negative button of the dialogue box.
    public void DeleteNegativeButton(int a_OwnerID, int a_Index)
    {
        if (m_DialogueBoxes.ContainsKey(a_OwnerID) && a_Index < m_DialogueBoxes[a_OwnerID].Count)
        {
            m_DialogueBoxes[a_OwnerID][a_Index].DeleteNegativeButton();
        }
    }
}
