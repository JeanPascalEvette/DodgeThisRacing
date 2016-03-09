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
 
    Text TextColorCar;
    Text Player2text;

    void Start()
    {
        num_players = 1;
        TextColorGo = GameObject.FindWithTag("Go").GetComponent<Text>();
        player = GameObject.FindWithTag("PlayerMenu");
      
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) &&!is_2created) { Create_Player(); }

        if (!Selected) { HandleMovement();}

        //Select the car by pressing X
        if (Input.GetKey(KeyCode.X) && is_inside)  { Selected = true; }

        //Deselect the car by pressing Z
        if (Input.GetKey(KeyCode.Z) && is_inside && (Selected))
        {
            Selected = false;
            TextColorGo.color = Color.white;
        }

        //If all players have selected their cars the GO text becomes green
        if (Selected && is_inside)
        {
            TextColorGo.color = Color.green;

            //If B is pressend when GO text is green the main game is loaded
            if (Input.GetKey(KeyCode.B)) {LoadLevel("main2"); }
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
        
            if (Input.GetKey(KeyCode.RightArrow)){ player.transform.Translate(move_player, 0, 0); }
            if (Input.GetKey(KeyCode.LeftArrow)) { player.transform.Translate(-move_player, 0, 0);}
            if (Input.GetKey(KeyCode.UpArrow))   { player.transform.Translate(0, move_player, 0); }
            if (Input.GetKey(KeyCode.DownArrow)) { player.transform.Translate(0, -move_player, 0);}

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