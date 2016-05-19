using UnityEngine;
using System.Collections;

public class CPUController : MonoBehaviour {

    public GameObject player;
    public MoveSelector player1Control;
    public PlayerSelector Panel,Player1Panel;
    public ControlsController ControlButton;
    public ButtonController ActivationButton;
    public LevelManager l;

    public bool is_player_near = false;
    public bool is_coin_cpu = false;
    public int CoinID;
    public bool is_grabbed = false;
    public bool is_car_selected = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Panel.is_CPU == true) { is_coin_cpu = true; }
        else                      { is_coin_cpu = false; }

        if (is_player_near && is_coin_cpu && !is_car_selected && player1Control.playerID != CoinID)
        {

            TokenController();
        }

    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        
        if (trigger.gameObject == player) { is_player_near = true;
            l.isP1onButton = true;
            Debug.Log("Player is near");
        }

    }

    void OnTriggerExit2D(Collider2D trigger) {

        if (trigger.gameObject == player) { is_player_near = false;
            l.isP1onButton = false;
            Debug.Log("Player exited");
        }

    }

   public void TokenController() {

        if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)
        {
            if (Input.GetButtonDown("ButtonAJoyStick1")) { CollectToken(); }
        }

        else if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)
        {
            if (Input.GetButtonDown("ButtonAJoyStick2")) { CollectToken(); }
        }

        else if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys)
        {
            if (Input.GetButtonDown("ButtonAArrows")) {CollectToken();}
        }

        else if (player1Control.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)
        {
            if (Input.GetButtonDown("ButtonAWSDA")) { CollectToken(); }
        }

    }

    void CollectToken()

    {
        transform.parent = player.transform;
        is_grabbed = true;
        player.GetComponent<Collider2D>().enabled = false;
        Player1Panel.Hand.sprite = Player1Panel.hand_closed;
        player1Control.Hand.SetAsLastSibling();
        ControlButton.SetOverlay(false);
        ActivationButton.SetOverlay(false);

    }

}
