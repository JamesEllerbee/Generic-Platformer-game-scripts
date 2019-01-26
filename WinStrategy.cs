using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinStrategy : Strategy {
    public static event Action playerWon;

    public void performEventAction()
    {
        if(playerWon != null)
        {
            playerWon();
        }
    }
}
