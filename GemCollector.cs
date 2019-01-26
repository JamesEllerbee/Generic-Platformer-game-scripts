using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GemCollector : MonoBehaviour
{
    [SerializeField] GameObject gems_parent;
    List<Gem> unCollectedList;
    [SerializeField] private List<Gem> gems_list;
    [SerializeField] private LayerMask playerMask;

    public Gem Gem
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
        //at start add all the gem child objects from the gems collection
        //LoadSceneStrategy.LoadingScene += LoadingScene;
        gems_list = new List<Gem>();
        AddGemsFromParent();
    }

    // Update is called once per frame
    void Update()
    {
        if(gems_parent == null)
        {
            //this allows the parent to be found easily in the scene without manually setting the reference to it, this helps with persistency
            gems_parent = GameObject.Find("Gems");
            //instantiate new list
            gems_list = new List<Gem>();
            //add the gems from the scene into the structure
            AddGemsFromParent();
        }
        else
        {

            if (gems_list.Count > 0)
            {
                unCollectedList = new List<Gem>(gems_list);
                foreach (Gem gem in gems_list)
                {
                    //this creates a collider around the gem gameObj and checks for the player's collider
                    Collider2D collider = Physics2D.OverlapCircle(gem.transform.position, gem.fgem_interactionradius, playerMask);
                    
                    //if there is a collider and the gameObject and the collision was the player
                    if (collider != null && gem.gameObject.activeSelf)
                    {
                        //perform interaction 
                        gem.Interact();
                        //removed 
                        unCollectedList.Remove(gem);
                    }
                }
                gems_list = unCollectedList;
            }
        }    
    }

    public void LoadingScene()
    {
        //set instance vars to null for new scene
        gems_parent = null;
        gems_list = null;
        unCollectedList = null;
    }

    public void AddGemsFromParent()
    {
        //iterate through childern
        if (gems_parent != null && gems_parent.transform.childCount > 0)
        {
            for (int i = 0; i < gems_parent.gameObject.transform.childCount; i++)
            {
                gems_list.Add(gems_parent.gameObject.transform.GetChild(i).GetComponent<Gem>());
            }
        }
    }
}