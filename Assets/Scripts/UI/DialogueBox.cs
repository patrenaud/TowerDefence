using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

    [SerializeField]
    private Button m_ButtonPrefab;
    private Button m_PositiveButton;
    private Button m_NegativeButton;

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
        m_Pannel.GetComponent<CanvasRenderer>().SetColor(a_Color);
    }

    public void SetPannelMaterial(Material a_Material)
    {
        m_Pannel.GetComponent<CanvasRenderer>().SetMaterial(a_Material, 0);
    }

    public void SetPannelTexture(Texture a_Texture)
    {
        m_Pannel.GetComponent<CanvasRenderer>().SetTexture(a_Texture);
    }

    public void SetTextColor(Color a_Color)
    {
        m_Text.color = a_Color;
    }

    public void SetText(string a_Text)
    {
        m_Text.text = a_Text;
    }

    //Create a button in the left bottom corner of the dialogue box.
    public void SetPositiveButton(string a_Text, UnityAction a_CallBack)
    {
        RectTransform pannel = m_Pannel.GetComponent<RectTransform>();
        Vector2 pos = new Vector2(-pannel.sizeDelta.x * 0.5f, -pannel.sizeDelta.y * 0.5f);
        pos += new Vector2(90, 20);

        m_PositiveButton = Instantiate(m_ButtonPrefab, Vector3.zero, Quaternion.identity, pannel);
        m_PositiveButton.GetComponent<RectTransform>().anchoredPosition = pos;
        m_PositiveButton.GetComponentInChildren<Text>().text = a_Text;
        m_PositiveButton.onClick.AddListener(a_CallBack);
    }

    //Create a button in the right bottom corner of the dialogue box.
    public void SetNegativeButton(string a_Text, UnityAction a_CallBack)
    {
        RectTransform pannel = m_Pannel.GetComponent<RectTransform>();
        Vector2 pos = new Vector2(pannel.sizeDelta.x * 0.5f, -pannel.sizeDelta.y * 0.5f);
        pos += new Vector2(-90, 20);

        m_NegativeButton = Instantiate(m_ButtonPrefab, Vector3.zero, Quaternion.identity, pannel);
        m_NegativeButton.GetComponent<RectTransform>().anchoredPosition = pos;
        m_NegativeButton.GetComponentInChildren<Text>().text = a_Text;
        m_NegativeButton.onClick.AddListener(a_CallBack);
    }

    //Erase all the buttons of the dialogue box.
    public void DeleteButtons()
    {
        DeletePositiveButton();
        DeleteNegativeButton();
    }

    //Erase the positive button of the dialogue box.
    public void DeletePositiveButton()
    {
        if(m_PositiveButton)
        {
            Destroy(m_PositiveButton.gameObject);
            m_PositiveButton = null;
        }
    }

    //Erase the negative button of the dialogue box.
    public void DeleteNegativeButton()
    {
        if (m_NegativeButton)
        {
            Destroy(m_NegativeButton.gameObject);
            m_NegativeButton = null;
        }
    }
}
