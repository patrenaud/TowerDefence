using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * The anchors of the Dialogue box will change its relative position. 
 * So be sure to set the anchors you want before using it.
 * 
 * Mathf.
 */

public class DialogueBox : MonoBehaviour
{
    [SerializeField]
    private Text m_Text;

    [SerializeField]
    private GameObject m_Pannel;


    //Set the scale of the recttransform of the dialogue box
    public void SetScale(float a_Width, float a_Height)
    {
        m_Pannel.GetComponent<RectTransform>().sizeDelta = new Vector2(a_Width, a_Height);
        m_Text.rectTransform.sizeDelta = new Vector2(a_Width, a_Height);
    }

    //Set the position of the recttransform of the dialogue box
    public void SetPosition(Vector2 a_Coordinates)
    {
        m_Pannel.GetComponent<RectTransform>().anchoredPosition = a_Coordinates;
    }

    public void SetPannelColor(Color a_Color)
    {
        m_Pannel.GetComponent<Renderer>().material.color = a_Color;
    }

    public void SetTextColor(Color a_Color)
    {
        m_Text.GetComponent<Renderer>().material.color = a_Color;
    }
}
