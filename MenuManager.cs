using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public LevelManager lm;

    public void CallCreateLevel()
    {
        lm.CreateLevel();
        lm.CreateLevel();
    }


}
