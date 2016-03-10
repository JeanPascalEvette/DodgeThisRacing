using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public GameObject player;
    GameObject newPlayer;

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

    

    public enum ControlTypes {

        ArrowKeys,
        WSDA,
        Joy1,
        Joy2

    }

    ControlTypes player1control;
    ControlTypes player2control;
    ControlTypes player3control;
    ControlTypes player4control;

    void Start()
    {
        num_players = 1;
        TextColorGo = GameObject.FindWithTag("Go").GetComponent<Text>();
        player = GameObject.FindWithTag("PlayerMenu");

        player1control = ControlTypes.Joy1;
        player2control = ControlTypes.ArrowKeys;

    }


    void Update()
    {
        float translationY = Input.GetAxis("Vertical") * move_player;
        float translationX = Input.GetAxis("Horizontal") * move_player;
        player.transform.Translate(0, translationY, 0);
        player.transform.Translate(translationX, 0, 0);

        if (Input.GetKeyDown(KeyCode.W) &&!is_2created) { Create_Player(); }

        if (!Selected) { HandleMovement();}

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

    //Function to move the player
    void HandleMovement()
    {
        if (player1control == ControlTypes.ArrowKeys)
        {

            if (Input.GetKey(KeyCode.RightArrow)) { player.transform.Translate(move_player, 0, 0); }
            if (Input.GetKey(KeyCode.LeftArrow)) { player.transform.Translate(-move_player, 0, 0); }
            if (Input.GetKey(KeyCode.UpArrow)) { player.transform.Translate(0, move_player, 0); }
            if (Input.GetKey(KeyCode.DownArrow)) { player.transform.Translate(0, -move_player, 0); }

        }

        else if (player1control == ControlTypes.Joy1) {

            translationY = Input.GetAxis("Vertical") * move_player;
            translationX = Input.GetAxis("Horizontal") * move_player;
            player.transform.Translate(0, translationY, 0);
            player.transform.Translate(translationX, 0, 0);

        }

        if (is_2created)
        {
            if (Input.GetKey(KeyCode.D)) { newPlayer.transform.Translate(move_player, 0, 0); }
            if (Input.GetKey(KeyCode.A)) { newPlayer.transform.Translate(-move_player, 0, 0); }
            if (Input.GetKey(KeyCode.W)) { newPlayer.transform.Translate(0, move_player, 0); }
            if (Input.GetKey(KeyCode.S)) { newPlayer.transform.Translate(0, -move_player, 0); }

        }

    }

    void Create_Player()
    {
         newPlayer = (GameObject)Instantiate(player, new Vector3(412, 96, 0), Quaternion.identity);
         newPlayer.transform.parent = GameObject.FindWithTag("GuiCanvas").transform;
         Player2text = newPlayer.GetComponent<Text>();
         Player2text.text = "2P";
         is_2created = true;
    }

    }