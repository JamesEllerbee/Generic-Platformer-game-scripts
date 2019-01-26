using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossEventExitStrategy : Strategy {
    public static event Action EnableObjects;
    private GameObject attachedObject;


    public BossEventExitStrategy() : this(null)
    {
    }
    public BossEventExitStrategy(GameObject gameObject)
    {
        this.attachedObject = gameObject;
    }
    public void performEventAction()
    {
        //reset checkpoints
        if(EnableObjects != null)
        {
            EnableObjects();
        }
        //change camera size/ target
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.GetComponent<Camera>().orthographicSize = 5;
        camera.GetComponent<CameraController>().Target = GameObject.FindGameObjectWithTag("Player");
        //disable bridge
        GameObject.FindGameObjectWithTag("BossBridge").SetActive(false);
        //disable walls
        foreach(GameObject gameObj in GameObject.FindGameObjectsWithTag("BossEvent"))
        {
            gameObj.SetActive(false);
        }
        //disable the object attached this event
        if(attachedObject != null)
        {
            attachedObject.SetActive(false);
        }
        //stop the boss from moving
        GameObject.Find("opossum-1").GetComponent<PossumController>().ApplyMovement = false;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
