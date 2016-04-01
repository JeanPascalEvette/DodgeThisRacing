using UnityEngine;
using System.Collections;

//This Script is attached to each Cursor (Player, Player2, etc.) Objects and deals with their movement and actions
public class MoveSelector : MonoBehaviour {

    public GameObject playerButton; //The object of the cursor at which this script is attached (assigned in inspector)
    public Vector3 playerPosition;  //The Initial position of the cursor

    public GameObject Coin;       //The Object of the coin held by the cursor used to select a car
    public Vector3 CoinPosition; // The initial position of the coin

    float move_player = 5.0f; //Speed at which the cursor moves
    public LevelManager l;   // Instance of LevelManager
    public int playerID;    //  An int that describes the player number (1-4). It is assigned in the Inspector

    public bool is_this_inside = false; //Bool variable that tells if the current cursor is inside a Car Icon
    public bool is_this_active = false; //Bool variable that tells if this player is active or not

    public bool is_on_button1 = false; //Bool variable to be used in the Script PlayerActivation (Currently not in use)

    //The enum Containing the different control Schemes
    public enum ControlTypesHere
    {

        ArrowKeys,
        WSDA,
        Joy1,
        Joy2,
        NotAssigned

    }

    // A variable of type ControlTypeshere that specifies which control scheme this player has chosen
    public ControlTypesHere ThisPlayerControl;

   
    //initialization
    void Start ()
    {
       l = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();

       //Obtaining and saving the initial positions of the cursor and the coin
       CoinPosition = Coin.transform.position;
       playerPosition = playerButton.transform.position;
    }


    //Called once per frame
    void Update()
    {
       HandleMovement(); 
    }


    //Handles the movement of the cursors according to the selected control Scheme
    void HandleMovement()
    { 
    
        if (ThisPlayerControl == ControlTypesHere.ArrowKeys)
        {
                float translationY = Input.GetAxis("VerticalArrows") * move_player;
                float translationX = Input.GetAxis("HorizontalArrows") * move_player;
                playerButton.transform.Translate(0, translationY, 0);
                playerButton.transform.Translate(translationX, 0, 0);
        }

        else  if (ThisPlayerControl == ControlTypesHere.WSDA)
        {
                float translationY = Input.GetAxis("VerticalWSDA") * move_player;
                float translationX = Input.GetAxis("HorizontalWSDA") * move_player;
                playerButton.transform.Translate(0, translationY, 0);
                playerButton.transform.Translate(translationX, 0, 0);
        }

        else if (ThisPlayerControl == ControlTypesHere.Joy1)
        {
                float translationY = Input.GetAxis("VerticalJoyStickLeft1") * move_player;
                float translationX = Input.GetAxis("HorizontalJoyStickLeft1") * move_player;
                playerButton.transform.Translate(0, translationY, 0);
                playerButton.transform.Translate(translationX, 0, 0);
        }

        else if (ThisPlayerControl == ControlTypesHere.Joy2)
        {
                float translationY = Input.GetAxis("VerticalJoyStickLeft2") * move_player;
                float translationX = Input.GetAxis("HorizontalJoyStickLeft2") * move_player;
                playerButton.transform.Translate(0, translationY, 0);
                playerButton.transform.Translate(translationX, 0, 0);
        }
    }  
}
