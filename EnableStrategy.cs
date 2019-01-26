using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableStrategy : Strategy
{
    [SerializeField] private GameObject toEnable;

    public EnableStrategy(GameObject gameObject)
    {
        toEnable = gameObject;
    }

    public void performEventAction()
    {   
        toEnable.SetActive(true);
    }
}
