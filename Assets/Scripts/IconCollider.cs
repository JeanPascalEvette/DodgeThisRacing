using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IconCollider : MonoBehaviour {


    Text TextColorCar;
    LevelManager l;

    MoveSelector m; // Script attached to the player objects (player, player2 etc.)

    ButtonCollider b; // Scripts attached to the coins objects (coin1, coin2 etc.)

    GameObject t;

    public PlayerSelector p1, p2, p3, p4;

    public int ID = 0;
 

    Transform old;

    private bool isActive;

    public GameObject Car;

    public bool ThisCarSelected = false;

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

        ID = m.playerID;
   
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
                l.num_ready_players++;

                ThisCarSelected = true;
                print(Car.name);
            }

            if (Input.GetButtonDown("ButtonXJoyStick1") && l.is_joy1_taken && m.is_this_inside == true && b.is_player_near)
            {

                t.transform.parent = old;
                Car.GetComponent<Collider2D>().enabled = true;
                l.num_ready_players--;

                ThisCarSelected = false;

            }

        }


        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)

        {

            if (Input.GetButtonDown("ButtonAJoyStick2") && l.is_joy2_taken && m.is_this_inside == true)
            {

                t.transform.parent = Car.transform;
                Car.GetComponent<Collider2D>().enabled = false;
                l.num_ready_players++;
                print("joy2 Select");

                ThisCarSelected = true;
            }

            if (Input.GetButtonDown("ButtonXJoyStick2") && l.is_joy2_taken && m.is_this_inside == true && b.is_player_near)
            {

                t.transform.parent = old;
                Car.GetComponent<Collider2D>().enabled = true;
                l.num_ready_players--;
                print("joy2 Taken");

                ThisCarSelected = false;
            }

        }

        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.ArrowKeys)

        {

            if (Input.GetButtonDown("ButtonAArrows") && l.is_arrowKeys_taken && m.is_this_inside == true)
            {

                t.transform.parent = Car.transform;
                Car.GetComponent<Collider2D>().enabled = false;
                l.num_ready_players++;
                print("arrows Select");

                ThisCarSelected = true;
            }

            if (Input.GetButtonDown("ButtonXArrows") && l.is_arrowKeys_taken && m.is_this_inside == true && b.is_player_near)
            {

                t.transform.parent = old;
                Car.GetComponent<Collider2D>().enabled = true;
                l.num_ready_players--;
                print("arrows Taken");
                ThisCarSelected = false;
            }

        }

        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.WSDA)

        {

            if (Input.GetButtonDown("ButtonAWSDA") && l.is_wsda_taken && m.is_this_inside == true)
            {

                t.transform.parent = Car.transform;
                Car.GetComponent<Collider2D>().enabled = false;
                l.num_ready_players++;
                print("wsda Select");

                ThisCarSelected = true;
            }

            if (Input.GetButtonDown("ButtonXWSDA") && l.is_wsda_taken && m.is_this_inside == true && b.is_player_near)
            {

                t.transform.parent = old;
                Car.GetComponent<Collider2D>().enabled = true;
                l.num_ready_players--;
                print("wsda Taken");

                ThisCarSelected = false;
            }

        }

       

    }

    public void CheckPlayerActivation() {

       // if (p2.switch_case == 0)
        {

            t.transform.parent = old;
            t.transform.position = m.CoinPosition;
            TextColorCar.color = Color.white;
            Car.GetComponent<Collider2D>().enabled = true;

            if (ThisCarSelected) { l.num_ready_players--; }

            ThisCarSelected = false;

        }

    }

}
