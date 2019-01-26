using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCheckPoint : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private CameraController camCtrl;

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

    private void Start()
    {
        BossEventEnterStrategy.DisableObjects += Disable;
    }

    private void FixedUpdate()
    {
  
            //if the player's position in the scene is less than the camera check point game object's position...
            if (player.transform.position.x < this.transform.position.x)
            {
                //... if the camera's target is not the checkpoint, then set it to the checkpoint
                if (camCtrl.Target != this.gameObject) { camCtrl.Target = this.gameObject; }
            }
            else
            {
                //...else set it to the player
                if (camCtrl.Target != player) { camCtrl.Target = player; }
            }
        

    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        BossEventEnterStrategy.DisableObjects -= Disable;

    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision: " + collision.gameObject.tag);
        if(collision.gameObject.tag == "Player")
        {
            if(camCtrl.getTarget().tag == "Player")
            {
                camCtrl.setTarget(this.gameObject);
            }
            else
            {
                camCtrl.setTarget(collision.gameObject);
                Debug.Log("target set to: " + collision.gameObject.tag);
            }
        }
       
    }
    */
}