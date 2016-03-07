using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CarMenu : MonoBehaviour {


    public Text TextColor;
    public Player p;
    bool is_inside = false;

    void Start()
    {
        TextColor = GetComponent<Text>();
        p = GameObject.FindWithTag("PlayerMenu").GetComponent<Player>();
    }

    void Update() {
        if (Input.GetKey(KeyCode.X)&&is_inside)
        {
            p.notSelected = false;
        }

        if (Input.GetKey(KeyCode.A)&&is_inside&&(!p.notSelected))
        {
            p.notSelected = true;
        }
    }


    void OnTriggerEnter2D (Collider2D trigger)
    {
        print("Trigger");

        TextColor.color = Color.yellow;
        is_inside = true;
    }

    //void OnTriggerStay2D(Collider2D trigger)
    //{
    //    print("STAY");
    //    // TextColor.color = Color.yellow;
    //    if (Input.GetKey(KeyCode.X))
    //    {
    //        p.notSelected = false;
    //    }

    //}

    void OnTriggerExit2D(Collider2D trigger)
    {
        print("EXIT");
        TextColor.color = Color.white;
        is_inside = false;
    }

}
