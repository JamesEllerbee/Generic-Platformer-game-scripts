using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public enum Player_State { GROUNDED, JUMPING, IN_AIR, DEAD, BEGIN_CLIMB, CLIMBING, LISTENING, PAUSED }
public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player_rigidbody2D; //used to apply force based on inputs
    [SerializeField] private Animator player_animator; //change animation states

    [SerializeField] private LayerMask ground_layer; //layer mask for use in checkground function

    [SerializeField] private LayerMask interaction_layer; //layer mask for use in interact function
    [SerializeField] private GameObject interaction_prompt; //keyboard button sprite for interation                                                          
    [SerializeField] private float fInteraction_radius; //sets the radius for the interaction overlap circle

    [SerializeField] private LayerMask climbLayer; //layermask for objects that can be climbed
    [SerializeField] private GameObject climbPrompt; //keyboard button sprite for climbing
    [SerializeField] private float fclimb_radius; //sets the radius for the climbable overlap circle
    [SerializeField] private float fclimb_speed;


    //physics 
    [SerializeField] float fHorizontalMod; //horizonal movement modifier
    [SerializeField] float fVerticalMod; //verical movement modifer
    [SerializeField] private float movement_smoothing;
    private Vector2 velocity = Vector2.zero; //use for out var in damping function
    private float fgravity_scale;
    private const float dist_to_ground = 1.08f;
    //[SerializeField] int intAxis = 3; //i forgot what this was for 

    //varibles involved with determining movement/animation
    float x_move;
    float y_move;
    bool facingRight;
    bool moveRight;
    bool doJump;
    bool isDead;
    bool tryInteract;
    bool tryClimb;

    //allows other objects to be notified when the player ineracts
    public static event Action Interact;


    //used for determining where to place the player after falling off
    [SerializeField] private GameObject checkPoint;
    public GameObject CheckPoint
    {
        get
        {
            return checkPoint;
        }
        set
        {
            checkPoint = value;
        }
    }
    [SerializeField] private Vector2 lastGroundedPos;
    private Vector2 spawnCoord;

    [SerializeField] private Player_State player_state;
    public Player_State Player_state
    {
        get
        {
            return player_state;
        }
        set
        {
            player_state = value;
        }
    }

    void Start()
    {
        lastGroundedPos = transform.position;
        facingRight = true;
        doJump = false;
        player_state = Player_State.GROUNDED;
        player_rigidbody2D = GetComponent<Rigidbody2D>();
        fgravity_scale = player_rigidbody2D.gravityScale;
    }

    void Update()
    {    
        UpdatePlayerState();
        //debugPlayerState();
        //print(lastGroundedPos);
    }

    void FixedUpdate()
    {
        ParseInput();
        UpdatePlayer();
        //updateLastGroundpos();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Respawn":
                //todo: play deaath animation, wait then transform...
                //reset the player's velocity
                player_rigidbody2D.velocity = Vector2.zero;
                //play the hurt animation
                player_animator.Play("player_hurt");
                //start coroutine before moving the player
                transform.position = checkPoint.transform.position;
                break;
            case "Checkpoint":
                //if the player came in collision with a checkpoint object, update the checkpoint reference
                checkPoint = collision.gameObject;
                print("checkpoint updated");
                break;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //play hurt animation if the player comes in collision with an enemy
        if (collision.gameObject.tag == "Enemy" && this.player_state != Player_State.IN_AIR)
        {
            player_animator.Play("player_hurt");
            //this.player_state = playerState.DEAD;
            //this can be expanded to do a death sequence
        }
    }

    /**
     * This method parses input from Input Manager to set flags for the update player method
     * */
    void ParseInput()
    {
        //retrieve horizontal input; values between -1, 1
        x_move = Input.GetAxis("Horizontal");
        //check if input = jump, 
        doJump = Input.GetButtonDown("Jump");
        //check if input = interact
        tryInteract = Input.GetButtonDown("Interact");
        //check if climb
        tryClimb = Input.GetButtonDown("Climb");
        y_move = Input.GetAxis("Climb");
    }

    /**
     * This method include phyics calculatios and applies them to the player's ridgidbody
     * */
    void UpdatePlayer()
    {
        if(player_state != Player_State.LISTENING && player_state != Player_State.PAUSED)
        {
            //check if player is facing the right direction
            Flip(x_move);
            float x = 0;
            float y = 0;
            //calculate movement force in x and y vectors
            switch (player_state)
            {
                case Player_State.GROUNDED:
                case Player_State.IN_AIR:
                    x = x_move * fHorizontalMod;
                    y = this.player_rigidbody2D.velocity.y;
                    break;
                case Player_State.CLIMBING:
                    x = x_move * fclimb_speed;
                    y = y_move * fclimb_speed;
                    break;
            }
            Vector2 movement = new Vector2(x, y);
            player_rigidbody2D.velocity = Vector2.SmoothDamp(player_rigidbody2D.velocity, movement, ref velocity, movement_smoothing);
            //if jump input and can jump, add jump force to player
            if (doJump && player_state == Player_State.GROUNDED)
            {
                player_rigidbody2D.AddForce(new Vector2(0f, fVerticalMod));
                player_state = Player_State.JUMPING;
            }
        }
        //draw overlap circle an return collider that is layered interactable
        Collider2D collided = Physics2D.OverlapCircle(this.transform.position, fInteraction_radius, interaction_layer);
        if (collided != null)
        {
            if (interaction_prompt != null && !interaction_prompt.activeSelf)
            {
                interaction_prompt.SetActive(true);
            }
            //check if there is an interacetable object in the scene
            if (Interact != null && tryInteract)
            {
                //change state to listening
                if(collided.gameObject.GetComponent<NPC>() != null)
                {
                    this.player_state = Player_State.LISTENING;
                }
                //send a notification to all interable objects
                Interact();
            }
        }
        else
        {
            if (interaction_prompt != null && interaction_prompt.activeSelf)
            {
                interaction_prompt.SetActive(false);
            }
        }
        
        if(Player_state != Player_State.CLIMBING)
        {
            //draw overlapcircle and return collider that is layered climbable
            collided = Physics2D.OverlapCircle(this.transform.position, fInteraction_radius, climbLayer);
            if (collided != null)
            {
                if (climbPrompt != null && !climbPrompt.activeSelf)
                {
                    climbPrompt.SetActive(true);
                }
                if (tryClimb)
                {
                    //set gravity scale to 0 while climbing
                    player_rigidbody2D.gravityScale = 0f;
                    Player_state = Player_State.BEGIN_CLIMB;
                }
            }
            else
            {
                if (climbPrompt != null && climbPrompt.activeSelf)
                {
                    climbPrompt.SetActive(false);
                }
            }

        }
    }

    /**
     * This method contains the logic for transitions between player states
     * */
    void UpdatePlayerState()
    {
        switch (player_state)
        {
            case Player_State.GROUNDED:
                if (!IsGrounded())
                {
                    player_state = Player_State.IN_AIR;
                    player_animator.SetBool("grounded", false);
                }
                else
                {
                    player_animator.SetBool("grounded", true);
                    player_animator.SetFloat("speed", Mathf.Abs(player_rigidbody2D.velocity.x));
                } 
                break;
            case Player_State.BEGIN_CLIMB:
                player_animator.SetBool("climbing", true);
                player_state = Player_State.CLIMBING;
                break;

            case Player_State.CLIMBING:
                Collider2D collided = Physics2D.OverlapCircle(this.transform.position, fclimb_radius, climbLayer);
                if(collided == null)
                {
                    //reset gravity scale when the player is no longer climbing
                    player_rigidbody2D.gravityScale = fgravity_scale;
                    if (IsGrounded())
                    {
                        player_state = Player_State.GROUNDED;
                        player_animator.SetBool("climbing", false);
                        player_animator.SetBool("grounded", true);
                    }
                    else
                    {
                        player_state = Player_State.IN_AIR;
                        player_animator.SetBool("climbing", false);
                        player_animator.SetBool("grounded", false);
                    }
                }
                break;

            case Player_State.JUMPING:

                player_animator.SetBool("grounded", false);
                player_state = Player_State.IN_AIR;
                break;

            case Player_State.IN_AIR:
                if (IsGrounded())
                {
                    player_state = Player_State.GROUNDED;
                }
                
                break;

            case Player_State.DEAD:
                //todo: plan logic for death, maybe use system.timer. and sync it to the death animation, at the end of the timer place the player at the spawn point and switch state.
                //
                player_animator.SetBool("alive", false);
                break;

            case Player_State.LISTENING:
                break;

            default:
                player_state = Player_State.GROUNDED;
                break;
        }
    }

    /**
     * 
     * checkGrounded method uses raycast to determine if the player is on the ground
     * 
     * */
    private bool IsGrounded()
    {
        //draw a raycast that will only interact with the ground
        RaycastHit2D results = Physics2D.Raycast(transform.position, Vector2.down, 400f, ground_layer);
        Debug.DrawLine(transform.position, results.point); //debug statement
        //the player if grounded if the distance is less than the distance to ground

        bool grounded = results.distance < dist_to_ground;
        //Debug.Log(grounded);

        return (grounded);//figure out where ground is
    }

    /**
     * This method updates the last grounded position
     * 
     */
    private void UpdateLastGroundpos()
    {
        if (IsGrounded())
        {

            lastGroundedPos = transform.position;
        }
    }

    //flip method contains logic to determine if the player is facing in the right directions upon horizontal movement
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
            //do flip

            //Switch the way the player is labelled as facing.
            facingRight = !facingRight;

            // Multiply the player's x local scale by -1.
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        return flip;
    }

    /**
     * Fuction to return whether the player can jump or not
     * deprecated
     * 
    private bool canJump()
    {
        //return true if the player is grounded, and upwards velocity is 0 (i need to figure out how much force is applied at a given time)
        return player_state == Player_State.GROUNDED && player_rigidbody2D.velocity.y == 0;
    }
    */

    /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, fInteractionRadius);
    }*/
}