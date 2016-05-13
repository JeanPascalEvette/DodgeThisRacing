using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

    private bool overlay = false;
    private PlayerSelector playerSelector;
    public MoveSelector player1Control;

    void Start()
    {
        playerSelector = transform.GetComponentInParent<PlayerSelector>();
        if (playerSelector == null)
        {
            Debug.LogError("Player Selector not found");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
        {
            overlay = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player1")
        {
            overlay = false;
        }
    }

    void Update()
    {
        if (overlay)
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

    public void SetOverlay(bool state)
    {
        overlay = state;
    }

}
