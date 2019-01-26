using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PossumController : MonoBehaviour
{
    //used for determining how far the possum should walk in 
    [SerializeField] float targetA;
    [SerializeField] float targetB;
    //states
    private bool facingRight;
    private bool moveRight;
    private bool applyMovement;
    public bool ApplyMovement
    {
        get
        {
            return applyMovement;
        }
        set
        {
            applyMovement = value;
        }
    }
    [SerializeField] private int worth;
    //horizontal speed, 5 by default
    [SerializeField] private float horizontalSpeed = 5f;
    [SerializeField] private Rigidbody2D possum_rigidBody; //used to apply physics
    [SerializeField] private Animator possum_animator; //used to play animations
    //obsever pattern to send this object's worth
    public static event Action<int> SendWorth;
    // Use this for initialization
    void Start()
    {
        facingRight = false;
        moveRight = false;
        applyMovement = true;
    }

    // Update is called once per frame
    void Update()
    {
        //update states
        if (transform.position.x < targetA)
        {
            moveRight = true;
        }
        if (transform.position.x > targetB)
        {
            moveRight = false;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    /**
     * Applies physics to the possum 
     * 
     * */
    private void Move()
    {
        if (applyMovement)
        {
            //declare vector object that'll store the the new movement velocity
            Vector2 movementVector;
            //if possum should move right, up date movement vector to be in the positive diection
            if (moveRight)
            {
                movementVector = new Vector2(horizontalSpeed, 0);
            }
            //otherwise, update movement vector to be negaitve
            else
            {
                movementVector = new Vector2(horizontalSpeed * -1, 0);
            }
            //flip if needed
            Flip(movementVector.x);
            //if the movement vector change, update ridgidbody's velocity vector
            if (possum_rigidBody.velocity != movementVector)
            {
                possum_rigidBody.velocity = movementVector;
            }
        }    
    }

    private bool Flip(float horizontalMove)
    {
        //flip boolean flag   
        bool flip = false;
        //flip logic
        if (horizontalMove != 0)
        {
            //if moving right and not facing right, flip
            if (horizontalMove > 0 && !facingRight)
            {
                flip = true;
            }
            //if moving left and facing right, flip
            if (horizontalMove < 0 && facingRight)
            {
                flip = true;
            }
        }
        //if need to flip; 
        if (flip)
        {
            //zero velocity
            possum_rigidBody.velocity = Vector2.zero;
            //Switch the way the player is labelled as facing.
            facingRight = !facingRight;
            
            Vector2 theScale = transform.localScale;
            // Multiply the objects's x local scale by -1.
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        return flip;
    }

    //handle collisions
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(worth != 0)
        {
            PlayerController2D whatCollided = collision.gameObject.GetComponent<PlayerController2D>();
            if (whatCollided != null)
            {
                if (whatCollided.Player_state == Player_State.IN_AIR)
                {
                    //set the animator's state to isDead true
                    possum_animator.SetBool("isDead", true);
                    horizontalSpeed = 0;
                    //wait for end of animation to disable the gameobject
                    StartCoroutine(Die());
                }
            }
        }    
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator Die()
    {
        //use the wait for seconds method to wait for the end of the current animation
        yield return new WaitForSeconds(0.8f); 
        this.gameObject.SetActive(false);
        if(SendWorth != null)
        {
            SendWorth(this.worth);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Death")
        {
            this.gameObject.SetActive(false);
        }
    }
}