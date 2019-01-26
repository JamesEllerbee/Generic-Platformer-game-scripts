using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {
    public GameObject pl;
    const float horizontalScrollingSpeed = 0.02f;
    const float verticalScrollingSpeed = 0.02f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //todo: since the background is clouds, always move in one direction, use a queue to dequeue and move sprite to the right, like a conveyer belt.

		//if the player is moving, move slightly in the oppisite direction
        if(pl == null)
        {
            pl = GameObject.Find("Player");
        }
        else
        {
            if(pl.GetComponent<Rigidbody2D>().velocity.x > 0 || pl.GetComponent<Rigidbody2D>().velocity.x < 0)
            {
                transform.position = new Vector2(transform.position.x + (pl.transform.localScale.x > 0 ? horizontalScrollingSpeed * -1 : horizontalScrollingSpeed * 1), transform.position.y);
            }
            if (pl.GetComponent<Rigidbody2D>().velocity.y > 0 || pl.GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + (pl.GetComponent<Rigidbody2D>().velocity.y > 0 ? horizontalScrollingSpeed * -1 : horizontalScrollingSpeed * 1));
            }
        }
	}
}
