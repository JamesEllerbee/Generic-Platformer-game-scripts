using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Dialogue_State { CONTROLING_DIALOGUE, WAITING }
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Dialogue_State dialogueState;
    [SerializeField] GameObject textBox;
    Queue<string> qDialogueQueue;
    [SerializeField] GameObject player;
    [SerializeField] List<GameObject> npcObjects;
    private PlayerController2D pc;

    public PlayerController2D PlayerController2D
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
    void Start()
    {
        //at start, subscribe to NPC inorder to to recieve when to display dialogue
        NPC.SendDialogue += DialogueStart;//problematic when used in another scene
        //create an empty queue to store dialogue sequence
        qDialogueQueue = new Queue<string>();
        dialogueState = Dialogue_State.WAITING;
        pc = player.GetComponent<PlayerController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
            pc = player.GetComponent<PlayerController2D>();
        }
    }

    public void DialogueStart(string Dialoguetext)
    {
        //todo: fix, not dynamically setting pc correct on scene load or restart
        //initalize dialogue UI if the player exists and if the player isn't already listening to dialogue
        if (pc != null && dialogueState != Dialogue_State.CONTROLING_DIALOGUE)
        {
            //split string and add each index to queue
            string[] textArr = Dialoguetext.Split('\n');
            foreach (string text in textArr)
            {
                qDialogueQueue.Enqueue(text);
            }
            if (qDialogueQueue.Count > 0)
            {
                //enable dialogue textbox
                textBox.SetActive(true);
                //set the text component to the first line
                textBox.GetComponentInChildren<Text>().text = qDialogueQueue.Dequeue();
                //--wait for player to control--
                //when the dialogue manager recieves a notification that it needs to display dialouge,
                //subscirbe to the playercontroller in order to be notified when the user inputs the interaction button
                PlayerController2D.Interact += Control_Dialogue;
                //after subsribing, set the dialogue manager's state to controlling dialogue to lock another instance from being initialzed
                dialogueState = Dialogue_State.CONTROLING_DIALOGUE;
            }
            else
            {
                print("started dialogue with empty log");
            }
        }
        else
        {
            //error statement
            if (pc == null)
            {
                print("player not set");
            }
            /*
            if(dialogueState == Dialogue_State.CONTROLING_DIALOGUE)
            {
                print("player already in dialogue");
            }
            */
        }
    }

    public void Control_Dialogue()
    {
        //as long as there is another entry in the dialogue, update it
        if (qDialogueQueue.Count > 0)
        {
            //update text to next entry in dialogue
            textBox.GetComponentInChildren<Text>().text = qDialogueQueue.Dequeue();
        }
        else
        {
            //end dialogue controller once there is no more entries
            End_Dialogue();
        }
    }

    public void End_Dialogue()
    {
        //reset the queue
        qDialogueQueue = new Queue<string>();
        //diable the textBox ui element
        textBox.SetActive(false);
        //stop listening to the player
        PlayerController2D.Interact -= Control_Dialogue;
        //set the player to grounded so that the player can regain controls 
        pc.Player_state = Player_State.GROUNDED;
        //reset the dialogue manager's state back to waiting for notification to begin
        dialogueState = Dialogue_State.WAITING;
    }

    private void OnDisable()
    {
        this.pc = null;
        this.player = null;
    }
}
