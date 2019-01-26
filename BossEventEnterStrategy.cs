using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossEventEnterStrategy : Strategy {

    private GameObject camera;
    private GameObject focusPoint;
    private GameObject[] toActivate;
    private float cameraSize; //to fit the scene
    public static event Action DisableObjects;

    public BossEventEnterStrategy() : this(null)
    {  
    }
    public BossEventEnterStrategy(GameObject camera) : this(camera, null)
    {
    }
    public BossEventEnterStrategy(GameObject camera, GameObject focusPoint) : this(camera, focusPoint, 0f)
    {       
    }
    public BossEventEnterStrategy(GameObject camera, GameObject focusPoint, float cameraSize) : this(camera,  focusPoint,  cameraSize, null)
    {
    }
    public BossEventEnterStrategy(GameObject camera, GameObject focusPoint, float cameraSize, GameObject[] toActivate)
    {
        this.camera = camera;
        this.focusPoint = focusPoint;
        this.cameraSize = cameraSize;
        this.toActivate = toActivate;

    }
    public void performEventAction()
    {
        if(DisableObjects != null)
        {
            DisableObjects();
        }
        //set the cameras positoin to the focuspoint
        CameraController camCtrl = camera.GetComponent<CameraController>();
        camCtrl.Target = focusPoint;
        //set the camera's size
        camera.GetComponent<Camera>().orthographicSize = cameraSize;
        //enable all the gameobjects involved with the boss event.
        if(toActivate != null)
        {
            foreach (GameObject gameobj in toActivate)
            {
                gameobj.SetActive(true);
            }
        }
 
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
