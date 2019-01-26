using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameState { PLAYING, PAUSED};
public class GameManager : MonoBehaviour
{
    GameState state;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject wonPanel;
    [SerializeField] private GameObject tutorial_text;
    [SerializeField] private GameObject tutorial_box;
    [SerializeField] private GameObject score_amount;
    // Use this for initialization
    private static GameManager gameManager;
    
    public static GameManager GM
    {
        get
        {
            if(gameManager == null)
            {
                gameManager = GameObject.Find("GameController").GetComponent<GameManager>();
            }
            return gameManager;
        }

    }

    public UIController UIController
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
        }
    }

    void Start()
    {
        gameManager = this;
        DontDestroyOnLoad(this);
        WinStrategy.playerWon += Win;
        state = GameState.PLAYING;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            pausePanel.SetActive(TogglePause());
        }
    }

    bool TogglePause()
    {
        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            state = GameState.PLAYING;
            return false;
        }
        else
        {
            Time.timeScale = 0f;
            state = GameState.PAUSED;
            return true;
        }
    }

    public void Resume()
    {
        if(pausePanel != null)
        {
            pausePanel.SetActive(TogglePause());
        }
    }

    public void Quit()
    {
        //do quit, quit method lol
    }

    public void Restart()
    {       
        SceneManager.LoadScene(0);
        TogglePause();
        Destroy(this.gameObject);
    }

    public void Win()
    {
        //do win
        if(wonPanel != null)
        {
            wonPanel.SetActive(TogglePause());
        }          
    }
    
    public GameObject RequestComponent(string name)
    {
        switch (name)
        {
            case "tutorial_text":
                if (tutorial_text != null)
                {
                    return tutorial_text;
                }
                break;

            case "tutorial_box":
                if(tutorial_box != null)
                {
                    return tutorial_box;
                }
                break;
            case "score_amount":
                if(score_amount != null)
                {
                    return score_amount;
                }
                break;
        }
        print("not found or DNE");
        return null;
    }
}