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

    public Text TextColorGo;
    float move_player = 5.0f;

    bool Selected = false;
    public bool is_inside = false; // Delete this later

    int num_players;

    public int num_ready_players;

    float translationX;
    float translationY;

    Text TextColorCar;
    Text Player2text;

    
    bool is_joy1_taken, is_arrowKeys_taken, is_wsda_taken, is_joy2_taken = false;
    bool is_joy1_used, is_arrowKeys_used, is_wsda_used, is_joy2_used = false;


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

        player1move.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy1;
        is_joy1_taken = true;

    }


    void Update()
    {
        Debug.Log(num_ready_players);
        
  if (
      
       (
            
            
       ((Input.GetAxis("VerticalWSDA") != 0 || Input.GetAxis("HorizontalWSDA") != 0) && !is_wsda_taken) 
         
                                                            ||

       ((Input.GetAxis("VerticalArrows") != 0 || Input.GetAxis("HorizontalArrows") != 0) && !is_arrowKeys_taken)

                                                            ||
        
       ((Input.GetAxis("VerticalJoyStickLeft2") !=0 || Input.GetAxis("HorizontalJoyStickLeft2") != 0) && !is_joy2_taken)
       
       
       )

                                                            && 
                                                            
                                                      num_players < 4
       )

        {
            if       (Input.GetAxis("VerticalWSDA") != 0 || Input.GetAxis("HorizontalWSDA") != 0)

                     { is_wsda_used = true; }


            else  if (Input.GetAxis("VerticalArrows") != 0 || Input.GetAxis("HorizontalArrows") != 0)

                     { is_arrowKeys_used = true; }


            else if  (Input.GetAxis("VerticalJoyStickLeft2") != 0 || Input.GetAxis("HorizontalJoyStickLeft2") != 0)

                     { is_joy2_used = true; }


            num_players++;
            Create_Player();

        }

        //If all players have selected their cars the GO text becomes green
        if (num_players == num_ready_players)
        {
            TextColorGo.color = Color.green;

            //If B or Start is pressed when GO text is green the main game is loaded
            if (Input.GetButtonDown("Submit") || Input.GetButtonDown("SubmitJoystick")) { LoadLevel("main2"); }
        }

        else { TextColorGo.color = Color.white; }

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

        else if (!is_joy2_taken && is_joy2_used)

        {
            newPlayer.ThisPlayerControl = MoveSelector.ControlTypesHere.Joy2;
            is_joy2_taken = true;
            is_joy2_used = false;

        }

    }

}