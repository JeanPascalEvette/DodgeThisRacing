using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IconCollider : MonoBehaviour {


    Text TextColorCar;
    LevelManager l;

    MoveSelector m; // Script attached to the player objects (player, player2 etc.)

    ButtonCollider b; // Scripts attached to the coins objects (coin1, coin2 etc.)

    GameObject t;

   // GameObject c1, c2, c3, c4;

    Transform old;

    private bool isActive;

    public GameObject Car;

    void Start()
    {
        l = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
        isActive = false;
        
    }

    void Update() {

        if (isActive) { checkControlType(); }
        
    }


    //Function to detect enter in the trigger area of the Car icon selection
    void OnTriggerEnter2D(Collider2D trigger)
    {
        isActive = true;

        TextColorCar = this.GetComponent<Text>();
        TextColorCar.color = Color.yellow;

        m = trigger.GetComponentInParent<MoveSelector>();

        b = trigger.GetComponent<ButtonCollider>();
   
        m.is_this_inside = true;
   
        t = trigger.gameObject;

        old = t.transform.parent;

        print("Enter Car Gui" + m.is_this_inside);

        
    }

    //Function to detect exit from the trigger area of the Car selection icon
    void OnTriggerExit2D(Collider2D trigger)
    {
        isActive = false;
        TextColorCar = this.GetComponent<Text>();
        TextColorCar.color = Color.white;

        m.is_this_inside = false;

        

        print("Exit Car Gui " + m.is_this_inside);

    }

  
    void checkControlType() {

        if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)

        {

            if (Input.GetButtonDown("ButtonAJoyStick1") && l.is_joy1_taken && m.is_this_inside == true)
            {

                t.transform.parent = Car.transform;
                Car.GetComponent<Collider2D>().enabled = false;
                print(Car.name);
            }

            if (Input.GetButtonDown("ButtonXJoyStick1") && l.is_joy1_taken && m.is_this_inside == true && b.is_player_near)
            {

                t.transform.parent = old;
                Car.GetComponent<Collider2D>().enabled = true;

            }

        }


        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)

        {

            if (Input.GetButtonDown("ButtonAJoyStick2") && l.is_joy2_taken && m.is_this_inside == true)
            {

                t.transform.parent = Car.transform;
                Car.GetComponent<Collider2D>().enabled = false;
                print("P2 Select");
            }

            if (Input.GetButtonDown("ButtonXJoyStick2") && l.is_joy2_taken && m.is_this_inside == true && b.is_player_near)
            {

                t.transform.parent = old;
                Car.GetComponent<Collider2D>().enabled = true;
                print("P2 Taken");
            }

        }


    }

}
