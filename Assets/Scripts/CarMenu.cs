using UnityEngine;
using System.Collections;

public class CarMenu : MonoBehaviour {

    
    void OnTriggerEnter2D (Collider2D trigger)
    {
        print("Trigger");
    }
    
    void OnTriggerExit2D(Collider2D trigger)
    {
        print("EXIT");
    }

}
