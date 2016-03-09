using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public GameObject player, player2, player3, player4;
    public Text TextColorGo;
    float move_player = 5.0f;
    bool Selected = false;
    public bool is_inside = false;
    bool isActive = true;
    public int i = 0; //counts added player instances
   
 
    Text TextColorCar;

    void Start()
    {
        TextColorGo = GameObject.FindWithTag("Go").GetComponent<Text>();
        //player = GameObject.FindWithTag("PlayerMenu");
        player.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
       // player2 = GameObjectFindWithTag("PlayerMenu2");
    }

    public void ShowPlayer()
    {
        i++;
            if (i == 1)
            {
                player.SetActive(true);
            }
            if (i == 2)
            {
                player2.SetActive(true);
            }
            if (i == 3)
            {
            player3.SetActive(true);
            }
            if (i==4)
            {
            player4.SetActive(true);
            }
     
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
        
    }

    }