using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KeepLocalScale : MonoBehaviour
{

    [SerializeField] GameObject player;

    const float y_translation = 1f;

    // Use this for initialization
    void Start()
    {

    }

    private void Awake()
    {
        if (player != null)
        {
            Vector2 translated_player_location = player.transform.position;
            translated_player_location.y += y_translation;
            this.transform.position = translated_player_location;
        }
    }

    // Update is called once per frame
    private void Update()
    {

        if(player != null)
        {
            Vector2 translated_player_location = player.transform.position;
            translated_player_location.y += y_translation;
            this.transform.position = translated_player_location;
            Vector2 theScale = this.transform.localScale;
            if (theScale.x <= 0)
            {
                print("flipping");
                theScale.x *= -1;
                this.transform.localScale = theScale;
            }
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

    }
}
