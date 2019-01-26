using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

//add enum to choose event strategy
enum Event_sstrategy { ENABLE, WIN }
public class NPC : MonoBehaviour {

    private Strategy eventAction;
    [SerializeField] private GameObject eventObject;

    // change this to be set in unity
    [SerializeField] private string character = "Tutori";
    //currently used for debug, will be changed
    [SerializeField] private string dialogue;

    public static event Action<string> SendDialogue;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float fInteractionRadius;

    // Use this for initialization
    void Start()
    {
        string path = "./dialogue/" + character + ".txt";
        if (File.Exists(path))
        {
            //print("file found");
            using (StreamReader sr = new StreamReader(path))
            {
                dialogue = sr.ReadToEnd();
            }
        }
        PlayerController2D.Interact += StartDialogue;
        //expand to use file io

        //only create an event action if there is a eventObject
        if (eventObject != null)
        {
            eventAction = new EnableStrategy(eventObject);
        }    
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartDialogue()
    {
        //print("notification recieved");
        //check if player is colliding w this object
        Collider2D collided = Physics2D.OverlapCircle(this.transform.position, fInteractionRadius, layerMask);

        if (collided != null)
        {            
            if(SendDialogue != null)
            {
                //used to send dialogue to the dialogue manager
                SendDialogue(dialogue);
            }
            //if the event action is not null
            if (eventAction != null)
            {
                //perform an event action
                eventAction.performEventAction();
            }
        }
    }


    private void OnDisable()
    {
        PlayerController2D.Interact -= StartDialogue;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, fInteractionRadius);
    }

    public PlayerController2D PlayerController2D
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
        }
    }

    public Strategy Strategy
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
        }
    }
}
