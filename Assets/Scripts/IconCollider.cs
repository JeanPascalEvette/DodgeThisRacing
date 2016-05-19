using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//This Script is attached to each Car Icon to detect if this car has been chosen by the player
public class IconCollider : MonoBehaviour {

    Text TextColorCar;//An instance of the text object of the Car icons
    LevelManager l;   //An instance of LevelManager
    MoveSelector m;   //An instance of MoveSelector. Script attached to the Cursor/player objects (player, player2 etc.) to handle its movements
    public MoveSelector m1, m2, m3, m4;
    ButtonCollider b; //An instance of ButtonCollider. Scripts attached to the coins objects (coin1, coin2 etc.) to detect if the cursor is near the coin
    GameObject t;     //Generic GameObject to be used in the script and assigned under certain conditions
    CPUController CPU;
    public CPUController C1, C2, C3, C4;
    public ButtonController buttoncontroller1, buttoncontroller2, buttoncontroller3, buttoncontroller4;
    public ControlsController controlscontroller1, controlscontroller2, controlscontroller3, controlscontroller4;
    public Vector3 ThisCarIconPosition;

    public PlayerSelector p1, p2, p3, p4; //Instances of The PlayerSelector scripts attached to each Panel controlling each player
    public int ID = 0;                    
 
    Transform old;  //A variable of type transform to save the position/parent of a specific object in the script in order to go back to it.

    public bool isActive; //Bool variable to control whether a certain car icon is active or not at that moment (if a cursor is pointing at one of them) 
    public GameObject Car; // The Object of the specific car icon this script is attached to

    public bool ThisCarSelected = false; // Bool Variable teeling if this car has been selected by the player
    public int ThisCarType;              //A public int (set in the inspector) to differentiate each Car Icon object
    public Sprite ThisCarImage, ThisCarImageTemp;
    private Vector3 ParentPosition;
    private Image WheelImage;
    

    //Initializing
    void Start()
    {
        l = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>(); //Getting an instance of the level manager
        isActive = false;
        ThisCarIconPosition = transform.position;
          
    }

    void Update()
    {

        if (isActive)        { checkControlType(); } //If a cursor is on the Icon call the function for interaction with it
       
    }

    //Function to detect a cursor entering in the trigger area of the Car icon selection
    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (isActive)
        {
            WheelImage = trigger.GetComponentInChildren<Image>();
            WheelImage.color = new Color(1f, 1f, 1f, .5f);
            return;
        }

        isActive = true; //If the cursor enters the icon space the icon becomes active

        TextColorCar = this.GetComponent<Text>(); //Get the instance of the car text and change it to yellow
        TextColorCar.color = Color.yellow;
       

        CPU = trigger.GetComponent<CPUController>();

        m = trigger.GetComponentInParent<MoveSelector>(); //Make the object that entered the icon space (the trigger) your moveselector target (it will be one of the cursors)
        m.is_this_inside = true;                        // Change the bool variable telling if the cursor is inside the icon area
        t = trigger.gameObject;                        //Set the generic t object to the cursor/trigger
        old = t.transform.parent;                     //Save the current parent of the coin (the cursor) so it can be reassigned to the coin later

        ID = CPU.CoinID;
        AddTempCar();

        if (!CPU.is_coin_cpu || CPU.CoinID == 1)
        {            
            ParentPosition = t.transform.localPosition;
        }

       
    }

    //Function to detect a cursor exiting from the trigger area of the Car selection icon
    void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.gameObject.name != CPU.gameObject.name)
        {
            WheelImage = trigger.GetComponentInChildren<Image>();
            WheelImage.color = new Color(1f, 1f, 1f, 1f);
            return;
        }  

        isActive = false;
        TextColorCar = this.GetComponent<Text>(); //The car text goes back to white
        TextColorCar.color = Color.white;
        m.is_this_inside = false;               //The specific cursor is not inside the icon area anymore
        RemoveTempCar();

    }

   // void OnTriggerStay2D() { Debug.Log("STAYING"); }

  //This function checks the control types used by the cursor entering the car icon area and then makes it possible to select or deselect it
    void checkControlType() {

        if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)

        {
            ////If the A button is pressed the coin is assigned to that car icon and the cursor doesn't move it around anymore (Selection of the car)
            //if (Input.GetButtonDown("ButtonAJoyStick1") && l.is_joy1_taken && m.is_this_inside == true && !ThisCarSelected)
            //{ SelectCar(); }

            ////If the cursor is near the coin inside the area of the car icon and presses B he re-acquires the coin and deselects the car
            //if (Input.GetButtonDown("ButtonXJoyStick1") && l.is_joy1_taken && ThisCarSelected)
            //{ DeSelectCar(); }

            //Controls for normal players Select
            if (Input.GetButtonDown("ButtonAJoyStick1") && l.is_joy1_taken && m.is_this_inside == true && !ThisCarSelected /*&& !CPU.is_coin_cpu*/ && m.playerID == CPU.CoinID)
            { SelectCar(); }

            //Controls for CPU Select
            else if (Input.GetButtonDown("ButtonAJoyStick1") && l.is_joy1_taken && m.is_this_inside == true && !ThisCarSelected && CPU.is_coin_cpu && m.playerID != CPU.CoinID)
            { CPUSelectCar(); }

            //Controls for normal players Deselect
            else if (Input.GetButtonDown("ButtonXJoyStick1") && l.is_joy1_taken && ThisCarSelected && m.playerID == CPU.CoinID)
            {
                if (m.playerID != 1)
                { DeSelectCar(); }

                else
                {

                    if (!C1.is_grabbed && !C2.is_grabbed && !C3.is_grabbed && !C4.is_grabbed)
                    {
                        ResetOverlay();
                        DeSelectCar();
                    }
                }
            }

            //Controls for CPU Deselect
            else if (Input.GetButtonDown("ButtonAJoyStick1") && l.is_joy1_taken && ThisCarSelected && CPU.is_coin_cpu && CPU.is_player_near && m.playerID != CPU.CoinID)
            { CPUDeSelectCar(); }

        }

        //Same for all the other control types
        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)
        {
            //if (Input.GetButtonDown("ButtonAJoyStick2") && l.is_joy2_taken && m.is_this_inside == true && !ThisCarSelected)
            //{ SelectCar(); }

            //if (Input.GetButtonDown("ButtonXJoyStick2") && l.is_joy2_taken && ThisCarSelected)
            //{ DeSelectCar(); }

            //Controls for normal players Select
            if (Input.GetButtonDown("ButtonAJoyStick2") && l.is_joy2_taken && m.is_this_inside == true && !ThisCarSelected /*&& !CPU.is_coin_cpu*/ && m.playerID == CPU.CoinID)
            { SelectCar(); }

            //Controls for CPU Select
            else if (Input.GetButtonDown("ButtonAJoyStick2") && l.is_joy2_taken && m.is_this_inside == true && !ThisCarSelected && CPU.is_coin_cpu && m.playerID != CPU.CoinID)
            { CPUSelectCar(); }

            //Controls for normal players Deselect
            else if (Input.GetButtonDown("ButtonXJoyStick2") && l.is_joy2_taken && ThisCarSelected && m.playerID == CPU.CoinID)
            {
                if (m.playerID != 1)
                { DeSelectCar(); }

                else
                {

                    if (!C1.is_grabbed && !C2.is_grabbed && !C3.is_grabbed && !C4.is_grabbed)
                    {
                        ResetOverlay();
                        DeSelectCar();
                    }
                }
            }

            //Controls for CPU Deselect
            else if (Input.GetButtonDown("ButtonAJoyStick2") && l.is_joy2_taken && ThisCarSelected && CPU.is_coin_cpu && CPU.is_player_near && m.playerID != CPU.CoinID)
            { CPUDeSelectCar(); }
        }

        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys)
        {
            //Controls for normal players Select
            if (Input.GetButtonDown("ButtonAArrows") && l.is_arrowKeys_taken && m.is_this_inside == true && !ThisCarSelected /*&& !CPU.is_coin_cpu*/ && m.playerID == CPU.CoinID)
            { SelectCar(); }

            //Controls for CPU Select
            else if (Input.GetButtonDown("ButtonAArrows") && l.is_arrowKeys_taken && m.is_this_inside == true && !ThisCarSelected && CPU.is_coin_cpu && m.playerID != CPU.CoinID)
            { CPUSelectCar(); }

            //Controls for normal players Deselect
            else if (Input.GetButtonDown("ButtonXArrows") && l.is_arrowKeys_taken && ThisCarSelected && m.playerID == CPU.CoinID)
            {
                if (m.playerID != 1)
                { DeSelectCar(); }

                else {

                    if (!C1.is_grabbed && !C2.is_grabbed && !C3.is_grabbed && !C4.is_grabbed && !l.isP1onButton)
                       {       
                            ResetOverlay();
                            DeSelectCar();      
                       }    
                    }
            }

            //Controls for CPU Deselect
            else if (Input.GetButtonDown("ButtonAArrows") && l.is_arrowKeys_taken && ThisCarSelected && CPU.is_coin_cpu && CPU.is_player_near && m.playerID != CPU.CoinID)
            { CPUDeSelectCar(); }
        }

        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)
        {
            //if (Input.GetButtonDown("ButtonAWSDA") && l.is_wsda_taken && m.is_this_inside == true && !ThisCarSelected)
            //{ SelectCar(); }

            //else if (Input.GetButtonDown("ButtonXWSDA") && l.is_wsda_taken && ThisCarSelected)
            //{ DeSelectCar(); }

            //Controls for normal players Select
            if (Input.GetButtonDown("ButtonAWSDA") && l.is_wsda_taken && m.is_this_inside == true && !ThisCarSelected /*&& !CPU.is_coin_cpu*/ && m.playerID == CPU.CoinID)
            { SelectCar(); }

            //Controls for CPU Select
            else if (Input.GetButtonDown("ButtonAWSDA") && l.is_wsda_taken && m.is_this_inside == true && !ThisCarSelected && CPU.is_coin_cpu && m.playerID != CPU.CoinID)
            { CPUSelectCar(); }

            //Controls for normal players Deselect
            else if (Input.GetButtonDown("ButtonXWSDA") && l.is_wsda_taken && ThisCarSelected && m.playerID == CPU.CoinID)
            {
                if (m.playerID != 1)
                { DeSelectCar(); }

                else
                {

                    if (!C1.is_grabbed && !C2.is_grabbed && !C3.is_grabbed && !C4.is_grabbed && !l.isP1onButton)
                    {
                        ResetOverlay();
                        DeSelectCar();
                    }
                }
            }

            //Controls for CPU Deselect
            else if (Input.GetButtonDown("ButtonAWSDA") && l.is_wsda_taken && ThisCarSelected && CPU.is_coin_cpu && CPU.is_player_near && m.playerID != CPU.CoinID)
            { CPUDeSelectCar(); }
        }
    }

    void SelectCar() {

        t.transform.parent = Car.transform;               //The coin is given the car icon as a parent
        Car.GetComponent<Collider2D>().enabled = false;   //The collider of the car icon is deactivated. This car cannot be selected by other players
        ThisCarSelected = true;                           //Bool variable telling the player has selected this car
        m.is_this_ready = true;                           //This player has selected the car and is ready to GO
        l.num_ready_players++;                            // Increase the number of players who are ready to GO

        if (m.playerID == 1) { CPU.player.GetComponent<Collider2D>().enabled = true; }

        switch (m.playerID)   //Assign the car image to the correct player
            {
                case 1:
                    p1.CurrentCar = ThisCarImage;
                    p1.ImageSwapper();
                    p1.Hand.sprite = p1.hand_opened;
                    break;
                case 2:
                    p2.CurrentCar = ThisCarImage;
                    p2.ImageSwapper();
                    p2.Hand.sprite = p2.hand_opened;
                    break;
                case 3:
                    p3.CurrentCar = ThisCarImage;
                    p3.ImageSwapper();
                    p3.Hand.sprite = p3.hand_opened;
                    break;
                case 4:
                    p4.CurrentCar = ThisCarImage;
                    p4.ImageSwapper();
                    p4.Hand.sprite = p4.hand_opened;
                    break;
            }

            m.ThisPlayerCar = ThisCarType;
        
    }

    void CPUSelectCar()
    {

        t.transform.parent = Car.transform;
        Car.GetComponent<Collider2D>().enabled = false;
        ThisCarSelected = true;
        l.num_ready_players++;
        l.num_CPU_SelectedCar++;

        CPU.player.GetComponent<Collider2D>().enabled = true;
        CPU.is_car_selected = true;

        CPU.is_grabbed = false;

        switch (CPU.CoinID)   //Assign the car image to the correct player
        {
            case 1:
                p1.CurrentCar = ThisCarImage;
                p1.ImageSwapper();
                p1.Hand.sprite = p1.hand_opened;
                m1.ThisPlayerCar = ThisCarType;
                m1.is_this_ready = true;
                break;
            case 2:
                p2.CurrentCar = ThisCarImage;
                p2.ImageSwapper();
                p1.Hand.sprite = p1.hand_opened;
                m2.ThisPlayerCar = ThisCarType;
                m2.is_this_ready = true;
                break;
            case 3:
                p3.CurrentCar = ThisCarImage;
                p3.ImageSwapper();
                p1.Hand.sprite = p1.hand_opened;
                m3.ThisPlayerCar = ThisCarType;
                m3.is_this_ready = true;
                break;
            case 4:
                p4.CurrentCar = ThisCarImage;
                p4.ImageSwapper();
                p1.Hand.sprite = p1.hand_opened;
                m4.ThisPlayerCar = ThisCarType;
                m4.is_this_ready = true;
                break;
        }

    }

    void DeSelectCar()
    {
         
        t.transform.parent = old;                      //The coin is given the old parent back (the cursor)
        t.transform.localPosition = ParentPosition;   
        m.is_this_inside = false;
        m.ResetChildPosition(); 
        TextColorCar.color = Color.white;    
        ThisCarSelected = false;                       //The car is deselected
        isActive = false;    
        m.is_this_ready = false;
        l.num_ready_players--;                         //The number of players ready to go is decreased
        Car.GetComponent<Collider2D>().enabled = true; //The car Collider is re-activated

        

        switch (m.playerID) //Assign the "NO Car" image to the correct player
            {
                case 1:
                    p1.CurrentCar = p1.default_Empty;
                    p1.ImageSwapper();
                    p1.Hand.sprite = p1.hand_closed;
                    
                    break;
                case 2:
                    p2.CurrentCar = p2.default_Empty;
                    p2.ImageSwapper();
                    p2.Hand.sprite = p2.hand_closed;
                    
                    break;
                case 3:
                    p3.CurrentCar = p3.default_Empty;
                    p3.ImageSwapper();
                    p3.Hand.sprite = p3.hand_closed;
                    
                    break;
                case 4:
                    p4.CurrentCar = p4.default_Empty;
                    p4.ImageSwapper();
                    p4.Hand.sprite = p4.hand_closed;
                    
                    break;
            }

            m.ThisPlayerCar = 0;
    }

    void CPUDeSelectCar()

    {
      
        m.is_this_inside = false;
        TextColorCar.color = Color.white;
        ThisCarSelected = false;
        isActive = false;
        l.num_ready_players--;
        l.num_CPU_SelectedCar--;
        Car.GetComponent<Collider2D>().enabled = true;

        CPU.is_car_selected = false;
        CPU.player.GetComponent<Collider2D>().enabled = true;

        switch (CPU.CoinID) //Assign the "NO Car" image to the correct player
        {
            case 1:
                p1.CurrentCar = p1.default_Empty;
                p1.ImageSwapper();
                p1.Hand.sprite = p1.hand_closed;
                m1.ThisPlayerCar = 0;
                m1.is_this_ready = false;
                break;
            case 2:
                p2.CurrentCar = p2.default_Empty;
                p2.ImageSwapper();
                p1.Hand.sprite = p1.hand_closed;
                m2.ThisPlayerCar = 0;
                m2.is_this_ready = false;
                break;
            case 3:
                p3.CurrentCar = p3.default_Empty;
                p3.ImageSwapper();
                p1.Hand.sprite = p1.hand_closed;
                m3.ThisPlayerCar = 0;
                m3.is_this_ready = false;
                break;
            case 4:
                p4.CurrentCar = p4.default_Empty;
                p4.ImageSwapper();
                p1.Hand.sprite = p1.hand_closed;
                m4.ThisPlayerCar = 0;
                m4.is_this_ready = false;
                break;
        }
    }

    void ResetOverlay() {

        C1.is_player_near = false;
        C2.is_player_near = false;
        C3.is_player_near = false;
        C4.is_player_near = false;

        buttoncontroller1.SetOverlay(false);
        buttoncontroller2.SetOverlay(false);
        buttoncontroller3.SetOverlay(false);
        buttoncontroller4.SetOverlay(false);

        controlscontroller1.SetOverlay(false);
        controlscontroller2.SetOverlay(false);
        controlscontroller3.SetOverlay(false);
        controlscontroller4.SetOverlay(false);
    }

    void AddTempCar()

    {
        if (!l.hasGameStarted)
        {
            switch (CPU.CoinID)   //Assign the car image to the correct player
            {
                case 1:
                    p1.CurrentCar = ThisCarImageTemp;
                    p1.ImageSwapper();

                    break;
                case 2:
                    p2.CurrentCar = ThisCarImageTemp;
                    p2.ImageSwapper();

                    break;
                case 3:
                    p3.CurrentCar = ThisCarImageTemp;
                    p3.ImageSwapper();

                    break;
                case 4:
                    p4.CurrentCar = ThisCarImageTemp;
                    p4.ImageSwapper();

                    break;
            }
        }
    }

    void RemoveTempCar()

    {
        switch (CPU.CoinID) //Assign the "NO Car" image to the correct player
        {
            case 1:
                p1.CurrentCar = p1.default_Empty;
                p1.ImageSwapper();
                break;
            case 2:
                p2.CurrentCar = p2.default_Empty;
                p2.ImageSwapper();
                break;
            case 3:
                p3.CurrentCar = p3.default_Empty;
                p3.ImageSwapper();
                break;
            case 4:
                p4.CurrentCar = p4.default_Empty;
                p4.ImageSwapper();
                break;
        }

     }



    //This function is called in the script PlayerSelector controlling the panels. If a player is deactivated by a panel the coin, the cursor and their positions are re-set to default
    public void CheckPlayerActivation()
    {
        if (m.playerID == CPU.CoinID)
        {
            m.ThisPlayerCar = 0;
            t.transform.parent = old;                       //The coin is given its parent back (the cursor)
            t.transform.position = m.CoinPosition;          //It gets shifter to its original position
            TextColorCar.color = Color.white;               //The text color of the car icon is set to white
            Car.GetComponent<Collider2D>().enabled = true;  //The icon collider is set to active
            isActive = false;

            if (ThisCarSelected)
            {
                l.num_ready_players--;                          //The number of ready players is decreased
                m.ThisPlayerCar = 0;
            }
            ThisCarSelected = false;                        //The boolian for the car selection is set to false
        }

        else {

            switch (CPU.CoinID)
            {
                case 1:
                    m1.ThisPlayerCar = 0;
                    t.transform.parent = m1.transform;
                    t.transform.position = m1.CoinPosition;
                    m1.is_this_ready = false;
                    break;
                case 2:
                    m2.ThisPlayerCar = 0;
                    t.transform.parent = m2.transform;
                    t.transform.position = m2.CoinPosition;
                    m2.is_this_ready = false;
                    break;
                case 3:
                    m3.ThisPlayerCar = 0;
                    t.transform.parent = m3.transform;
                    t.transform.position = m3.CoinPosition;
                    m3.is_this_ready = false;
                    break;
                case 4:
                    m4.ThisPlayerCar = 0;
                    t.transform.parent = m4.transform;
                    t.transform.position = m4.CoinPosition;
                    m4.is_this_ready = false;
                    break;
            }

            TextColorCar.color = Color.white;               //The text color of the car icon is set to white
            Car.GetComponent<Collider2D>().enabled = true;  //The icon collider is set to active
            isActive = false;

            if (ThisCarSelected)
            {
                l.num_ready_players--;                          //The number of ready players is decreased
                l.num_CPU_SelectedCar--;
            }
            ThisCarSelected = false;                        //The boolian for the car selection is set to false

        }
    }
}
