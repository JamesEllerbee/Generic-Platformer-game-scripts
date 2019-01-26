using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gem : MonoBehaviour {

    public int worth;
    public float fgem_interactionradius = 0.5f;
    public Animator gem_animator;
    public float animation_time ;
    public static event Action<int> GemCollected;
    public bool collected;

    public void Start()
    {
        collected = false;
    }
    public void Interact()
    {
        //set the state of the animator
        gem_animator.SetBool("collected", true);
        if (!collected)
        {
            GemCollected(worth);
            collected = true;
        }
        //start coroutine to handle animation
        StartCoroutine(Collect());
    }

    public IEnumerator Collect()
    {
        //send worth notificatino
        yield return new WaitForSeconds(animation_time);
        this.gameObject.SetActive(false);
        
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }
    //used this for viewing collider
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, fgem_interactionradius);
    }
}
