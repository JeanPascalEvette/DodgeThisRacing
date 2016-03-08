using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public GameObject player;
    public Text TextColorGo;
    float move_player = 5.0f;
    bool Selected = false;
    public bool is_inside = false;
 
    Text TextColorCar;

    void Start()
    {
        TextColorGo = GameObject.FindWithTag("Go").GetComponent<Text>();
        player = GameObject.FindWithTag("PlayerMenu");
      
    }


    void Update()
    {
        if (!Selected) { HandleMovement();}

        //Select the car by pressing X
        if (Input.GetKey(KeyCode.X) && is_inside)  { Selected = true; }

        //Deselect the car by pressing A
        if (Input.GetKey(KeyCode.A) && is_inside && (Selected))
        {
            Selected = false;
            TextColorGo.color = Color.white;
        }

        //If all players have selected their cars the GO text becomes green
        if (Selected && is_inside)
        {
            TextColorGo.color = Color.green;

            //If B is pressend when GO text is green the main game is loaded
            if (Input.GetKey(KeyCode.B)) {LoadLevel(name); }
        }

    }

    //Function to load the main game scene
    public void LoadLevel(string name)
    {
        name = "main2";
        SceneManager.LoadScene(name);

    }

    //Function to move the player
    void HandleMovement()
    {
        
            if (Input.GetKey(KeyCode.RightArrow)){ player.transform.Translate(move_player, 0, 0); }
            if (Input.GetKey(KeyCode.LeftArrow)) { player.transform.Translate(-move_player, 0, 0);}
            if (Input.GetKey(KeyCode.UpArrow))   { player.transform.Translate(0, move_player, 0); }
            if (Input.GetKey(KeyCode.DownArrow)) { player.transform.Translate(0, -move_player, 0);}
        
    }

    }