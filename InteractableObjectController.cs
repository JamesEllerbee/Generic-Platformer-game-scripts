using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum InteractableEventAction { LoadScene, EventExit };
public class InteractableObjectController : MonoBehaviour {
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float fInteractionRadius; //radius of activation 
    //only for use with loadscene event
    [SerializeField] private int sceneNumber;
    [SerializeField] private InteractableEventAction whichEventAction;
    Strategy eventAction;

    [SerializeField] private bool isReusable;

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

    // Use this for initialization
    void Start () {
        PlayerController2D.Interact += PlayerInteracted;
        //this breaks generalization, consider using inheritence for specific cases.
        switch (whichEventAction)
        {
            case InteractableEventAction.LoadScene:
                eventAction = new LoadSceneStrategy(sceneNumber);
                break;
            case InteractableEventAction.EventExit:
                eventAction = new BossEventExitStrategy(this.gameObject);
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayerInteracted()
    {
        //check if player is colliding w this object
        if (Physics2D.OverlapCircle(this.transform.position, fInteractionRadius, layerMask) != null)
        {
            if (eventAction != null)
            {
                //perform event action
                eventAction.performEventAction();
                //disable object if is not reusable
                if (!isReusable)
                {
                    //unsub from player
                    PlayerController2D.Interact -= PlayerInteracted;
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, fInteractionRadius);
    }*/
    public void ButtonClick()
    {
        eventAction.performEventAction();
    }

    private void OnDisable()
    {
        PlayerController2D.Interact -= PlayerInteracted;

    }
}
