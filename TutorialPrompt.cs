using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class TutorialPrompt : MonoBehaviour {
    public static Action<string> collidedMessage;
    [SerializeField] private string update_text;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float fInteractionRadius;

    public string Update_text
    {
        get
        {
            return update_text;
        }
    }

    public void Start()
    {
        
    }
    public void Update()
    {
        Collider2D collided = Physics2D.OverlapCircle(this.transform.position, fInteractionRadius, layerMask);
        if(collided != null)
        {
            if(collidedMessage != null)
            {
                collidedMessage(Update_text);
            }       
        }
    }
}
