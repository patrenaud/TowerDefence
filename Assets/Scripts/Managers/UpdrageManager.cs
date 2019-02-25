using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdrageManager : Singleton<TrooperManager>
{

    private int m_Credits = 0;
    

    public void IncreaseCredits(int credits)
    {
        m_Credits += credits;
    }

}
