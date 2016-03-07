using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CarMenu : MonoBehaviour {


    public Text TextColor;

    void Start()
    {
        TextColor = GetComponent<Text>();
    }

    void OnTriggerEnter2D (Collider2D trigger)
    {
        print("Trigger");

        TextColor.color = Color.yellow;

    }
    
    void OnTriggerExit2D(Collider2D trigger)
    {
        print("EXIT");
        TextColor.color = Color.white;
    }

}
