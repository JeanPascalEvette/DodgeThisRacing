using UnityEngine;
using System.Collections;

public class MoveSelector : MonoBehaviour {

    public GameObject playerButton;

    float move_player = 5.0f;

    public LevelManager l;

    public bool is_this_inside = false;

    public int playerID;

    public bool is_this_active = false;

    public bool is_on_button1 = false;

   

    public enum ControlTypesHere
    {

        ArrowKeys,
        WSDA,
        Joy1,
        Joy2,
        NotAssigned

    }

    public ControlTypesHere ThisPlayerControl;
    bool thisPlayerReady = false;

    // Use this for initialization
    void Start () {

        l = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();

    }

    // Update is called once per frame
    void Update() {

       HandleMovement(); 

    }



        void HandleMovement() { 
    
        if (ThisPlayerControl == ControlTypesHere.ArrowKeys)
        {

            if (!thisPlayerReady)
            {
                float translationY = Input.GetAxis("VerticalArrows") * move_player;
                float translationX = Input.GetAxis("HorizontalArrows") * move_player;
                playerButton.transform.Translate(0, translationY, 0);
                playerButton.transform.Translate(translationX, 0, 0);

            }


            if (is_this_inside && !thisPlayerReady && Input.GetButtonDown("ButtonAArrows"))
            {
                //thisPlayerReady = true;
               // l.num_ready_players++;

            }

            if (is_this_inside && thisPlayerReady && Input.GetButtonDown("ButtonXArrows"))
            {
                //thisPlayerReady = false;
               // l.num_ready_players--;

            }



        }

        else  if (ThisPlayerControl == ControlTypesHere.WSDA)
        {
            if (!thisPlayerReady)
            {
                float translationY = Input.GetAxis("VerticalWSDA") * move_player;
                float translationX = Input.GetAxis("HorizontalWSDA") * move_player;
                playerButton.transform.Translate(0, translationY, 0);
                playerButton.transform.Translate(translationX, 0, 0);

            }

            if (is_this_inside && !thisPlayerReady && Input.GetButtonDown("ButtonAWSDA"))
            {
               // thisPlayerReady = true;
               // l.num_ready_players++;

            }

            if (is_this_inside && thisPlayerReady && Input.GetButtonDown("ButtonXWSDA"))
            {
                //thisPlayerReady = false;
                //l.num_ready_players--;

            }

        }

        else if (ThisPlayerControl == ControlTypesHere.Joy1)
          {

            if (!thisPlayerReady)
            {

                float translationY = Input.GetAxis("VerticalJoyStickLeft1") * move_player;
                float translationX = Input.GetAxis("HorizontalJoyStickLeft1") * move_player;
                playerButton.transform.Translate(0, translationY, 0);
                playerButton.transform.Translate(translationX, 0, 0);

            }

            if (is_this_inside && !thisPlayerReady && Input.GetButtonDown("ButtonAJoyStick1"))
            {
                //thisPlayerReady = true;
               // l.num_ready_players++;

            }


            if (is_this_inside && thisPlayerReady && Input.GetButtonDown("ButtonXJoyStick1"))
            {
                //thisPlayerReady = false;
                //l.num_ready_players--;

            }

      }

        else if (ThisPlayerControl == ControlTypesHere.Joy2)
        {
            if (!thisPlayerReady)
            {

                float translationY = Input.GetAxis("VerticalJoyStickLeft2") * move_player;
                float translationX = Input.GetAxis("HorizontalJoyStickLeft2") * move_player;
                playerButton.transform.Translate(0, translationY, 0);
                playerButton.transform.Translate(translationX, 0, 0);

            }

            if (is_this_inside && !thisPlayerReady && Input.GetButtonDown("ButtonAJoyStick2"))
            {
                //thisPlayerReady = true;
                //l.num_ready_players++;

            }

            if (is_this_inside && thisPlayerReady && Input.GetButtonDown("ButtonXJoyStick2"))
            {
                //thisPlayerReady = false;
               // l.num_ready_players--;

            }

        }



    }


    
}
