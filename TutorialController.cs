using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialController : MonoBehaviour
{
    //used to determine whether to activate the tutorial box and text
    bool keepActive;

    [SerializeField] GameObject tutorial_text;
    [SerializeField] GameObject tutorial_box;
    [SerializeField] GameObject player;

    public TutorialPrompt TutorialPrompt
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
        keepActive = false;
        TutorialPrompt.collidedMessage += updateText;

    }
    void Update()
    {
        if (tutorial_text == null)
        {
            //tutorial_text
            tutorial_text = GameManager.GM.RequestComponent("tutorial_text");

        }
        if (tutorial_box == null)
        {
            tutorial_box = GameManager.GM.RequestComponent("tutorial_box");
        }
        if(player == null)
        {
            player = GameObject.Find("Player");
        }
        tutorial_text.SetActive(keepActive);
        tutorial_box.SetActive(keepActive);

        keepActive = false;
        //extremely bad design, i cannot believe i even wrote this, this is why i need think and plan before writing
        /*
        //flag for if the tutorial text needs to be disabled
        List<GameObject> notCollided = new List<GameObject>();
        //draw overlap circle at each of the tutorial objects' locations to check for the player
        foreach (GameObject tutorialObj in tutorial_objects)
        {

            Collider2D collided = Physics2D.OverlapCircle(tutorialObj.transform.position, fInteractionRadius, layerMask);
            if(collided != null)
            {
                TutorialPrompt tutorialPrompt = tutorialObj.GetComponent<TutorialPrompt>();
                print(tutorialPrompt.Update_text);

                tutorial_text.SetActive(true);
                //get text component form the tutorial text object and update the text to the update text from the tutorial object
                tutorial_text.GetComponent<Text>().text = tutorialPrompt.Update_text;
            }
            else
            {
                notCollided.Add(tutorialObj);
            }

        }
        //if the tutorial text object is active when the player is not within any of the tutorial objects, disable
        /*
        if (!isActive)
        {
            tutorial_text.SetActive(false);
        }*/
        //
    }

    public void updateText(string theText)
    {
        if(tutorial_text != null)
        {
            Text textField = tutorial_text.GetComponent<Text>();
            if (textField != null)
            {
                textField.text = theText;
            }
            keepActive = true;
        }
        else
        {
            tutorial_text = GameManager.GM.RequestComponent("tutorial_text");
        }

        /*
        private void Collided(Collider2D collided, TutorialPrompt gameObject)
        {
            if (collided != null)
            {
                //print("correctly detected player");
                if (tutorial_text != null && update_text != null)
                {

                    textComponent.text = update_text;
                }
            }
            else
            {
                if (tutorial_text != null)
                {
                    tutorial_text.SetActive(false);
                }
            }
        }
        */
    }

    private void OnDisable()
    {
        tutorial_text = null;
        tutorial_box = null;
    }
}
