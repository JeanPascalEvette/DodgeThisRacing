using UnityEngine;
using System.Collections;

public class CPUController : MonoBehaviour {

    public GameObject player;
    public MoveSelector player1Control;
    public PlayerSelector Panel;


    public bool is_player_near = false;
    public bool is_coin_cpu = false;
    public int CoinID;
    public bool is_grabbed = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Panel.is_CPU == true) { is_coin_cpu = true; }
        else                      { is_coin_cpu = false; }

        if (is_player_near && is_coin_cpu && !is_grabbed) {

            TokenController();
        }

	}

    void OnTriggerEnter2D(Collider2D trigger)
    {
        
        if (trigger.gameObject == player) { is_player_near = true;
            Debug.Log("Player is near");
        }

    }

    void OnTriggerExit2D(Collider2D trigger) {

        if (trigger.gameObject == player) { is_player_near = false;
            Debug.Log("Player exited");
        }

        if (is_grabbed && is_coin_cpu) { is_grabbed = false; }
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
                is_grabbed = true;
            }
        }
        else if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA) { }

    }


}
