using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IconCollider : MonoBehaviour {


    Text TextColorCar;
    LevelManager l;

    void Start()
    {
        l = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();

    }


    //Function to detect enter in the trigger area of the Car icon selection
    void OnTriggerEnter2D(Collider2D trigger)
    {

        TextColorCar = this.GetComponent<Text>();
        TextColorCar.color = Color.yellow;
        l.is_inside = true;
        print("Trigger");


    }

    //Function to detect exit from the trigger area of the Car selection icon
    void OnTriggerExit2D(Collider2D trigger)
    {
        TextColorCar = this.GetComponent<Text>();
        TextColorCar.color = Color.white;
        l.is_inside = false;
        print("Exit");


    }

}
