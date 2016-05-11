using UnityEngine;
using System.Collections;

public class CPUController : MonoBehaviour {

    public GameObject player;
    public MoveSelector player1Control;

    bool is_player_near = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (is_player_near) {

            TokenController();
        }

	}

    void OnTriggerEnter2D(Collider2D trigger)
    {
        
        if (trigger.gameObject == player) { is_player_near = true;
            Debug.Log("Player is near");
        }

       // Debug.Log("Player is near");
    }

    void OnTriggerExit2D(Collider2D trigger) {

        if (trigger.gameObject == player) { is_player_near = false;
            Debug.Log("Player exited");
        }
    }

    void TokenController() {

        if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)
        {

        }
        else if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2) { }
        else if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys)
        {
            if (Input.GetButtonDown("ButtonAArrows")) {

                transform.parent = player.transform;
            }
        }
        else if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA) { }

    }


}
