using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public GameObject winpanel;
    // Use this for initialization
    private void Awake()
    {
        WinStrategy.playerWon += PlayerWon;
    }
    void Start () {
		
	}
    private void OnDisable()
    {
        WinStrategy.playerWon -= PlayerWon;
    }

    private void OnEnable()
    {
        WinStrategy.playerWon -= PlayerWon;
    }

    // Update is called once per frame
    void Update () {
		
	}

    void PlayerWon()
    {
        if(winpanel == null)
        {
            winpanel = GameObject.Find("WonGamePanel");
        }

        winpanel.SetActive(true);
        
    }
}
