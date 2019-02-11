using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle", menuName = "ScriptableObjects/Obstacle", order = 1)]
public class ObstacleData : ScriptableObject
{
    [SerializeField]
    private GameObject m_Prefab;
    public GameObject Prefab
    {
        get { return m_Prefab; }
    }

    [SerializeField]
    private AnimationCurve m_Quantity; //Gives the amount of this specific obstacle for each level. level 0-100
    public int Quantity(int a_Level)
    {

        if(LevelManager.Instance)
        {
            return (int)m_Quantity.Evaluate(a_Level * (1.0f / LevelManager.Instance.GetMaxLevels));
        }
        else
        {
            Debug.LogWarning("There is no level manager, the number of levels is set to 100 by default");
            return (int)m_Quantity.Evaluate(a_Level * 0.01f);
        }
    } 
}
