using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IconCollider : MonoBehaviour {


    Text TextColorCar;
    LevelManager l;
    MoveSelector m;

    GameObject t;
    Transform old;

    public GameObject Car;

    void Start()
    {
        l = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();

    }

    void Update() {

        if ((Input.GetButtonDown("ButtonAJoyStick1") || Input.GetButtonDown("ButtonAJoyStick2")) && m.is_this_inside == true)
        {
            // t = trigger.GetComponentInChildren<Text>();

            t.transform.parent = Car.transform;
            print("Elio");

        }

        if ((Input.GetButtonDown("ButtonXJoyStick1") || Input.GetButtonDown("ButtonXJoyStick2")) && m.is_this_inside == true)
        {
            

            t.transform.parent = old;
            print("Elio");

        }

    }


    //Function to detect enter in the trigger area of the Car icon selection
    void OnTriggerEnter2D(Collider2D trigger)
    {

        TextColorCar = this.GetComponent<Text>();
        TextColorCar.color = Color.yellow;
        m = trigger.GetComponent<MoveSelector>();

        m.is_this_inside = true;

        
        t = trigger.gameObject;
        old = t.transform.parent;

        print("Trigger");


    }

    //Function to detect exit from the trigger area of the Car selection icon
    void OnTriggerExit2D(Collider2D trigger)
    {
        TextColorCar = this.GetComponent<Text>();
        TextColorCar.color = Color.white;

        m.is_this_inside = false;

        print("Exit");


    }

    //public void Example(Transform newParent)
    //{
    //    //Sets "newParent" as the new parent of the player GameObject.
    //    player.transform.SetParent(newParent);

    //    //Same as above, except this makes the player keep its local orientation rather than its global orientation.
    //    player.transform.SetParent(newParent, false);

    //    player.transform.parent = newParent.transform;
    //}

}
