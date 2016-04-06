using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Script that controls the overall Stats of the GUI
public class LevelManager : MonoBehaviour
{
    //The GameObjects of the Cursors for each player (Assigned in the Inspector)
    public GameObject player;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    //The Gameobjects of the Panels referring to each player (Assigned in the Inspector)
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel4;

    //Objects to access the script MoveSelector (handling how the cursors move and behave) for each player (Assigned in the Inspector)
    MoveSelector player1move;
    MoveSelector player2move;
    MoveSelector player3move;
    MoveSelector player4move;

    MoveSelector newPlayer; //A generic MoveSelector object that gets assigned to a player on its activation
    PlayerSelector ps;      //An instance of the script PlayerSelector (handles how the panels behave)

    public Text TextColorGo;       //An instance of the Text Element of the GO Object
   
    public int num_players;        //The number of active players
    public int num_ready_players;  //The number of players that have selected a car and are ready to start

    public bool is_joy1_taken, is_arrowKeys_taken, is_wsda_taken, is_joy2_taken = false; //Bool variables to check if a control type has already been assigned or not
    public bool is_joy1_used, is_arrowKeys_used, is_wsda_used, is_joy2_used = false;     //Bool variables to check if a control type is being used at that moment
    public bool is_p1_active, is_p2_active, is_p3_active, is_p4_active = false;          //Bool Variable to check if each player is active or not

    //Initialization. Player 1 is set active by default and controllable with Joystick 1
    void Start()
    {
        num_players = 1;
        num_ready_players = 0;

        TextColorGo = GameObject.FindWithTag("Go").GetComponent<Text>();

        player1move = player.GetComponent<MoveSelector>();
        player2move = player2.GetComponent<MoveSelector>();
        player3move = player3.GetComponent<MoveSelector>();
        player4move = player4.GetComponent<MoveSelector>();

        player.SetActive(true);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);

        is_p1_active = true;
        player1move.is_this_active = true;

        player1move.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy1;
        is_joy1_taken = true;
    }


    void Update()
    {
        //Debug messages (Delete Later on)
        print("num players: "   + num_players);
        print("ready players: " + num_ready_players);

        CheckWhichInput();    //Function to detect a control input
        CheckControlinUse();  //Function to keep the current control scheme used by each player up to date

        //If all players have selected their cars the GO text becomes green
        if (num_players == num_ready_players && num_players > 0)
        {
            TextColorGo.color = Color.green;

            //If Enter or Start is pressed when GO text is green the main game is loaded
            if (Input.GetButtonDown("Submit")) {LoadLevel("Game"); }
        }

        else { TextColorGo.color = Color.white; }
    }

    //Function to load the main game scene
    public void LoadLevel(string name)
    {
        PlayerData[] _PlayerData = new PlayerData[num_players]; //Change The player1move to an array
        for(int i = 0; i < num_players; i++)
        {
            switch (i)
            {
                case 0:
                    player1move.CreatePlayerData();
                    _PlayerData[i] = player1move.ThisPlayerData;
                    break;
                case 1:
                    player2move.CreatePlayerData();
                    _PlayerData[i] = player2move.ThisPlayerData;
                    break;
                case 2:
                    player3move.CreatePlayerData();
                    _PlayerData[i] = player3move.ThisPlayerData;
                    break;
                case 3:
                    player4move.CreatePlayerData();
                    _PlayerData[i] = player4move.ThisPlayerData;
                    break;
            }
        }

        Data.selectCars(_PlayerData);
        SceneManager.LoadScene(name);
    }

    //Function to detect a control input
    void CheckWhichInput() {

        //Can probably be cut down to less code lines
        if (

       (


       ((Input.GetAxis("VerticalWSDA") != 0 || Input.GetAxis("HorizontalWSDA") != 0) && !is_wsda_taken)

                                                            ||

       ((Input.GetAxis("VerticalArrows") != 0 || Input.GetAxis("HorizontalArrows") != 0) && !is_arrowKeys_taken)

                                                            ||

       ((Input.GetAxis("VerticalJoyStickLeft2") != 0 || Input.GetAxis("HorizontalJoyStickLeft2") != 0) && !is_joy2_taken)

                                                            ||

       ((Input.GetAxis("VerticalJoyStickLeft1") != 0 || Input.GetAxis("HorizontalJoyStickLeft1") != 0) && !is_joy1_taken)


       )

                                                            &&

                                                      num_players <= 4
       )

        {
            if (Input.GetAxis("VerticalWSDA") != 0 || Input.GetAxis("HorizontalWSDA") != 0)

            { is_wsda_used = true; }


            else if (Input.GetAxis("VerticalArrows") != 0 || Input.GetAxis("HorizontalArrows") != 0)

            { is_arrowKeys_used = true; }


            else if (Input.GetAxis("VerticalJoyStickLeft2") != 0 || Input.GetAxis("HorizontalJoyStickLeft2") != 0)

            { is_joy2_used = true; }

            else if (Input.GetAxis("VerticalJoyStickLeft1") != 0 || Input.GetAxis("HorizontalJoyStickLeft1") != 0)

            { is_joy1_used = true; }

            Create_Player();

        }

    }

    //Function that creates the player and assigns a control scheme to it as detected by the input or if the player is already active it just assigns the control scheme detected
    void Create_Player()
    {
        //If some Players are already active it assigns a control scheme to them (works progressively from player 1 to 4)
        if (is_p1_active == true && player1move.ThisPlayerControl == MoveSelector.ControlTypesHere.NotAssigned)
        {
            newPlayer = player1move;
            ps = panel1.GetComponent<PlayerSelector>();
        }

        else if (is_p2_active == true && player2move.ThisPlayerControl == MoveSelector.ControlTypesHere.NotAssigned)
        {
            newPlayer = player2move;
            ps = panel2.GetComponent<PlayerSelector>();
        }

        else if (is_p3_active == true && player3move.ThisPlayerControl == MoveSelector.ControlTypesHere.NotAssigned)
        {
            newPlayer = player3move;
            ps = panel3.GetComponent<PlayerSelector>();
        }

        else if (is_p4_active == true && player4move.ThisPlayerControl == MoveSelector.ControlTypesHere.NotAssigned)
        {
            newPlayer = player4move;
            ps = panel4.GetComponent<PlayerSelector>();
        }

        //If there are no active players it creates a new player and assigns the control scheme detected
        else {

            num_players++;

            switch (num_players)
        {

            case 1:
                player.SetActive(true);
                newPlayer = player1move;
                is_p1_active = true;
                ps = panel1.GetComponent<PlayerSelector>();
                break;

            case 2:
                player2.SetActive(true);
                newPlayer = player2move;
                is_p2_active = true;
                ps = panel2.GetComponent<PlayerSelector>();
                break;

            case 3:
                player3.SetActive(true);
                newPlayer = player3move;
                is_p3_active = true;
                ps = panel3.GetComponent<PlayerSelector>();
                break;

            case 4:
                player4.SetActive(true);
                newPlayer = player4move;
                is_p4_active = true;
                ps = panel4.GetComponent<PlayerSelector>();
                break;

            default:
                break;

        }

            newPlayer.is_this_active = true;
            ps.CPU_Controls = 1;
            ps.switch_case  = 1;
        }

        setControlScheme();
    }

    //Function that assigns a control Scheme to a specific player as detected by the input
    void setControlScheme()
    {
        
        //If the Control Scheme is not already in use and is the one being currently detected assign it to the Player
        if (!is_arrowKeys_taken && is_arrowKeys_used)

        {
            newPlayer.ThisPlayerControl = MoveSelector.ControlTypesHere.ArrowKeys;
            ps.Controls = 3;
            is_arrowKeys_taken = true;
            is_arrowKeys_used = false;
        }

        else if (!is_wsda_taken && is_wsda_used)

        {
            newPlayer.ThisPlayerControl = MoveSelector.ControlTypesHere.WSDA;
            ps.Controls = 4;
            is_wsda_taken = true;
            is_wsda_used = false;
        }

        else if (!is_joy2_taken && is_joy2_used)

        {
            newPlayer.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy2;
            ps.Controls = 2;
            is_joy2_taken = true;
            is_joy2_used = false;
        }

        else if (!is_joy1_taken && is_joy1_used)

        {
            newPlayer.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy1;
            ps.Controls = 1;
            is_joy1_taken = true;
            is_joy1_used = false;
        }
    }

    //Keep the control schemes in use updated
    void CheckControlinUse() {

        if (    player1move.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1
                               ||
                player2move.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1
                               ||
                player3move.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1
                               ||
                player4move.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1

           )

             { is_joy1_taken = true; }

        else { is_joy1_taken = false; }


        if (player1move.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2
                               ||
                player2move.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2
                               ||
                player3move.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2
                               ||
                player4move.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2

           )

             { is_joy2_taken = true; }

        else { is_joy2_taken = false; }


        if (player1move.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys
                            ||
             player2move.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys
                            ||
             player3move.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys
                            ||
             player4move.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys

        )
             { is_arrowKeys_taken = true; }

        else { is_arrowKeys_taken = false; }

        if (player1move.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA
                            ||
             player2move.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA
                            ||
             player3move.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA
                            ||
             player4move.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA

        )

             { is_wsda_taken = true; }

        else { is_wsda_taken = false; }

    }
}