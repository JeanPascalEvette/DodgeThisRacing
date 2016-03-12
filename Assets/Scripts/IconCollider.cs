using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IconCollider : MonoBehaviour {


    Text TextColorCar;
    LevelManager l;
    MoveSelector m,n;

    GameObject t;
    Transform old;

    private bool isActive;

   public GameObject Car;

    void Start()
    {
        l = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
        isActive = false;
        
    }

    void Update() {

        
        //if (((Input.GetButtonDown("ButtonAJoyStick1") && l.is_joy1_taken) || (Input.GetButtonDown("ButtonAJoyStick2") && l.is_joy2_taken)) && m.is_this_inside == true)
        //{
        //    // t = trigger.GetComponentInChildren<Text>();

        //    t.transform.parent = Car.transform;
        //    print("Elio");

        //}

        if (isActive)
        {
            checkControlType();
        }
        

        //if ((Input.GetButtonDown("ButtonXJoyStick1") || Input.GetButtonDown("ButtonXJoyStick2")) && m.is_this_inside == true)
        //{
            

        //    t.transform.parent = old;
        //    print("Elio");

        //}

    }


    //Function to detect enter in the trigger area of the Car icon selection
    void OnTriggerEnter2D(Collider2D trigger)
    {
        isActive = true;
        TextColorCar = this.GetComponent<Text>();
        TextColorCar.color = Color.yellow;
        m = trigger.GetComponentInParent<MoveSelector>();

        //n = trigger.GetComponentInParent<MoveSelector>();

        m.is_this_inside = true;

        
        t = trigger.gameObject;
        old = t.transform.parent;

        

        //print("Trigger " + gameObject.name);


    }

    //Function to detect exit from the trigger area of the Car selection icon
    void OnTriggerExit2D(Collider2D trigger)
    {
        isActive = false;
        TextColorCar = this.GetComponent<Text>();
        TextColorCar.color = Color.white;

        m.is_this_inside = false;

        //print("Exit");


    }

    //public void Example(Transform newParent)
    //{
    //    //Sets "newParent" as the new parent of the player GameObject.
    //    player.transform.SetParent(newParent);

    //    //Same as above, except this makes the player keep its local orientation rather than its global orientation.
    //    player.transform.SetParent(newParent, false);

    //    player.transform.parent = newParent.transform;
    //}

    void checkControlType() {

        if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy1)

        {

            if (Input.GetButtonDown("ButtonAJoyStick1") && l.is_joy1_taken && m.is_this_inside == true)
            {

                t.transform.parent = Car.transform;
                print(Car.name);
            }

            if (Input.GetButtonDown("ButtonXJoyStick1") && l.is_joy1_taken && m.is_this_inside == true)
            {

                t.transform.parent = old;
                //print("Elio2");
            }

        }


        else if (m.ThisPlayerControl == MoveSelector.ControlTypesHere.Joy2)

        {

            if (Input.GetButtonDown("ButtonAJoyStick2") && l.is_joy2_taken && m.is_this_inside == true)
            {

                t.transform.parent = Car.transform;
                print("Elio");
            }

            if (Input.GetButtonDown("ButtonXJoyStick2") && l.is_joy2_taken && m.is_this_inside == true)
            {

                t.transform.parent = old;
                print("Elio2");
            }

        }


    }

}
