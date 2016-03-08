using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public GameObject player;
    public Text TextColorGo;
    public Text TextColorCar1;
    public Text TextColorCar2;
    public Text TextColorCar3;
    public Text TextColorCar4;
    Text test;
    bool is_inside = false;
    float move_player = 5.0f;
    bool notSelected = true;

    void Start()
    {
        TextColorGo = GameObject.FindWithTag("Go").GetComponent<Text>();
        TextColorCar1 = GameObject.FindWithTag("Car1").GetComponent<Text>();
        TextColorCar2 = GameObject.FindWithTag("Car2").GetComponent<Text>();
        TextColorCar3 = GameObject.FindWithTag("Car3").GetComponent<Text>();
        TextColorCar4 = GameObject.FindWithTag("Car4").GetComponent<Text>();
        player = GameObject.FindWithTag("PlayerMenu");
    }


    void Update()
    {
        HandleMovement();

        //Select the car by pressing X
        if (Input.GetKey(KeyCode.X) && is_inside)  { notSelected = false; }

        //Deselect the car by pressing A
        if (Input.GetKey(KeyCode.A) && is_inside && (!notSelected))
        {
            notSelected = true;
            TextColorGo.color = Color.white;
        }

        //If all players have selected their cars the GO text becomes green
        if (notSelected == false)
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

    //Function to detect enter in the trigger area of the Car icon selection
    void OnTriggerEnter2D(Collider2D trigger)
    {


        test = trigger.GetComponent<Text>();
        test.color = Color.yellow;
            is_inside = true;
            print("Trigger");
     
        
        
    }


    //Function to detect exit from the trigger area of the Car selection icon
    void OnTriggerExit2D(Collider2D trigger)
    {
        print("EXIT");
        if (trigger.gameObject.tag == "Car1")
        {
            TextColorCar1.color = Color.white;
            is_inside = false;
        }

    }

    //Function to move the player
    void HandleMovement()
    {
        if (notSelected)
        {
            if (Input.GetKey(KeyCode.RightArrow)){ player.transform.Translate(move_player, 0, 0); }
            if (Input.GetKey(KeyCode.LeftArrow)) { player.transform.Translate(-move_player, 0, 0);}
            if (Input.GetKey(KeyCode.UpArrow))   { player.transform.Translate(0, move_player, 0); }
            if (Input.GetKey(KeyCode.DownArrow)) { player.transform.Translate(0, -move_player, 0);}
        }
    }

    }