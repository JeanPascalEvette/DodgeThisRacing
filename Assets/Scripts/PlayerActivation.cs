using UnityEngine;
using System.Collections;

//NOT IN USE FOR NOW

public class PlayerActivation : MonoBehaviour {

    //public GameObject player;
   // public bool is_player_near = true;
    bool is_player_on_button = false;

    public PlayerSelector Panel;
   // public GameObject player;

   public MoveSelector m1,m2,m3,m4;

    MoveSelector m;

    //// Use this for initialization
    void Start()
    {
        //m = GameObject.FindWithTag("PlayerMenu").GetComponent<MoveSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (m1.is_on_button1)
        //{
        //    if (m1.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1 && Input.GetButtonDown("ButtonAJoyStick1"))
        //    {

        //        Panel.player_selector();
        //        Panel.PanelManager();

        //    }
        //}
    }


    void OnTriggerEnter2D(Collider2D trigger)
    {

        //if (trigger == player.GetComponent<Collider2D>())

        //if(Input.GetButtonDown("ButtonAJoyStick1"))

        //trigger = player.GetComponent<Collider2D>();

        //m = trigger.GetComponent<MoveSelector>();

        if(trigger == GameObject.FindWithTag("Player1").GetComponent<Collider2D>())

        {
           // m = m1.GetComponent<MoveSelector>();
            //m1.is_on_button1 = true;
        }

        //print("Enter Button player " + m1.is_on_button1);
    }


    void OnTriggerExit2D(Collider2D trigger)
    {

        if (trigger == GameObject.FindWithTag("Player1").GetComponent<Collider2D>())

        {
            //m = m1.GetComponent<MoveSelector>();
            //m1.is_on_button1 = false;
        }


        //print("Exit Button player " + m1.is_on_button1);
    }
}
