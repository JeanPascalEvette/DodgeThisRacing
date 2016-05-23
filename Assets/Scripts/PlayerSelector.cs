using UnityEngine;
using UnityEngine.UI;

//This script is attached to each panel controlling the status of a player and its control types
public class PlayerSelector : MonoBehaviour
{
    //Instances of the text displayed on the panels
    public Text t;
    public Text cpuText;
    public Text Control_Type;

    //Misc int variables used in switch cases 
    public int switch_case = 1;
    public int Controls = 0;
    public int CPU_Controls = -1;
    public int PanelNumber;

    //Instances of the objects in the scene: Tokens, car icons and their scripts
    public GameObject playerCoin,playerToken;
    public CPUController CoinController;
    public IconCollider Car1, Car2, Car3, Car4;
    public LevelManager l;
    public MoveSelector m;
    public GameObject HandObject;
    
    string nameplayer;
    public bool is_CPU = false;

    //Sprites for the hand animations and car icons
    public Sprite default_Empty, CurrentCar,hand_closed,hand_opened,default_notActive;
    public Sprite CPU_Token, Player_Token;
    public Sprite NA, J1, J2, ArrowK, WASDIcon,CPUIcon;

    //Images objects to which the sprites will be swapped on
    public Image carImage;
    public Image Hand;
    public Image Token;
    public Image ControlIcon;
    
    //Instances of the controls and activation buttons on the panels
    private ButtonController buttonController;
    private ControlsController controlController;
    
    //Initialize variables, check the statuses of each panle and display the correct information
    void Start() {

        buttonController = GetComponentInChildren<ButtonController>();
        controlController = GetComponentInChildren<ControlsController>();
        CPU_Controls = 1;

        l.is_p1_active = true;

        carImage = transform.FindChild("CarImage").GetComponent<Image>();
        carImage.sprite = default_Empty;

        if   (!m.is_this_active) { carImage.sprite = default_notActive; }
        else { carImage.sprite = default_Empty; }


        switch (PanelNumber)
        {

            case 1:
                if (l.is_p1_active == true) { nameplayer = "1P"; Controls = 1; }
                break;

            case 2:
                nameplayer = "2P"; 
                break;

            case 3:
                nameplayer = "3P"; 
                break;

            case 4:
                nameplayer = "4P"; 
                break;
        }

        player_selector();

        if (m.is_this_active)
        {
            if      (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)      { Control_Type.text = "Joy1"; ControlIcon.sprite = J1; }
            else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)      { Control_Type.text = "Joy2"; ControlIcon.sprite = J2; }
            else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys) { Control_Type.text = "ArrowKeys"; ControlIcon.sprite = ArrowK; }
            else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)      { Control_Type.text = "WSDA"; ControlIcon.sprite = WASDIcon; }
            else                                                                     { Control_Type.text = "Not Assigned"; ControlIcon.sprite = NA; }
        }

        else { Control_Type.text = "Not Assigned"; ControlIcon.sprite = NA; }

    }

    //Check the status of each player and display the correct text
    void Update()
    {
        if (m.is_this_active)
        {
            if      (PanelNumber == 1 && switch_case != 2 && !is_CPU) { t.text = "P1"; }
            else if (PanelNumber == 2 && switch_case != 2 && !is_CPU) { t.text = "P2"; }
            else if (PanelNumber == 3 && switch_case != 2 && !is_CPU) { t.text = "P3"; }
            else if (PanelNumber == 4 && switch_case != 2 && !is_CPU) { t.text = "P4"; }
        }

    }

    void switch_reset() { switch_case = 0; }

    //Function to display the correct information about the player when its status (active, cpu, not-active) is changed
    public void player_selector()
    {
        if (switch_case < 4)
        {
            switch (switch_case)
            {
                case 1:
                    if (m.is_this_active == true) { t.text = nameplayer; }
                    else                          { t.text = "N/A"; }
                    break;

                case 2:
                    t.text = "CPU";
                    break;

                default:
                    t.text = "N/A";
                    switch_case = 0;
                    break;
            }
        }

        else { switch_reset(); }
    }

    //This funciton is called when the activation button on the panel is pressed (changes the player status to active, non active or cpu)
    public void PanelManager()
    {
        
        if (m.playerID != 1) //Player 1 will have a slightly different behaviour
        {
            if (!CoinController.is_grabbed)
            {
                //A new player can be activated only if it's the one right next to the highest currently activated player. e.g. if only player 1 is active you can activate player 2 but not player 3
                if (l.num_players == m.playerID || l.num_players == m.playerID - 1)
                {
                    switch_case++;
                    switch (switch_case)
                    {
                        case 1:
                            //Makes it an active player
                            if (l.num_players == m.playerID - 1 && !m.is_this_active)
                            {
                                Token.sprite = Player_Token;
                                HandObject.SetActive(true);

                                playerCoin.SetActive(true);
                                cpuText.text = "";
                                t.text = nameplayer;

                                Hand.sprite = hand_closed;
                                m.Hand.SetAsLastSibling();
                                if (m.playerID == 1) { m.GetComponent<Collider2D>().enabled = false; }


                                if (l.num_players < m.playerID) { l.num_players++; }

                                if (m.playerID == 1) { l.is_p1_active = true; }
                                else if (m.playerID == 2) { l.is_p2_active = true; }
                                else if (m.playerID == 3) { l.is_p3_active = true; }
                                else if (m.playerID == 4) { l.is_p4_active = true; }

                                m.is_this_active = true;
                                CPU_Controls = 1;
                                is_CPU = false;
                                carImage.sprite = default_Empty;
                            }

                            break;

                        case 2:
                            //Makes it a CPU
                            if (l.num_players == m.playerID - 1 && !m.is_this_active)
                            {
                                playerCoin.SetActive(true);
                                m.is_this_active = true;
                                l.num_players++;

                                if (m.playerID == 1) { l.is_p1_active = true; }
                                else if (m.playerID == 2) { l.is_p2_active = true; }
                                else if (m.playerID == 3) { l.is_p3_active = true; }
                                else if (m.playerID == 4) { l.is_p4_active = true; }
                            }

                            if (m.is_this_active)
                            {
                                cpuText.text = "";
                                t.text = "CPU";
                                is_CPU = true;
                                CPU_Controls = 2;
                                carImage.sprite = default_Empty;
                                Controls = 4;

                                HandObject.SetActive(false);
                                Token.sprite = CPU_Token;

                                l.num_CPU_Players++;
                                resetControlsCPU();
                                ControlManager();
                            }

                            break;

                        default:
                            //Deactivates it and resets all its variables
                            if (l.num_players == m.playerID && m.is_this_active)

                            {
                                print("deleting player");

                                playerCoin.SetActive(false);


                                if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)
                                {
                                    m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                                    l.is_joy1_taken = false;
                                }

                                else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)
                                {
                                    m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                                    l.is_joy2_taken = false;
                                }

                                else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys)
                                {
                                    m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                                    l.is_arrowKeys_taken = false;
                                }

                                else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)
                                {
                                    m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                                    l.is_wsda_taken = false;
                                }

                                if (m.playerID == 1) { l.is_p1_active = false; }
                                else if (m.playerID == 2) { l.is_p2_active = false; }
                                else if (m.playerID == 3) { l.is_p3_active = false; }
                                else if (m.playerID == 4) { l.is_p4_active = false; }

                                l.num_players--;
                                l.num_CPU_Players--;
                                m.is_this_active = false;
                                is_CPU = false;
                                CoinController.is_car_selected = false;

                            }

                            switch_case = 0;
                            playerCoin.transform.position = m.playerPosition;
                            carImage.sprite = default_notActive;

                            if      (Car1.ThisCarSelected == true && CoinController.CoinID == Car1.ID) { Car1.CheckPlayerActivation(); }
                            else if (Car2.ThisCarSelected == true && CoinController.CoinID == Car2.ID) { Car2.CheckPlayerActivation(); }
                            else if (Car3.ThisCarSelected == true && CoinController.CoinID == Car3.ID) { Car3.CheckPlayerActivation(); }
                            else if (Car4.ThisCarSelected == true && CoinController.CoinID == Car4.ID) { Car4.CheckPlayerActivation(); }

                            CPU_Controls = 0;
                            t.text = "N/A";
                            cpuText.text = "";

                            if (m.playerID == 1)
                            {
                                buttonController.SetOverlay(false);
                                controlController.SetOverlay(false);
                            }

                            Controls = 4;
                            ControlManager();

                            break;
                    }
                }
                //If this is a middle player (e.g. player 2 when 3 players are active) only switches between CPU and active status. Player cannot be disabled
                else if (m.is_this_active)
                {

                    CPU_Controls++;

                    switch (CPU_Controls)
                    {
                        case 1:

                            is_CPU = false;
                            resetControlsCPU();
                            Controls = 4;
                            ControlManager();
                            Hand.sprite = hand_closed;
                            cpuText.text = "";
                            t.text = nameplayer;
                            switch_case = 1;
                            Token.sprite = Player_Token;
                            HandObject.SetActive(true);
                            CPU_Controls = 1;
                            l.num_CPU_Players--;

                            break;

                        case 2:
                            cpuText.text = "";
                            t.text = "CPU";
                            is_CPU = true;
                            CPU_Controls = 0;
                            switch_case = 2;
                            resetControlsCPU();
                            Controls = 4;
                            ControlManager();

                            l.num_CPU_Players++;
                            HandObject.SetActive(false);
                            Token.sprite = CPU_Token;

                            m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                            break;

                        default:
                            switch_case = 0;
                            CPU_Controls = 0;
                            Controls = 4;
                            is_CPU = false;
                            break;
                    }
                }
            }
        }

        //Player 1 can also not be deactivated as it controls other players' statuses and activation
        else {

            CPU_Controls++;

            switch (CPU_Controls)
            {
                case 1:
                    cpuText.text = "";
                    t.text = nameplayer;
                    is_CPU = false;
                    switch_case = 1;
                    Token.sprite = Player_Token;
                    break;

                case 2:
                    cpuText.text = "";
                    t.text = "CPU";
                    is_CPU = true;
                    CPU_Controls = 0;
                    Token.sprite = CPU_Token;
                    switch_case = 2;
                    break;

                default:
                    switch_case = 0;
                    CPU_Controls = 0;
                    is_CPU = false;
                    break;
            }
        }
    }

    //Fnction to swap the panle images
    public void ImageSwapper() {

        carImage.sprite = CurrentCar;
    }

    //Resets some variables and statuses the a certain player is made a CPU
    void resetControlsCPU()

    {
        if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)
        {
            m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
            l.is_joy1_taken = false;
        }

        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)
        {
            m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
            l.is_joy2_taken = false;
        }

        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys)
        {
            m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
            l.is_arrowKeys_taken = false;
        }

        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)
        {
            m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
            l.is_wsda_taken = false;
        }

        Control_Type.text = "Not Assigned";
        carImage.sprite = default_Empty;
        playerCoin.transform.position = m.playerPosition;

        if      (Car1.ThisCarSelected == true && CoinController.CoinID == Car1.ID) { Car1.CheckPlayerActivation(); }
        else if (Car2.ThisCarSelected == true && CoinController.CoinID == Car2.ID) { Car2.CheckPlayerActivation(); }
        else if (Car3.ThisCarSelected == true && CoinController.CoinID == Car3.ID) { Car3.CheckPlayerActivation(); }
        else if (Car4.ThisCarSelected == true && CoinController.CoinID == Car4.ID) { Car4.CheckPlayerActivation(); }

        CPU_Controls = 0;
        m.Hand.SetAsLastSibling();
        CoinController.is_car_selected = false;

        if (m.playerID == 1)
        {
            buttonController.SetOverlay(false);
            controlController.SetOverlay(false);
        }
    }

    //This function is called when the control scheme button is pressed. If it's not already in use the relative control scheme is assigned to the player unless is a CPU
    public void ControlManager() {

        if (m.is_this_active && !m.is_this_ready)
        {
            Controls++;

            switch (Controls)
            {
                case 1:
                    //Player 1 won't be given a dead control scheme in order to always be able to control the other players tokens 
                    if (is_CPU && m.playerID!=1)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                        Control_Type.text = "AI Controller";
                        ControlIcon.sprite = CPUIcon;
                        return;
                    }
                    else
                    {
                        if (l.is_joy1_taken == false)
                        {
                            m.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy1;
                            l.is_joy1_taken = true;
                            Control_Type.text = "Joy1";
                            ControlIcon.sprite = J1;

                        }

                        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1) { ControlIcon.sprite = J1; }
                        else
                        {
                            m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                            Control_Type.text = "Joy1 (N/A)";
                            ControlIcon.sprite = NA;
                        }
                    }

                    break;

                case 2:
                    if (is_CPU && m.playerID != 1)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                        Control_Type.text = "AI Controller";
                        ControlIcon.sprite = CPUIcon;
                        return;
                    }
                    else
                    {
                        if (l.is_joy2_taken == false)
                        {
                            m.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy2;
                            l.is_joy2_taken = true;
                            Control_Type.text = "Joy2";
                            ControlIcon.sprite = J2;
                        }

                        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2) { ControlIcon.sprite = J2; }
                        else
                        {
                            m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                            Control_Type.text = "Joy2 (N/A)";
                            ControlIcon.sprite = NA;
                        }
                    }
                    break;

                case 3:
                    if (is_CPU && m.playerID != 1)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                        Control_Type.text = "AI Controller";
                        ControlIcon.sprite = CPUIcon;
                        return;
                    }
                    else
                    {
                        if (l.is_arrowKeys_taken == false)
                        {
                            m.ThisPlayerControl = MoveSelector.ControlTypesHere.ArrowKeys;
                            l.is_arrowKeys_taken = true;
                            Control_Type.text = "ArrowKeys";
                            ControlIcon.sprite = ArrowK;
                        }

                        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys) { ControlIcon.sprite = ArrowK; }
                        else
                        {
                            m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                            Control_Type.text = "Arrows (N/A)";
                            ControlIcon.sprite = NA;
                        }
                    }
                    break;

                case 4:
                    if (is_CPU && m.playerID != 1)
                    {
                        m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                        Control_Type.text = "AI Controller";
                        ControlIcon.sprite = CPUIcon;
                        return;
                    }
                    else
                    {
                        if (l.is_wsda_taken == false)
                        {
                            m.ThisPlayerControl = MoveSelector.ControlTypesHere.WSDA;
                            l.is_wsda_taken = true;
                            Control_Type.text = "WSDA";
                            ControlIcon.sprite = WASDIcon;
                        }

                        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA) { ControlIcon.sprite = WASDIcon; }
                        else
                        {
                            m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
                            Control_Type.text = "WSDA (N/A)";
                            ControlIcon.sprite = NA;
                        }
                    }
                    break;

                default:
                    m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;

                    if (is_CPU && m.playerID != 1) { ControlIcon.sprite = CPUIcon; Control_Type.text = "AI Controller"; }
                    else { ControlIcon.sprite = NA; Control_Type.text = "Not Assigned"; }
                    Controls = 0;
                    break;
            }
        }

        else if (!m.is_this_ready) {

            m.ThisPlayerControl = MoveSelector.ControlTypesHere.NotAssigned;
            if   (is_CPU && m.playerID != 1) { ControlIcon.sprite = CPUIcon; Control_Type.text = "AI Controller"; }
            else { ControlIcon.sprite = NA; Control_Type.text = "Not Assigned"; }
            Controls = 0;
        }
    }
}
