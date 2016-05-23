using UnityEngine;
using System.Collections;

//Script that allows player 1 to press the activation button on other players' panels
public class ButtonController : MonoBehaviour {

    private bool overlay2 = false;
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

    //If the cursor s on top of the button make it possible to press it
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
        {
            overlay2 = true;
            l.isP1onButton = true;
        }
    }

    //Otherwise don't make it possible
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
        {
            overlay2 = false;
            l.isP1onButton = false;
        }
    }

    //If the button can be pressed check the control scheme and call the relative funcion in playerselector screen
    void Update()
    {
        if (overlay2)
        {
            if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)
            {
                if (Input.GetButtonDown("ButtonAJoyStick1")) { playerSelector.PanelManager(); }
            }

            else if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)
            {
                if (Input.GetButtonDown("ButtonAJoyStick2")) { playerSelector.PanelManager(); }
            }

            else if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys)
            {
                if (Input.GetButtonDown("ButtonAArrows")) { playerSelector.PanelManager(); }
            }

            else if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)
            {
                if (Input.GetButtonDown("ButtonAWSDA")) { playerSelector.PanelManager(); }
            }

        }
    }

    //Resets the overlay bool
    public void SetOverlay(bool state)
    {
        overlay2 = state;
       
    }

}
