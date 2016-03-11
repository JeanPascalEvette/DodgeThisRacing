using UnityEngine;
using System.Collections;

public class MoveSelector : MonoBehaviour {

    public GameObject playerButton;

    float move_player = 5.0f;

    float translationX;
    float translationY;

    public LevelManager l;

    public enum ControlTypesHere
    {

        ArrowKeys,
        WSDA,
        Joy1,
        Joy2

    }

    public ControlTypesHere ThisPlayerControl;

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

            if (Input.GetKey(KeyCode.RightArrow)){ playerButton.transform.Translate(move_player, 0, 0);}
            if (Input.GetKey(KeyCode.LeftArrow)) { playerButton.transform.Translate(-move_player, 0, 0);}
            if (Input.GetKey(KeyCode.UpArrow))   { playerButton.transform.Translate(0, move_player, 0);}
            if (Input.GetKey(KeyCode.DownArrow)) { playerButton.transform.Translate(0, -move_player, 0);}

        }

        if (ThisPlayerControl == ControlTypesHere.WSDA)
        {

            if (Input.GetKey(KeyCode.D)) { playerButton.transform.Translate(move_player, 0, 0); }
            if (Input.GetKey(KeyCode.A)) { playerButton.transform.Translate(-move_player, 0, 0); }
            if (Input.GetKey(KeyCode.W)) { playerButton.transform.Translate(0, move_player, 0); }
            if (Input.GetKey(KeyCode.S)) { playerButton.transform.Translate(0, -move_player, 0); }

        }

        else if (ThisPlayerControl == ControlTypesHere.Joy1)
            {

                translationY = Input.GetAxis("Vertical") * move_player;
                translationX = Input.GetAxis("Horizontal") * move_player;
                playerButton.transform.Translate(0, translationY, 0);
                playerButton.transform.Translate(translationX, 0, 0);

            }

           

        }


    
}
