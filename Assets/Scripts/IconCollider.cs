using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//This Script is attached to each Car Icon to detect if this car has been chosen by the player
public class IconCollider : MonoBehaviour {

    Text TextColorCar;//An instance of the text object of the Car icons
    LevelManager l;   //An instance of LevelManager
    MoveSelector m;   //An instance of MoveSelector. Script attached to the Cursor/player objects (player, player2 etc.) to handle its movements
    ButtonCollider b; //An instance of ButtonCollider. Scripts attached to the coins objects (coin1, coin2 etc.) to detect if the cursor is near the coin
    GameObject t;     //Generic GameObject to be used in the script and assigned under certain conditions

    public PlayerSelector p1, p2, p3, p4; //Instances of The PlayerSelector scripts attached to each Panel controlling each player
    public int ID = 0;                    //A public int (set in the inspector) to differentiate each Car Icon object
 
    Transform old;  //A variable of type transform to save the position/parent of a specific object in the script in order to go back to it.

    private bool isActive; //Bool variable to control whether a certain car icon is active or not at that moment (if a cursor is pointing at one of them) 
    public GameObject Car; // The Object of the specific car icon this script is attached to

    public bool ThisCarSelected = false; // Bool Variable teeling if this car has been selected by the player
    public int ThisCarType;

    //Initializing
    void Start()
    {
        l = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>(); //Getting an instance of the level manager
        isActive = false;
        
    }

    void Update()
    {

        if (isActive) { checkControlType(); } //If a cursor is on the Icon call the function for interaction with it
        
    }


    //Function to detect a cursor entering in the trigger area of the Car icon selection
    void OnTriggerEnter2D(Collider2D trigger)
    {
        isActive = true; //If the cursor enters the icon space the icon becomes active

        TextColorCar = this.GetComponent<Text>(); //Get the instance of the car text and change it to yellow
        TextColorCar.color = Color.yellow;

        m = trigger.GetComponentInParent<MoveSelector>(); //Make the object that entered the icon space (the trigger) your moveselector target (it will be one of the cursors)
        b = trigger.GetComponent<ButtonCollider>();      // Same thing for the coin script ButtonCollider
        m.is_this_inside = true;                        // Change the bool variable telling if the cursor is inside the icon area

        ID = m.playerID;           //Get the ID of the player that entered that icon area
        t = trigger.gameObject;    //Set the geenric t object to the cursor/trigger
        old = t.transform.parent;  //Save the current parent of the coin (the cursor) so it can be reassigned to the coin later       
    }

    //Function to detect a cursor exiting from the trigger area of the Car selection icon
    void OnTriggerExit2D(Collider2D trigger)
    {
        isActive = false;                          //If the cursor enters the icon space the icon becomes inactive
        TextColorCar = this.GetComponent<Text>(); //The car text goes back to white
        TextColorCar.color = Color.white;
        m.is_this_inside = false;               //The specific cursor is not inside the icon area anymore

    }

  //This function checks the control types used by the cursor entering the car icon area and then makes it possible to select or deselect it
    void checkControlType() {

        if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)

        {
            //If the A button is pressed the coin is assigned to that car icon and the cursor doesn't move it around anymore (Selection of the car)
            if (Input.GetButtonDown("ButtonAJoyStick1") && l.is_joy1_taken && m.is_this_inside == true)
            {

                t.transform.parent = Car.transform;             //The coin is given the car icon as a parent
                Car.GetComponent<Collider2D>().enabled = false; //The collider of the car icon is deactivated. This car cannot be selected by other players
                l.num_ready_players++;                          // Increase the number of players who are ready to GO
                ThisCarSelected = true;                         //Bool variable telling the player has selected this car

                m.ThisPlayerCar = ThisCarType;
            }

            //If the cursor is near the coin inside the area of the car icon and presses B he re-acquires the coin and deselects the car
            if (Input.GetButtonDown("ButtonXJoyStick1") && l.is_joy1_taken && m.is_this_inside == true && b.is_player_near)
            {

                t.transform.parent = old;                      //The coin is given the old parent back (the cursor)
                Car.GetComponent<Collider2D>().enabled = true; //The car Collider is re-activated
                l.num_ready_players--;                         //The number of players ready to go is decreased
                ThisCarSelected = false;                       //The car is deselected

                m.ThisPlayerCar = 0;
            }
        }

        //Same for all the other control types
        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)
        {
            if (Input.GetButtonDown("ButtonAJoyStick2") && l.is_joy2_taken && m.is_this_inside == true)
            {

                t.transform.parent = Car.transform;
                Car.GetComponent<Collider2D>().enabled = false;
                l.num_ready_players++;
                ThisCarSelected = true;

                m.ThisPlayerCar = ThisCarType;
            }

            if (Input.GetButtonDown("ButtonXJoyStick2") && l.is_joy2_taken && m.is_this_inside == true && b.is_player_near)
            {

                t.transform.parent = old;
                Car.GetComponent<Collider2D>().enabled = true;
                l.num_ready_players--;
                ThisCarSelected = false;

                m.ThisPlayerCar = 0;
            }
        }

        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys)
        {
            if (Input.GetButtonDown("ButtonAArrows") && l.is_arrowKeys_taken && m.is_this_inside == true)
            {

                t.transform.parent = Car.transform;
                Car.GetComponent<Collider2D>().enabled = false;
                l.num_ready_players++;
                ThisCarSelected = true;

                m.ThisPlayerCar = ThisCarType;
            }

            if (Input.GetButtonDown("ButtonXArrows") && l.is_arrowKeys_taken && m.is_this_inside == true && b.is_player_near)
            {
                t.transform.parent = old;
                Car.GetComponent<Collider2D>().enabled = true;
                l.num_ready_players--;
                ThisCarSelected = false;

                m.ThisPlayerCar = 0;
            }
        }

        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)
        {
            if (Input.GetButtonDown("ButtonAWSDA") && l.is_wsda_taken && m.is_this_inside == true)
            {
                t.transform.parent = Car.transform;
                Car.GetComponent<Collider2D>().enabled = false;
                l.num_ready_players++;
                ThisCarSelected = true;

                m.ThisPlayerCar = ThisCarType;
            }

            if (Input.GetButtonDown("ButtonXWSDA") && l.is_wsda_taken && m.is_this_inside == true && b.is_player_near)
            {
                t.transform.parent = old;
                Car.GetComponent<Collider2D>().enabled = true;
                l.num_ready_players--;
                ThisCarSelected = false;

                m.ThisPlayerCar = 0;
            }
        }
    }

    //This function is called in the script PlayerSelector controlling the panels. If a player is deactivated by a panel the coin, the cursor and their positions are re-set to default
    public void CheckPlayerActivation()
    {
            m.ThisPlayerCar = 0;
            t.transform.parent = old;                       //The coin is given its parent back (the cursor)
            t.transform.position = m.CoinPosition;          //It gets shifter to its original position
            TextColorCar.color = Color.white;               //The text color of the car icon is set to white
            Car.GetComponent<Collider2D>().enabled = true;  //The icon collider is set to active

            if (ThisCarSelected)
        {
            l.num_ready_players--;                          //The number of ready players is decreased
            m.ThisPlayerCar = 0; 
        } 
            ThisCarSelected = false;                        //The boolian for the car selection is set to false
    }
}
