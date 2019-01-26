using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreCounter: MonoBehaviour {

    public Text score_amount;
    private int score;

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

    public PossumController PossumController
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
        score = 0;
        Gem.GemCollected += AddScore;
        PossumController.SendWorth += AddScore;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddScore(int toAdd)
    {
        //adds score to text 
        score += toAdd;
        if (score_amount == null)
        {
            score_amount = GameManager.GM.RequestComponent("score_amount").GetComponent<Text>();
        }
        score_amount.text = score.ToString();

    }

    private void OnDisable()
    {
        score_amount = null;
    }
}
