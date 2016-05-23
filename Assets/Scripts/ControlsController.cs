using UnityEngine;
using System.Collections;

//Script to detect the input from player 1 on the button to switch the control scheme of another player
public class ControlsController : MonoBehaviour {

    private bool overlay = false;
    private PlayerSelector playerSelector;
    public MoveSelector player1Control;
    public LevelManager l;

    void Start()
    {
        playerSelector = transform.GetComponentInParent<PlayerSelector>();
        if (playerSelector == null)
        {
            Debug.LogError("Player Selector not found");
        }
    }

    //If the icon of player one is inside the collider of the button allow it to press it
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
        {
            overlay = true;
            l.isP1onButton = true;
        }
    }

    //if it's outside don't allow it
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
        {
            overlay = false;
            l.isP1onButton = false;
        }
    }

    //When the cursor is on top of the button and presses the "a" button call the control manager funcion that switches the control scheme in the playerselector script
    void Update()
    {
        if (overlay)
        {
            if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)
            {
                if (Input.GetButtonDown("ButtonAJoyStick1")) { playerSelector.ControlManager(); }
            }

            else if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)
            {
                if (Input.GetButtonDown("ButtonAJoyStick2")) { playerSelector.ControlManager(); }
            }

            else if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys)
            {
                if (Input.GetButtonDown("ButtonAArrows")) { playerSelector.ControlManager(); }
            }

            else if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)
            {
                if (Input.GetButtonDown("ButtonAWSDA")) { playerSelector.ControlManager(); }
            }

        }
    }

    public void SetOverlay(bool state)
    {
        overlay = state;
        
    }

}
