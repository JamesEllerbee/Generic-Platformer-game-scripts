using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoadSceneStrategy : Strategy  {

    [SerializeField] int sceneNum;
   
    public LoadSceneStrategy() : this(0)
    {
    }
    public LoadSceneStrategy(int sceneNum)
    {
        this.sceneNum = sceneNum;
    }

    public void performEventAction()
    {   
        SceneManager.LoadScene(sceneNum);
    }
}
