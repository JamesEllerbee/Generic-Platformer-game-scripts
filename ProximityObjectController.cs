using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ProximityEventAction { EventEnter }
public class ProximityObjectController : MonoBehaviour
{
    Strategy eventAction;
    [SerializeField] private ProximityEventAction whichEventAction;
    //for use in evententer
    [SerializeField] private GameObject cameraObj;
    [SerializeField] private GameObject focus;
    [SerializeField] private float size;
    [SerializeField] private GameObject[] toActivate;

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
    void Start()
    {
        switch (whichEventAction)
        {
            case ProximityEventAction.EventEnter:
                //instantiate enter event action 
                eventAction = new BossEventEnterStrategy(cameraObj, focus, size, toActivate);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(eventAction != null)
        {
            eventAction.performEventAction();
        }
    }
}
