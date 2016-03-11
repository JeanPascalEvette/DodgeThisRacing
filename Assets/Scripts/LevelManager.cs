using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public GameObject player;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    MoveSelector player1move;
    MoveSelector player2move;
    MoveSelector player3move;
    MoveSelector player4move;

    MoveSelector newPlayer;

    //GameObject newPlayer;

    public Text TextColorGo;
    float move_player = 5.0f;
    bool Selected = false;
    public bool is_inside = false;

    bool is_2created = false;
    int num_players;

    float translationX;
    float translationY;

    Text TextColorCar;
    Text Player2text;

    
    bool is_joy1_taken, is_arrowKeys_taken, is_wsda_taken, is_joy2_taken = false;
    bool is_joy1_used, is_arrowKeys_used, is_wsda_used, is_joy2_used = false;


    void Start()
    {
        num_players = 1;
        TextColorGo = GameObject.FindWithTag("Go").GetComponent<Text>();

        player1move = player.GetComponent<MoveSelector>();
        player2move = player2.GetComponent<MoveSelector>();
        player3move = player3.GetComponent<MoveSelector>();
        player4move = player4.GetComponent<MoveSelector>();

        player.SetActive(true);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);

        player1move.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy1;
        is_joy1_taken = true;

        num_players = 1;

    }


    void Update()
    {
        
  if ((((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) 
         && !is_wsda_taken) 
         
                                                            ||

        ((Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        && !is_arrowKeys_taken)) 
        
        
        && num_players < 4)

        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            { is_wsda_used = true;}

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            { is_arrowKeys_used = true;}

            num_players++;
            Create_Player();

        }


        //Select the car by pressing X
        if ((Input.GetKey(KeyCode.X) || Input.GetButton("Jump")) && is_inside)  { Selected = true; }

   
        //Deselect the car by pressing Z
        if ((Input.GetKey(KeyCode.Z) || Input.GetButton("Fire1")) && is_inside && (Selected))
        {
            Selected = false;
            TextColorGo.color = Color.white;
        }

        //If all players have selected their cars the GO text becomes green
        if (Selected && is_inside)
        {
            TextColorGo.color = Color.green;

            //If B is pressend when GO text is green the main game is loaded
            if (Input.GetKey(KeyCode.B) || Input.GetButton("Submit")) {LoadLevel("main2"); }
        }

    }

    //Function to load the main game scene
    public void LoadLevel(string name)
    {
       
        SceneManager.LoadScene(name);

    }


    void Create_Player()
    {
        switch (num_players) {

            case 2:
                player2.SetActive(true);
                newPlayer = player2move;
                setControlScheme();
                break;

            case 3:
                player3.SetActive(true);
                newPlayer = player3move;
                setControlScheme();
                break;
            case 4:
                player4.SetActive(true);
                newPlayer = player4move;
                setControlScheme();
                break;

            default:
                break;

        }


    }


    void setControlScheme()
    {
        
        if  (!is_arrowKeys_taken && is_arrowKeys_used)

        {

            newPlayer.ThisPlayerControl = MoveSelector.ControlTypesHere.ArrowKeys;
            is_arrowKeys_taken = true;
            is_arrowKeys_used = false;

        }    

        else if (!is_wsda_taken && is_wsda_used)

        {
            newPlayer.ThisPlayerControl = MoveSelector.ControlTypesHere.WSDA;
            is_wsda_taken = true;
            is_wsda_used = false;

        }
    
    }

}